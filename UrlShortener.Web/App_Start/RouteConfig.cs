using System.Web.Mvc;
using System.Web.Routing;

namespace UrlShortener.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "RedirectRoute",
                url: "{shortUrl}",
                defaults: new { controller = "Home", action = "RedirectUrl", shortenedUrl = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
