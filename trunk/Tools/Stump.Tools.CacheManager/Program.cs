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
using Definitions = Stump.Server.AuthServer.Definitions;

namespace Stump.Tools.CacheManager
{
    internal class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static string AuthConfigPath = "../../../../Run/Debug/AuthServer/auth_config.xml";
        public static string WorldConfigPath = "../../../../Run/Debug/WorldServer/world_config.xml";

        private static void Main(string[] args)
        {
            string dofusDataPath = args.Length == 0 ? FindDofusDataPath() : args[0];
            string d2OFolder = Path.Combine(dofusDataPath, "common");
            string d2IFolder = Path.Combine(dofusDataPath, "i18n");

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
            var dbAccessor = new DatabaseAccessor(AuthServer.DatabaseConfiguration,
                                                  Definitions.DatabaseRevision,
                                                  typeof (AuthBaseRecord<>),
                                                  typeof (AuthServer).Assembly);
            dbAccessor.Initialize();
            dbAccessor.OpenDatabase();

            logger.Info("Building Auth Database...");
            var dbBuilder = new DatabaseBuilder(typeof (AuthServer).Assembly, d2OFolder, d2IFolder);
            dbBuilder.Build();

            dbAccessor.CloseDatabase();

            if (!string.IsNullOrEmpty(WorldConfigPath))
            {
                logger.Info("Opening World Config");
                config =
                    new XmlConfig(WorldConfigPath) {IgnoreUnloadedAssemblies = true};
                config.AddAssembly(typeof (WorldServer).Assembly);
                config.Load();
            }

            logger.Info("Opening World Database");
            dbAccessor = new DatabaseAccessor(WorldServer.DatabaseConfiguration,
                                              Server.WorldServer.Definitions.DatabaseRevision,
                                              typeof (WorldBaseRecord<>),
                                              typeof (WorldServer).Assembly);
            dbAccessor.Initialize();
            dbAccessor.OpenDatabase();

            logger.Info("Building World Database");
            dbBuilder = new DatabaseBuilder(typeof (WorldServer).Assembly, d2OFolder, d2IFolder);
            dbBuilder.Build();

            logger.Info("All tasks done.");
            Console.Read();
        }

        private static string FindDofusDataPath()
        {
            string programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");

            if (string.IsNullOrEmpty(programFiles))
                programFiles = Environment.GetEnvironmentVariable("programfiles");

            string dofusDataPath;

            if (string.IsNullOrEmpty(programFiles))
                dofusDataPath =  Path.Combine(AskDofusPath(), "Dofus 2", "app", "data");

            dofusDataPath = Path.Combine(programFiles, "Dofus 2", "app", "data");

            if (Directory.Exists(dofusDataPath))
                return dofusDataPath;

            dofusDataPath = Path.Combine(AskDofusPath(), "Dofus 2", "app", "data");

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