using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config?.MapHttpAttributeRoutes();

            config?.Routes.MapHttpRoute(
                name: "DFCApi",
                routeTemplate: "dfcapi/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}