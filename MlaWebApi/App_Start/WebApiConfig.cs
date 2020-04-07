using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MlaWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {action=RouteParameter.Optional, id = RouteParameter.Optional });

            /*
            config.Routes.MapHttpRoute(
                           name: "UserGroupAPI",
                           routeTemplate: "api/userGroup/{action}/{id}",
                           defaults: new { id = RouteParameter.Optional });

*/
        }
    }
}
