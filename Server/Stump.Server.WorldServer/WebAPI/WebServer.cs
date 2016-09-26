using Microsoft.Owin.Hosting;
using Stump.Core.Attributes;
using Stump.Server.BaseServer.Initialization;
using System;

namespace Stump.Server.WorldServer.WebAPI
{
    public class WebServer
    {
        [Variable(false)]
        public static int Port = 9000;

        [Initialization(InitializationPass.Last)]
        public static void Initialize()
        {
            try
            {
                // Start OWIN host 
                WebApp.Start<Startup>(url: $"http://{WorldServer.Host}:{Port}/");
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot start WebAPI: {ex.ToString()}");
            }
        }
    }
}
