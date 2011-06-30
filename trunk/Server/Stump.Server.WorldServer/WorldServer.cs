
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using Castle.ActiveRecord.Framework.Config;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.IO;
using Stump.Server.WorldServer.IPC;
using Stump.Server.WorldServer.Network;

namespace Stump.Server.WorldServer
{
    public class WorldServer : ServerBase<WorldServer>
    {
        [Variable]
        public static WorldServerData ServerInformation = new WorldServerData
        {
            Id = 1,
            Name = "Jiva",
            Address = "localhost",
            Port = 3467,
        };

        [Variable]
        public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            DatabaseType = DatabaseType.MySql,
            Host = "localhost",
            Name = "stump_world",
            User = "root",
            Password = "",
            UpdateFileDir = "./sql_update/world/",
        };

        public WorldServer()
            : base(Definitions.ConfigFilePath, Definitions.SchemaFilePath)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            ConsoleInterface = new WorldConsole();
            ConsoleBase.SetTitle("#Stump World Server : " + ServerInformation.Name);

            logger.Info("Initializing Database...");
            DatabaseAccessor = new DatabaseAccessor(DatabaseConfiguration, Definitions.DatabaseRevision, typeof(WorldBaseRecord<>), Assembly.GetExecutingAssembly());
            DatabaseAccessor.Initialize();

            logger.Info("Opening Database...");
            DatabaseAccessor.OpenDatabase();

            logger.Info("Register Messages...");
            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();

            logger.Info("Register Packet Handlers...");
            HandlerManager.RegisterAll(Assembly.GetExecutingAssembly());

            logger.Info("Register Commands...");
            CommandManager.RegisterAll(Assembly.GetExecutingAssembly());

            logger.Info("Initializing IPC Client..");
            IpcAccessor.Instance.Initialize();
        }

        protected override void OnPluginAdded(PluginContext plugincontext)
        {
            CommandManager.RegisterAll(plugincontext.PluginAssembly);

            base.OnPluginAdded(plugincontext);
        }

        public override void Start()
        {
            logger.Info("Starting Console Handler Interface...");
            ConsoleInterface.Start();

            logger.Info("Starting IPC Communication on " + IpcAccessor.IpcAuthPort + "/" +
                        IpcAccessor.IpcWorldPort + "...");
            IpcAccessor.Instance.Start();

            base.Start();
        }

        protected override BaseClient CreateClient(Socket s)
        {
            return new WorldClient(s);
        }

        public override void OnShutdown()
        {
        }


        public IEnumerable<WorldClient> FindClients(Predicate<WorldClient> predicate)
        {
            return ( from WorldClient entry in MessageListener.ClientList
                     where predicate(entry)
                     select entry );
        }
    }
}