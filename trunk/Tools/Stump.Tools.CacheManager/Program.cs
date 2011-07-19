using System;
using System.IO;
using NLog;
using Stump.Core.IO;
using Stump.Core.Xml.Config;
using Stump.Server.AuthServer;
using Stump.Server.AuthServer.Database;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database;
using Stump.Tools.CacheManager.SQL;
using Definitions = Stump.Server.AuthServer.Definitions;

namespace Stump.Tools.CacheManager
{
    internal class Program
    {
        public static MySqlAccessor DBAccessor;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static string AuthConfigPath = "../../../../GUI/Stump.GUI.AuthConsole/auth_config.xml";
        public static string WorldConfigPath = "../../../../GUI/Stump.GUI.WorldConsole/world_config.xml";

        private static void Main(string[] args)
        {
            string dofusPath = args.Length == 0 ? FindDofusPath() : args[0];
            string d2OFolder = Path.Combine(dofusPath, "data", "common");
            string d2IFolder = Path.Combine(dofusPath, "data", "i18n");
            string mapsFolder = Path.Combine(dofusPath, "content", "maps");

            NLogHelper.DefineLogProfile(false, true);

            XmlConfig config;
            if (!string.IsNullOrEmpty(AuthConfigPath))
            {
                logger.Info("Opening Auth Config");
                config = new XmlConfig(AuthConfigPath) {IgnoreUnloadedAssemblies = true};
                config.AddAssembly(typeof (AuthServer).Assembly);
                config.Load();
            }

            logger.Info("Opening Auth Database");
            DBAccessor = new MySqlAccessor(AuthServer.DatabaseConfiguration);
            DBAccessor.Open();

            logger.Info("Building Auth Database...");
            var dbBuilder = new DatabaseBuilder(typeof (AuthServer).Assembly, d2OFolder, d2IFolder);
            dbBuilder.Build();

            DBAccessor.Close();

            if (!string.IsNullOrEmpty(WorldConfigPath))
            {
                logger.Info("Opening World Config");
                config =
                    new XmlConfig(WorldConfigPath) {IgnoreUnloadedAssemblies = true};
                config.AddAssembly(typeof (WorldServer).Assembly);
                config.Load();
            }

            logger.Info("Opening World Database");
            DBAccessor = new MySqlAccessor(WorldServer.DatabaseConfiguration);
            DBAccessor.Open();

            logger.Info("Building World Database");
            Maps.MapLoader.LoadMaps(mapsFolder);
            dbBuilder = new DatabaseBuilder(typeof (WorldServer).Assembly, d2OFolder, d2IFolder);
            dbBuilder.Build();

            DBAccessor.Close();

            logger.Info("All tasks done.");
            Console.Read();
        }

        private static string FindDofusPath()
        {
            string programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");

            if (string.IsNullOrEmpty(programFiles))
                programFiles = Environment.GetEnvironmentVariable("programfiles");

            string dofusDataPath;

            if (string.IsNullOrEmpty(programFiles))
                dofusDataPath =  Path.Combine(AskDofusPath(), "app");

            dofusDataPath = Path.Combine(programFiles, "Dofus 2", "app");

            if (Directory.Exists(dofusDataPath))
                return dofusDataPath;

            dofusDataPath = Path.Combine(AskDofusPath(), "app");

            if (!Directory.Exists(dofusDataPath))
                Exit("Dofus data path not found");

            return string.Empty;
        }

        private static string AskDofusPath()
        {
            logger.Warn("Dofus path not found. Enter Dofus 2 root folder (%programFiles%/Dofus 2):");

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