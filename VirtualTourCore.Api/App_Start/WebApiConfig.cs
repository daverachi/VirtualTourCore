using System.Web.Http;

namespace VirtualTourCore.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{guid}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
