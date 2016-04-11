using Microsoft.Owin.Hosting;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.WebAPI
{
    public class WebServer
    {
        [Initialization(InitializationPass.First)]
        public static void Initialize()
        {
            // Start OWIN host 
            WebApp.Start<Startup>(url: "http://localhost:9000/");
        }
    }
}
