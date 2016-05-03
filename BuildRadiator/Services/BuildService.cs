using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml;

using BuildRadiator.Helpers;
using BuildRadiator.Model.Builds;

namespace BuildRadiator.Services {
  public class BuildService {
    private readonly HttpClient _client;

    public static BuildService CreateWithClient( IPrincipal user ) {
      var tcUser = user as TeamCityPrincipal;
      if ( null == tcUser ) {
        throw new UnauthorizedAccessException( "not a teamcity principal" );
      }

      var client = new HttpClient();
      client.BaseAddress = new Uri( ConfigurationManager.AppSettings["TeamCityUrl"].TrimEnd( '/' ) + "/httpAuth/app/rest/latest/" );
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Basic", tcUser.AuthenticationHeader );
      return new BuildService( client );
    }


    public BuildService( HttpClient client) {
      _client = client;
    }

    public async Task<Build> Get( string buildType, string branchName ) {

      if ( string.IsNullOrEmpty( buildType ) ) {
        throw new ArgumentNullException( nameof( buildType ) );
      }

      var buildInfoTask = Task.Run( () => GetBuildInfo( _client, buildType, branchName ) );
      var investigationInfoTask = Task.Run( () => GetInvestigationInfo( _client, buildType ) );
      var changesSinceFailureInfoTask = Task.Run( () => GetChangesSinceFailureInfo( _client, buildType, branchName ) );

      await Task.WhenAll( buildInfoTask, investigationInfoTask, changesSinceFailureInfoTask );

      var parser = new BuildParser( buildInfoTask.Result, investigationInfoTask.Result, changesSinceFailureInfoTask.Result );

      return parser.Parse();
    }


    private static string GenerateBranchUrlFragment( string branchName ) {
      // HACK: Handle specific case - TC does not seem to be able to handle this specific case
      if ( branchName == "refs/heads/master" ) {
        return string.Empty;
      }

      var branchUrl = string.Empty;
      if ( !string.IsNullOrEmpty( branchName ) && "null" != branchName ) {
        branchUrl = ",branch:" + Uri.EscapeUriString( branchName );
      }
      return branchUrl;
    }

    private static async Task<XmlDocument> GetBuildInfo( HttpClient client, string buildType, string branchName ) {
      var url = "builds/buildType:name:" + Uri.EscapeDataString( buildType ) + ",running:any" + GenerateBranchUrlFragment( branchName ) + ",count:1?fields=buildType,branchName,status,statusText,startDate,finishDate,running,running-info";
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
      var buildLocator = "buildType:name:" + Uri.EscapeDataString( buildType ) + GenerateBranchUrlFragment( branchName );
      var url = "builds/?locator=" + buildLocator + ",status:failure,running:false,sinceBuild:(" + buildLocator + ",status:success,running:false)&fields=build(changes(change(username,user(email))))";
      var cacheBuster = "&t=" + DateTime.UtcNow.Ticks;

      var document = new XmlDocument();
      try {
        var data = await client.GetStringAsync( url + cacheBuster );
        document.LoadXml( data );
      }
      catch {
        Debug.WriteLine( "Could not load changes, probably because there are no successful builds" );
      }
      return document;

    }
  }
}