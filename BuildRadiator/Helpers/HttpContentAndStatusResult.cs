using System;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace BuildRadiator.Helpers {
  /// <summary>
  /// Class to return an http status code and content (based on ContentResult and HttpStatusCodeResult)
  /// </summary>
  public class HttpContentAndStatusResult: ActionResult {
    public int StatusCode { get; set; }

    public string StatusDescription { get; set; }

    public string Content { get; set; }

    public Encoding ContentEncoding { get; set; }

    public string ContentType { get; set; }

    public HttpContentAndStatusResult() {
      StatusCode = (int) HttpStatusCode.OK;
    }

    public override void ExecuteResult( ControllerContext context ) {
      if ( context == null ) {
        throw new ArgumentNullException( "context" );
      }

      var response = context.HttpContext.Response;

      response.StatusCode = StatusCode;

      if ( StatusDescription != null ) {
        response.StatusDescription = StatusDescription;
      }

      if ( !string.IsNullOrEmpty( ContentType ) ) {
        response.ContentType = ContentType;
      }

      if ( ContentEncoding != null ) {
        response.ContentEncoding = ContentEncoding;
      }

      if ( Content != null ) {
        response.Write( Content );
      }
    }
  }
}