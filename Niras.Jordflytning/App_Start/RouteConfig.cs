using System.Web.Mvc;
using System.Web.Routing;

namespace Niras.Jordflytning.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Historiske zip filer
            routes.MapRoute(
                "HistorikArkiv.zip",
                "HistorikArkiv/{id}.zip",
                new { controller = "Default", action = "HistorikArkiv" }
            );
            routes.MapRoute(
                "HistorikArkiv",
                "HistorikArkiv/{id}",
                new { controller = "Default", action = "HistorikArkiv" }
            );

            // Blanket pdf filer
            routes.MapRoute(
                "Blanket.pdf",
                "Blanketter/{id}.pdf",
                new { controller = "Default", action = "Blanketter" }
            );

            routes.MapRoute(
                "BlanketID",
                "Blanketter/{id}",
                new { controller = "Default", action = "Blanketter" }
            );

            routes.MapRoute(
					name: "Default",
					url: "{controller}/{action}/{id}",
					namespaces: new []{"Niras.Jordflytning.Controllers"},
					defaults: new { controller = "Default", action = "FrontPage", id = UrlParameter.Optional }
			);
        }
    }
}