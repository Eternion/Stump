
using System;
using Stump.Server.WorldServer;

namespace Stump.GUI.WorldConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            var server = new WorldServer();

            try
            {
                server.Initialize();
                server.Start();

                while (server.Running)
                {
                    server.Update();
                }
            }
            catch (Exception e)
            {
                server.HandleCrashException(e);
            }
            finally
            {
                server.Shutdown();
            }
        }
    }
}
