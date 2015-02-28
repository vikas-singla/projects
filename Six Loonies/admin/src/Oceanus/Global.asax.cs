using System.Web.Mvc;
using System.Web.Routing;

namespace Oceanus
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Root", // Route name
                "", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Category", // Route name
                "Category/Delete/{id}", // URL with parameters
                new { controller = "Category" , Action="Delete", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "PhysicalVendorLocations", // Route name
                "AjaxCreate", // URL with parameters
                new { controller = "PhysicalVendorLocations", Action = "AjaxCreate" } // Parameter defaults
            );

            routes.MapRoute(
                "Listings",
                "Listings/{categoryId}",
                new { controller = "Listings", action = "Index", categoryId = UrlParameter.Optional }
            );

            routes.MapRoute(
                "SignupDelete",
                "SignUp/Delete/{emailAddress}",
                new { controller = "SignUp", action = "Delete"}
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                //new { controller = "SignUp", action = "Create" }
            );

            // uncomment to see route mapping
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}