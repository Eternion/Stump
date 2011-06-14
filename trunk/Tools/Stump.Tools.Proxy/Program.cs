
using System;
using System.Threading;

namespace Stump.Tools.Proxy
{
    public class Program
    {
        private static void Main()
        {
            Proxy.Instance.Initialize();

            while(true)
                Thread.Sleep(20);
        }
    }
}