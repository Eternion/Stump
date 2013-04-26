using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.Targets;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Xml.Config;
using Stump.ORM;
using Stump.Server.WorldServer;
using Stump.Tools.CacheManager.Maps;

namespace Stump.Tools.CacheManager
{
    internal class Program
    {
        private static DatabaseAccessor m_dbAccessor;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public const string ConfigFile = "config.xml";

        [Variable(true)]
        public static string WorldConfigPath = "../../../Run/Debug/WorldServer/world_config.xml";

        /// <summary>
        /// Store only the text of given languages separated by a comma. Or leave blank to store all texts
        /// </summary>
        [Variable(true)]
        public static string SpecificLanguage = "fr,en";

        [Variable(true)]
        public static string DofusCustomPath = "";

        [Variable(true)]
        public static string PatchsFolder = "world_patchs";

        private static Tuple<string, Action>[] m_menus = new[]
        {
            Tuple.Create<string, Action>("Set Dofus Path (empty = default)", SetDofusPath),
            Tuple.Create<string, Action>("Set languages (empty = all)", SetLanguages),
            Tuple.Create<string, Action>("Load maps", LoadMaps),
            Tuple.Create<string, Action>("Load D2Os (empty = all)", LoadD2Os),
            Tuple.Create<string, Action>("Load D2Is", LoadD2Is),
            Tuple.Create<string, Action>("Load patchs (empty = all)", LoadPatchs),
            Tuple.Create<string, Action>("Load all (empty = all)", LoadAll)
        };


        private static XmlConfig m_config;
        private static bool m_dbOpened;
        private static DatabaseBuilder m_databaseBuilder;


        private static void Main(string[] args)
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            consoleTarget.Layout = "${message}";

                config.AddTarget("console", consoleTarget);

            var rule = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;


            m_config = new XmlConfig(ConfigFile);
            m_config.AddAssembly(typeof(Program).Assembly);
            if (!File.Exists(ConfigFile))
                m_config.Create();
            else
                m_config.Load();

            while (true)
            {
                ShowMenus();

                try
                {
                    var input = Console.ReadLine();
                    int number;
                    while (!int.TryParse(input, out number) || number < 1 || number > m_menus.Length)
                    {
                        Console.WriteLine("Give a valid number between 1 and {0}", m_menus.Length);
                        input = Console.ReadLine();
                    }

                    Console.Clear();
                    m_menus[number - 1].Item2();
                }
                catch (Exception ex)
                {
                    logger.Error("Error : " + ex);
                }

                Console.WriteLine("Press Enter to return to the menu");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static void ShowMenus()
        {
            Console.WriteLine("Dofus path = {0}", FindDofusPath());
            Console.WriteLine("Languages = {0}", SpecificLanguage);
            Console.WriteLine();
            Console.WriteLine("What to do ?");
            int number = 1;
            foreach (var menu in m_menus)
            {
                Console.WriteLine(number + ". " + menu.Item1);
                number++;
            }
        }

        #region Menus actions
        private static void SetDofusPath()
        {
            Console.WriteLine("Enter dofus path (empty = default path):");
            var path = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Path '{0}' doesn't exists", path);
            }
            else
            {
                DofusCustomPath = path;

                m_config.Save();

                Console.WriteLine("Path changed (saved)");
            }
        }

        private static void SetLanguages()
        {
            Console.WriteLine("Enter languages (default = fr,en) :");
            var langs = Console.ReadLine();
            SpecificLanguage = langs;

            m_config.Save();

            Console.WriteLine("Languages changed (saved)");
        }

        private static void LoadMaps()
        {
            OpenDatabase();

            string mapsFolder = Path.Combine(FindDofusPath(), "content", "maps");

            if (!Directory.Exists(mapsFolder))
                logger.Error("{0} doesn't exists", mapsFolder);
            else
            {
                var mapBuilder = new MapLoader(m_dbAccessor);
                mapBuilder.LoadMaps(mapsFolder);
            }
        }

        private static void LoadD2Os()
        {
            OpenDatabase();

            string d2OFolder = Path.Combine(FindDofusPath(), "data", "common");

            if (!Directory.Exists(d2OFolder))
                logger.Error("{0} doesn't exists", d2OFolder);
            else
            {
                Console.WriteLine("Enter the tables to build (separated by comma, empty = all)");
                var tables = Console.ReadLine().Split(',');
            
                m_databaseBuilder.BuildD2OTables(d2OFolder, tables);
            }
        }

        private static void LoadD2Is()
        {
            OpenDatabase();

            string d2IFolder = Path.Combine(FindDofusPath(), "data", "i18n");

            if (!Directory.Exists(d2IFolder))
                logger.Error("{0} doesn't exists", d2IFolder);
            else
            {
                m_databaseBuilder.BuildD2ITables(d2IFolder);
            }
        }

        private static void LoadPatchs()
        {
            OpenDatabase();

            if (!Directory.Exists(PatchsFolder))
                logger.Error("{0} doesn't exists", Path.GetFullPath(PatchsFolder));
            else
            {
                foreach (var file in Directory.EnumerateFiles(PatchsFolder))
                {
                    Console.WriteLine(" - " + Path.GetFileName(file));
                }

                Console.WriteLine("Enter the patchs to build (separated by comma, empty = all)");
                var patchs = Console.ReadLine().Split(',');

                m_databaseBuilder.ExecutePatchs(PatchsFolder, patchs);
            }
        }

        private static void LoadAll()
        {
            string dofusPath = FindDofusPath();
            string d2OFolder = Path.Combine(dofusPath, "data", "common");
            string d2IFolder = Path.Combine(dofusPath, "data", "i18n");
            string mapsFolder = Path.Combine(dofusPath, "content", "maps");

            // build maps
            var mapBuilder = new MapLoader(m_dbAccessor);
            mapBuilder.LoadMaps(mapsFolder);

            var dbBuilder = new DatabaseBuilder(m_dbAccessor, typeof(WorldServer).Assembly);

            m_databaseBuilder.BuildD2OTables(d2OFolder);
            m_databaseBuilder.BuildD2ITables(d2IFolder);
            m_databaseBuilder.ExecutePatchs(PatchsFolder);
        }
#endregion

        private static void OpenDatabase()
        {
            if (m_dbOpened)
                return;

            XmlConfig worldConfig;
            if (!string.IsNullOrEmpty(Path.GetFullPath(WorldConfigPath)))
            {
                logger.Info("Opening World Config");
                worldConfig =
                    new XmlConfig(WorldConfigPath)
                    {
                        IgnoreUnloadedAssemblies = true
                    };
                worldConfig.AddAssembly(typeof(WorldServer).Assembly);
                worldConfig.Load();
            }

            logger.Info("Opening World Database");
            m_dbAccessor = new DatabaseAccessor(WorldServer.DatabaseConfiguration);
            m_dbAccessor.RegisterMappingAssembly(Assembly.GetAssembly(typeof(WorldServer)));
            m_dbAccessor.Initialize();
            m_dbAccessor.OpenConnection();
            m_databaseBuilder = new DatabaseBuilder(m_dbAccessor, typeof(WorldServer).Assembly);

            m_dbOpened = true;
        }

        private static string FindDofusPath()
        {
            if (!string.IsNullOrEmpty(DofusCustomPath))
            {
                return DofusCustomPath;
            }

            string programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");

            if (string.IsNullOrEmpty(programFiles))
                programFiles = Environment.GetEnvironmentVariable("programfiles");

            if (string.IsNullOrEmpty(programFiles))
                return Path.Combine(AskDofusPath(), "app");

            if (Directory.Exists(Path.Combine(programFiles, "Dofus2", "app")))
                return Path.Combine(programFiles, "Dofus2", "app");
            if (Directory.Exists(Path.Combine(programFiles, "Dofus 2", "app")))
                return Path.Combine(programFiles, "Dofus 2", "app");

            var dofusDataPath = Path.Combine(AskDofusPath(), "app");

            if (!Directory.Exists(dofusDataPath))
                throw new Exception("Dofus data path not found");

            return dofusDataPath;
        }

        private static string AskDofusPath()
        {
            logger.Warn("Dofus path not found. Enter Dofus 2 root folder (%programFiles%/Dofus2):");

            return Path.GetFullPath(Console.ReadLine());
        }

        private static void Exit(string reason = "", bool error = false)
        {
            if (!string.IsNullOrEmpty(reason))
                if (error)
                    logger.Error(reason);
                else
                    logger.Info(reason);

            Console.WriteLine("Press enter to exit");
            Console.Read();

            Environment.Exit(-1);
        }
    }
}