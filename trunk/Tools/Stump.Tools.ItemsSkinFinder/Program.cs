using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinFinder = Stump.Tools.ItemsSkinFinder.Finder.ItemsSkinFinder;

namespace Stump.Tools.ItemsSkinFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var skinFinder = new SkinFinder();
            skinFinder.Load();

            //TODO do a real program...

            Console.WriteLine("all done !");
            Console.ReadKey(true);
        }
    }
}
