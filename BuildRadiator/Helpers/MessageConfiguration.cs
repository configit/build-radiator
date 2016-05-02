using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BuildRadiator.Model;

using Newtonsoft.Json.Linq;

namespace BuildRadiator.Helpers {
  public class MessageConfiguration {
    public static IDictionary<string, Message> ReadFromFile( string confFile ) {
      return Convert( File.ReadAllText( confFile ) );
    }

    public static IDictionary<string, Message> Convert( string json ) {
      var dic = new Dictionary<string, Message>();
      var jobj = JArray.Parse( json );
      foreach ( var jsonMessage in jobj ) {
        var key = jsonMessage.Value<string>( "key" );
        dic.Add( key, Convert( jsonMessage ) );
      }
      return dic;
    }

    private static Message Convert( JToken jsonMessage ) {
      var type = jsonMessage.Value<string>( "type" );
      var title = jsonMessage.Value<string>( "title" );
      var classes = jsonMessage.Value<JArray>( "classes" ).ToObject<string[]>();
      if ( type == "theme" ) {
        var summary = jsonMessage.Value<string>( "summary" );
        return new Message( BuildTheme( title, summary ), classes );
      }

      if ( type == "message" ) {
        var messages = jsonMessage.Value<JArray>( "messages" ).ToObject<string[]>();
        return new Message( BuildMessage( title, messages ), classes );
      }

      throw new ArgumentException( "Tile type not suppored: " + jsonMessage );
    }

    private static string BuildTheme( string title, string summary ) {
      var sb = new StringBuilder();

      sb.Append( "<div class=\"title\">" );
      sb.Append( title );
      sb.Append( "</div>" );

      sb.Append( "<div class=\"quote\">" );
      sb.Append( summary );
      sb.Append( "</div>" );

      return sb.ToString();
    }

    private static string BuildMessage( string title, params string[] messages ) {
      var sb = new StringBuilder();

      sb.Append( "<div>" );
      sb.Append( title );
      sb.Append( "</div>" );

      if ( messages != null ) {
        foreach ( var message in messages ) {
          sb.Append( "<div class=\"small\">" );
          sb.Append( message );
          sb.Append( "</div>" );
        }
      }

      return sb.ToString();
    }
  }
}