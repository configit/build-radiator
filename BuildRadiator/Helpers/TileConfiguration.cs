using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using BuildRadiator.Model;

using Newtonsoft.Json.Linq;

namespace BuildRadiator.Helpers {
  public class TileConfiguration {
    public static IReadOnlyCollection<Tile> ReadFromFile( string confFile ) {
      return Convert( File.ReadAllText( confFile ) );
    }

    public static IReadOnlyCollection<Tile> Convert( string json ) {
      var jobj = JArray.Parse( json );
      return jobj.Select( Convert ).ToList();
    }

    private static Tile Convert( JToken tileJson ) {
      var caption = tileJson.Value<string>( "caption" );
      var config = tileJson.Value<JToken>( "config" );
      var columnSpan = tileJson.Value<int>( "columnSpan" );
      var rowSpan = tileJson.Value<int>( "rowSpan" );

      Tile convertedTile;

      switch ( tileJson.Value<string>( "type" ) ) {
        case "project":
          var buildName = config.Value<string>( "buildName" );
          var branchName = config.Value<string>( "branchName" );
          convertedTile = new ProjectTile( caption, buildName, branchName );
          break;
        case "message":
          var messageKey = config.Value<string>( "messageKey" );
          var classes = config.Value<JArray>( "classes" ).ToObject<string[]>();
          convertedTile = new MessageTile( caption, messageKey, classes );
          break;
        case "clock":
          var timezone = config.Value<string>( "timezone" );
          convertedTile = new ClockTile( caption, timezone );
          break;
        default:
          throw new ArgumentException( "Tile type not suppored: " + tileJson );
      }
      convertedTile.ColumnSpan = columnSpan;
      convertedTile.RowSpan = rowSpan;
      return convertedTile;
    }

  }
}