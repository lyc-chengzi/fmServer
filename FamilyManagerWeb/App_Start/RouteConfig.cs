using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FamilyManagerWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "InOutInfo",
                url: "ConsumptionInfo/{id}",
                defaults: new { controller = "ApplySub", action = "List", id = UrlParameter.Optional },
                namespaces: new string[] { "FamilyManage.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "toLogin", id = UrlParameter.Optional },
                namespaces: new string[] { "FamilyManagerWeb.Controllers" }
            );
        }
    }
}