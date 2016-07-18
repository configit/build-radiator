using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Http;

using BuildRadiator.Helpers;
using BuildRadiator.Model;

namespace BuildRadiator.Controllers {
  public class MessageController: ApiController {

    private static readonly IDictionary<string, IDictionary<string, Message>> Messages = new Dictionary<string, IDictionary<string, Message>>();
    private static readonly string DefaultConfiguration;

    static MessageController() {
      DefaultConfiguration = ConfigurationManager.AppSettings["DefaultMessageConfiguration"];

      var confPath = HttpContext.Current.Server.MapPath( @"~/App_Data/" );
      var configurationFiles = Directory.GetFiles( confPath, "*_messageconf.json" );

      foreach ( var configurationFile in configurationFiles ) {

        if ( string.IsNullOrEmpty( configurationFile ) ) {
          continue;
        }

        var name = Path.GetFileNameWithoutExtension( configurationFile ).Split( '_' )[0];

        if ( File.Exists( configurationFile ) ) {
          Messages[name] = MessageConfiguration.ReadFromFile( configurationFile );
        }
      }
    }

    public Message Get( string id, [FromUri]string confId = "" ) {
      var confName = string.IsNullOrEmpty( confId ) ? DefaultConfiguration : confId;

      if ( Messages.ContainsKey( confName ) ) {
        if ( Messages[ confName ].ContainsKey( id ) ) {
          return Messages[confName][id];
        }
      }

      return new Message( "UNKNOWN: " + id );
    }
  }
}