
using System;
using System.Threading;

namespace Stump.Tools.Proxy
{
    public class Program
    {
        private static void Main()
        {
            var proxy = Proxy.Instance;

            proxy.Initialize();

            while (true)
                proxy.Update();
        }
    }
}