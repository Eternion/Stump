using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Stump.Core.Attributes;
using Stump.Core.Xml.Config;
using Stump.Core.Xml.Docs;

namespace PostBuild
{
    internal class Program
    {
        /// <summary>
        ///   Stuff to be executed after Stump has been built
        /// </summary>
        /// <param name = "args"></param>
        private static void Main(string[] args)
        {
            // TODO : Docs commands & msdn'like
            // TODO : generate config file

            AppDomain.CurrentDomain.UnhandledException += (sender, e) => Console.WriteLine("Exception raised while generating config : " + e.ExceptionObject);
            //Debugger.Launch();
            if (args[0] == "-config")
            {
                string path = args[1].Replace("\"", "");

                string output = Path.Combine(path, "default_config.xml");
                var config = new XmlConfig(output); 

                foreach (string file in Directory.EnumerateFiles(path, "Stump*.dll"))
                {
                    Assembly asm;
                    try
                    {
                        if (Path.GetFileNameWithoutExtension(file) != "Stump.Core")
                            asm = Assembly.LoadFrom(file);
                        else
                            continue;
                    }
                    catch
                    {
                        continue;
                    }

                    string docPath = Path.Combine(path, Path.GetFileNameWithoutExtension(file) + ".xml");

                    if (File.Exists(docPath))
                        config.AddAssembly(asm, docPath);
                    else
                        config.AddAssembly(asm);

                }

                config.Create(true);

                Console.WriteLine("Default config file generetad : {0}", output);
            }
        }
    }
}