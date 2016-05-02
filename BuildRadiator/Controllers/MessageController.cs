using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Http;

using BuildRadiator.Helpers;
using BuildRadiator.Model;

namespace BuildRadiator.Controllers {
  public class MessageController: ApiController {
    private static readonly IDictionary<string, Message> Messages;
    private static readonly IDictionary<string, Func<Message>> DynamicMessages;

    static MessageController() {
      // hardcoded reference to HttpContext
      var jsonConf = HttpContext.Current.Server.MapPath( @"~/App_Data/messageconf.json" );
      if ( File.Exists( jsonConf ) ) {
        Messages = MessageConfiguration.ReadFromFile( jsonConf );
      }

      DynamicMessages = new Dictionary<string, Func<Message>>();
    }



    public Message Get( string id ) {
      if ( Messages.ContainsKey( id ) ) {
        return Messages[id];
      }

      if ( DynamicMessages.ContainsKey( id ) ) {
        return DynamicMessages[id]();
      }

      return new Message( "UNKNOWN: " + id );
    }
  }
}