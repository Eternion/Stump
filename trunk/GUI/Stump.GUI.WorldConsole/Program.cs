
using System;
using System.Threading;
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
                    Thread.Sleep(5000);
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
