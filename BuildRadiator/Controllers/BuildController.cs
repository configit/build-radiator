using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using BuildRadiator.Helpers;
using BuildRadiator.Model.Builds;

namespace BuildRadiator.Controllers {
  [Authorize]
  public class BuildController: ApiController {

    private HttpClient CreateClient() {
      var user = (TeamCityPrincipal) User;

      var client = new HttpClient();
      client.BaseAddress = new Uri( ConfigurationManager.AppSettings["TeamCityUrl"].TrimEnd( '/' ) + "/httpAuth/app/rest/latest/" );
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Basic", user.AuthenticationHeader );

      return client;
    }

    public async Task<Build> Get( string buildType, string branchName ) {
      var client = CreateClient();

      var buildInfoTask = Task.Run( () => GetBuildInfo( client, buildType, branchName ) );
      var investigationInfoTask = Task.Run( () => GetInvestigationInfo( client, buildType ) );
      var changesSinceFailureInfoTask = Task.Run( () => GetChangesSinceFailureInfo( client, buildType, branchName ) );

      await Task.WhenAll( buildInfoTask, investigationInfoTask, changesSinceFailureInfoTask );

      var parser = new BuildParser( buildInfoTask.Result, investigationInfoTask.Result, changesSinceFailureInfoTask.Result );

      return parser.Parse();
    }

    private static async Task<XmlDocument> GetBuildInfo( HttpClient client, string buildType, string branchName ) {
      var url = "builds/buildType:name:" + Uri.EscapeDataString( buildType ) + ",running:any,branch:" + Uri.EscapeUriString( branchName ) + ",count:1?fields=buildType,branchName,status,statusText,startDate,finishDate,running,running-info";
      var cacheBuster = "&t=" + DateTime.UtcNow.Ticks;

      var data = await client.GetStringAsync( url + cacheBuster );

      var document = new XmlDocument();
      document.LoadXml( data );

      return document;
    }

    private static async Task<XmlDocument> GetInvestigationInfo( HttpClient client, string buildType ) {
      var url = "investigations?locator=buildType:name:" + Uri.EscapeDataString( buildType ) + "&fields=investigation(state,assignee(name,email),assignment(text,user(name,email)))";
      var cacheBuster = "&t=" + DateTime.UtcNow.Ticks;

      var data = await client.GetStringAsync( url + cacheBuster );

      var document = new XmlDocument();
      document.LoadXml( data );

      return document;
    }

    private static async Task<XmlDocument> GetChangesSinceFailureInfo( HttpClient client, string buildType, string branchName ) {
      var buildLocator = "buildType:name:" + Uri.EscapeDataString( buildType ) + ",branch:" + Uri.EscapeDataString( branchName );
      var url = "builds/?locator=" + buildLocator + ",status:failure,running:false,sinceBuild:(" + buildLocator + ",status:success,running:false)&fields=build(changes(change(username,user(email))))";
      var cacheBuster = "&t=" + DateTime.UtcNow.Ticks;

      var document = new XmlDocument();
      try {
        var data = await client.GetStringAsync( url + cacheBuster );
        document.LoadXml( data );
      } catch {
        Debug.WriteLine( "Could not load changes, probably because there are no successful builds" );
      }
      return document;

    }
  }
}