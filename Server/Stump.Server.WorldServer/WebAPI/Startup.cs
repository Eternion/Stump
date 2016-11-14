using Owin;
using System.Web.Http;

namespace Stump.Server.WorldServer.WebAPI
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host.
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}