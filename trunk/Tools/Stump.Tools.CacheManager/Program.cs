using System;
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
        public static string D2OFolder = "./data/";

        public static string AuthConfigPath = "../../../../GUI/Stump.GUI.AuthConsole/auth_config.xml";
        public static string WorldConfigPath = "../../../../GUI/Stump.GUI.WorldConsole/world_config.xml";

        private static void Main(string[] args)
        {
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
            var dbBuilder = new DatabaseBuilder(typeof (AuthServer).Assembly, D2OFolder);
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
            dbBuilder = new DatabaseBuilder(typeof (WorldServer).Assembly, D2OFolder);
            dbBuilder.Build();

            logger.Info("All tasks done.");
            Console.Read();
        }
    }
}