
using System;
using System.Diagnostics;
using System.Threading;
using Stump.Core.IO;
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

                    while (true)
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

                while (true)
                {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
