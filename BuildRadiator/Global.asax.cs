using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BuildRadiator.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using IAuthorizationFilter = System.Web.Mvc.IAuthorizationFilter;

namespace BuildRadiator {
  public class Global: HttpApplication {

    protected void Application_Start( object sender, EventArgs e ) {
      var authentication = new TeamCityAuthentication();

      ConfigureWebApi( GlobalConfiguration.Configuration, authentication );
      ConfigureMvc( GlobalFilters.Filters, authentication );
      ConfigureBundles( BundleTable.Bundles );
      ConfigureRoutes( RouteTable.Routes );
    }

    private static void ConfigureMvc( GlobalFilterCollection filters, IAuthorizationFilter authenticationFilter ) {
      filters.Add( authenticationFilter );
    }

    private static void ConfigureWebApi( HttpConfiguration configuration, IFilter authenticationFilter ) {
      // Remove WebApi XML serialization
      configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

      // Enable custom authentication
      configuration.Filters.Add( authenticationFilter );

      // Configure Json Serializer
      configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = new JsonConverter[] {
          new StringEnumConverter { CamelCaseText = true }
        }
      };
    }

    private static void ConfigureBundles( BundleCollection bundles ) {
      // Stylesheet bundle
      var styleBundle = new StyleBundle( "~/content/styles" )
        .Include( "~/Content/angular-material.css" )
        .Include( "~/Content/styles.css" );

      // Library scripts
      var scriptBundle = new ScriptBundle( "~/bundles/scripts" )
        .Include( "~/Scripts/jquery-{version}.js" )
        .Include( "~/Scripts/angular.js" )
        .Include( "~/Scripts/moment/moment.js" )
        .Include( "~/Scripts/moment/moment-timezone-with-data-2010-2020.js" )
        .IncludeDirectory( "~/Scripts", "*.js" )
        .Include( "~/Scripts/i18n/angular-locale_en-gb.js" );

      // Angular scripts
      var appBundle = new ScriptBundle( "~/bundles/application" )
        .IncludeDirectory( "~/Client", "*.js", true );

      bundles.Add( styleBundle );
      bundles.Add( scriptBundle );
      bundles.Add( appBundle );
    }

    public static void ConfigureRoutes( RouteCollection routes ) {
      routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );
      routes.IgnoreRoute( "{resource}.ashx/{*pathInfo}" );

      // Don't route anything in the Content, Client or Bundles folder
      routes.Ignore( "Content/{*anything}" );
      routes.Ignore( "Client/{*anything}" );
      routes.Ignore( "bundles/{*anything}" );

      // Web Api Routes
      routes.MapHttpRoute(
        name : "DefaultApi",
        routeTemplate : "api/{controller}/{id}",
        defaults : new { id = RouteParameter.Optional }
      );

      // MVC Routes
      routes.MapRoute(
        name : "Default",
        url : "{controller}/{action}/{id}",
        defaults : new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}