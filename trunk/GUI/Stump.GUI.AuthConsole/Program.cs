
using System;
using System.Diagnostics;
using System.Threading;
using Stump.Server.AuthServer;

namespace Stump.GUI.AuthConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            var server = new AuthServer();
            if (!Debugger.IsAttached)
            {
                try
                {
                    server.Initialize();
                    server.Start();

                    GC.Collect();

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
            else
            {
                server.Initialize();
                server.Start();

                GC.Collect();

                while (server.Running)
                {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
