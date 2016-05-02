using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

using BuildRadiator.Helpers;
using BuildRadiator.Model;

namespace BuildRadiator.Controllers {
  public class TileController: ApiController {
    private static readonly IReadOnlyCollection<Tile> Tiles;

    static TileController() {
      // hardcoded reference to HttpContext
      var jsonConf = HttpContext.Current.Server.MapPath( @"~/App_Data/tileconf.json" );
      if ( File.Exists( jsonConf )) {
        Tiles = TileConfiguration.ReadFromFile( jsonConf );
      }
      else {
        Tiles = new Tile[] { new MessageTile( "no tiles", "noTilesFound" ) };
      }
    }

    public IEnumerable<Tile> Get() {
      return Tiles;
    }
  }
}