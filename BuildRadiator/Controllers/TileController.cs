using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Http;

using BuildRadiator.Helpers;
using BuildRadiator.Model;

namespace BuildRadiator.Controllers {
  public class TileController: ApiController {

    private static readonly IDictionary<string, IReadOnlyCollection<Tile>> Tiles = new Dictionary<string, IReadOnlyCollection<Tile>>();
    private static readonly string DefaultConfiguration;

    static TileController() {
      DefaultConfiguration = ConfigurationManager.AppSettings["DefaultConfiguration"];

      var confPath = HttpContext.Current.Server.MapPath( @"~/App_Data/" );
      var configurationFiles = Directory.GetFiles( confPath, "*_tileconf.json" );

      foreach ( var configurationFile in configurationFiles ) {

        if ( string.IsNullOrEmpty( configurationFile ) ) {
          continue;
        }

        var name = Path.GetFileNameWithoutExtension( configurationFile ).Split( '_' )[0];

        if ( File.Exists( configurationFile ) ) {
          Tiles[name] = TileConfiguration.ReadFromFile( configurationFile );
        }
      }
    }

    public IEnumerable<Tile> Get( string id = "" ) {
      var confName = string.IsNullOrEmpty( id ) ? DefaultConfiguration : id;
      return Tiles[confName];
    }
  }
}