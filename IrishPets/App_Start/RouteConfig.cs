using System.Web.Mvc;
using System.Web.Routing;

namespace IrishPets
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection _routes)
        {
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            _routes.MapRoute
                (name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
