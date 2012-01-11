
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.Core.IO;
using Stump.Server.BaseServer.Network;

namespace Stump.Tools.Proxy
{
    public class Program
    {
        private static void Main()
        {
            var proxy = Proxy.Instance;

            proxy.Initialize();

            while (true)
                Thread.Yield();
        }
    }
}