using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using BuildRadiator.Helpers;
using BuildRadiator.Model;
using BuildRadiator.Model.Builds;
using BuildRadiator.Services;

namespace BuildRadiator.Controllers {
  [Authorize]
  public class BuildController: ApiController {

    public async Task<Build> Get( string buildType, string branchName ) {
        var service = BuildService.CreateWithClient( User );
        return await service.Get( buildType, branchName );
    }

    [HttpPost]
    public async Task<IEnumerable<Build>> GetAllBuilds( [FromBody] BuildDefinition[] projects ) {
      var buildService = BuildService.CreateWithClient( User );
      var buildTasks = projects.Select( project => buildService.Get( project.BuildName, project.BranchName ) );
      return await Task.WhenAll( buildTasks );
    }
  }
}