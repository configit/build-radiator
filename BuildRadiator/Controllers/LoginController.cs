using System.Threading.Tasks;
using System.Web.Mvc;
using BuildRadiator.Helpers;

namespace BuildRadiator.Controllers {
  [AllowAnonymous]
  public class LoginController: Controller {
    public ActionResult Index() {
      return View();
    }

    [HttpPost]
    [ActionName( "Index" )]
    public async Task<ActionResult> IndexPost() {
      var username = Request.Form["username"];
      var password = Request.Form["password"];

      var loginSuccessful = await TeamCityAuthentication.Login( username, password );

      if ( loginSuccessful ) {
        return RedirectToAction( "Index", "Home" );
      }

      ViewBag.LoginError = "Invalid username or password";
      return View();
    }

    public ActionResult Logout() {
      TeamCityAuthentication.Logout();
      return RedirectToAction( "Index", "Home" );
    }
  }
}