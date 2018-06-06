using System.Web.Http;

namespace demo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Create Public folder if it does not exist.
            string publicPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Public");

            bool exists = System.IO.Directory.Exists(publicPath);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(publicPath);
            }

            // Web API routes.
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
