using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;
using Stump.Core.Xml.Config;
using SkinFinder = Stump.Tools.ItemsSkinFinder.Finder.ItemsSkinFinder;

namespace Stump.Tools.ItemsSkinFinder
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public const string ConfigFile = "config.xml";
        private static XmlConfig m_config;

        static void Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget {Layout = "${message}"};
            config.AddTarget("console", consoleTarget);
            var rule = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;


            Console.WriteLine("Load the config file...");

            m_config = new XmlConfig(ConfigFile);
            m_config.AddAssembly(typeof(Program).Assembly);
            if (!File.Exists(ConfigFile))
                m_config.Create();
            else
                m_config.Load();

            var skinFinder = new SkinFinder();
            skinFinder.Load();

            // A REFAIRE 
            var couples = skinFinder.Find();

            // POUR TESTER
            var stream = new StreamWriter("./test.txt", false, Encoding.ASCII);
            foreach (var couple in couples)
            {
                stream.WriteLine(couple.Item1 + ',' + couple.Item2);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);
        }
    }
}
