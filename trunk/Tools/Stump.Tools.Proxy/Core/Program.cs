using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stump.Tools.Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Proxy.Initialize();
            Console.WriteLine("Proxy enabled");
            Console.ReadLine();
        }
    }
}
