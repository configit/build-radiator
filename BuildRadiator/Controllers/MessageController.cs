using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using BuildRadiator.Model;

namespace BuildRadiator.Controllers {
  public class MessageController: ApiController {
    private static readonly IDictionary<string, Message> Messages;
    private static readonly IDictionary<string, Func<Message>> DynamicMessages;

    static MessageController() {
      var standardMessage = new[] { "standard" };

      Messages = new Dictionary<string, Message> {
        { "lastRelease", new Message( BuildMessage( "2.4.2", "Xenon Patch 2", "07 Jan 2016" ), standardMessage ) },
        { "sprintTheme", new Message( BuildTheme( "Logging &amp; Traceability", "Provide detailed information of changes within Ace to allow investigations into what happened and when"), "fancy" ) }
      };

      DynamicMessages = new Dictionary<string, Func<Message>>();
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