
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using Castle.ActiveRecord.Framework.Config;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.IO;
using Stump.Server.AuthServer.IPC;
using Stump.Server.AuthServer.Managers;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;

namespace Stump.Server.AuthServer
{
    public class AuthServer : ServerBase<AuthServer>
    {
        /// <summary>
        /// Current server adress
        /// </summary>
        [Variable]
        public static string Host = "127.0.0.1";

        /// <summary>
        /// Server port
        /// </summary>
        [Variable]
        public static int Port = 443;

        [Variable]
        public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            DatabaseType = DatabaseType.MySql,
            Host = "localhost",
            Name = "stump_auth",
            User = "root",
            Password = "",
            UpdateFileDir = "./sql_update/auth/",
        };

        public AuthServer() :
            base(Definitions.ConfigFilePath, Definitions.SchemaFilePath)
        {
        }

        public override void Initialize()
        {
            try
            {
                base.Initialize();
                ConsoleInterface = new AuthConsole();
                ConsoleBase.SetTitle("#Stump Authentification Server");

                logger.Info("Initializing Database...");
                DatabaseAccessor = new DatabaseAccessor(DatabaseConfiguration, Definitions.DatabaseRevision, typeof(AuthBaseRecord<>), Assembly.GetExecutingAssembly());
                DatabaseAccessor.Initialize();

                logger.Info("Opening Database...");
                DatabaseAccessor.OpenDatabase();

                logger.Info("Register Messages...");
                MessageReceiver.Initialize();
                ProtocolTypeManager.Initialize();

                logger.Info("Register Packets Handlers...");
                HandlerManager.RegisterAll(Assembly.GetExecutingAssembly());

                logger.Info("Register Commands...");
                CommandManager.RegisterAll(Assembly.GetExecutingAssembly());

                logger.Info("Start World Servers Manager");
                WorldServerManager.Instance.Initialize();
                WorldServerManager.Instance.Start();

                logger.Info("Initialize IPC Server..");
                IpcServer.Instance.Initialize();

                InitializationManager.InitializeAll();
            }
            catch (Exception ex)
            {
                HandleCrashException(ex);
                Shutdown();
            }
        }

        protected override void OnPluginAdded(PluginContext plugincontext)
        {
            CommandManager.RegisterAll(plugincontext.PluginAssembly);

            base.OnPluginAdded(plugincontext);
        }

        public override void Start()
        {
            base.Start();

            logger.Info("Starting Console Handler Interface...");
            ConsoleInterface.Start();

            logger.Info("Start listening on port : " + Port + "...");
            ClientManager.Start(Host, Port);

            StartTime = DateTime.Now;
        }

        public override void OnShutdown()
        {
        }

        protected override BaseClient CreateClient(Socket s)
        {
            return new AuthClient(s);
        }

        public IEnumerable<AuthClient> FindClients(Predicate<AuthClient> predicate)
        {
            return ClientManager.FindAll(predicate);
        }
    }
}