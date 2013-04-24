using System;
using System.IO;
using System.Reflection;
using NLog;
using Stump.Core.IO;
using Stump.Core.Xml.Config;
using Stump.ORM;
using Stump.Server.WorldServer;
using Stump.Tools.CacheManager.Maps;

namespace Stump.Tools.CacheManager
{
    internal class Program
    {
        public static DatabaseAccessor DBAccessor;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static string AuthConfigPath = "../../../Run/Debug/AuthServer/auth_config.xml";
        public static string WorldConfigPath = "../../../Run/Debug/WorldServer/world_config.xml";

        /// <summary>
        /// Store only the text of given languages separated by a comma. Or leave blank to store all texts
        /// </summary>
        public static string SpecificLanguage = "fr,en";

        private static void Main(string[] args)
        {
            NLogHelper.DefineLogProfile(false, true);

            foreach (var s in args)
            {
                Console.WriteLine(s);
            }

            logger.Info("Parameters : [auth config path] [world config path] [dofus dir path] [languages]");

            AuthConfigPath = args.Length > 0 ? args[0] : AuthConfigPath;
            WorldConfigPath = args.Length > 1 ? args[1] : WorldConfigPath;

            string dofusPath = args.Length > 2 ? args[2] : FindDofusPath();
            string d2OFolder = Path.Combine(dofusPath, "data", "common");
            string d2IFolder = Path.Combine(dofusPath, "data", "i18n");
            string mapsFolder = Path.Combine(dofusPath, "content", "maps");

            SpecificLanguage = args.Length > 3 ? args[3] : SpecificLanguage;

            XmlConfig config;
            if (!string.IsNullOrEmpty(Path.GetFullPath(WorldConfigPath)))
            {
                logger.Info("Opening World Config");
                config =
                    new XmlConfig(WorldConfigPath) {IgnoreUnloadedAssemblies = true};
                config.AddAssembly(typeof (WorldServer).Assembly);
                config.Load();
            }

            logger.Info("Opening World Database");
            DBAccessor = new DatabaseAccessor(WorldServer.DatabaseConfiguration);
            DBAccessor.RegisterMappingAssembly(Assembly.GetAssembly(typeof(WorldServer)));
            DBAccessor.Initialize();
            DBAccessor.OpenConnection();

            logger.Info("Building World Database");
            // build maps
            var mapBuilder = new MapLoader(DBAccessor);
            mapBuilder.LoadMaps(mapsFolder);

            var dbBuilder = new DatabaseBuilder(DBAccessor, typeof(WorldServer).Assembly, d2OFolder, d2IFolder, "world_patchs");
            dbBuilder.Build();

            DBAccessor.CloseConnection();

            logger.Info("All tasks done.");
            Console.Read();
        }

        private static string FindDofusPath()
        {
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
                Exit("Dofus data path not found");

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