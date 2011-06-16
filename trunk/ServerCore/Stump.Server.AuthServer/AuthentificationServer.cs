
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Stump.Core.Attributes;
using Stump.Database.AuthServer;
using Stump.Database.Types;
using Stump.DofusProtocol;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Commands;
using Stump.Server.AuthServer.IPC;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;

namespace Stump.Server.AuthServer
{
    public class AuthentificationServer : ServerBase<AuthentificationServer>
    {

        [Variable]
        public static DatabaseConfiguration AuthDatabaseConfiguration = new DatabaseConfiguration
        {
            DatabaseType = Castle.ActiveRecord.Framework.Config.DatabaseType.MySql,
            Host = "localhost",
            Name = "stump_auth",
            User = "root",
            Password = "",
            UpdateFileDir = "./sql_update/auth/",
        };

        public DatabaseAccessor AuthDbAccessor
        {
            get;
            private set;
        }

        public DatabaseAccessor DataDbAccessor
        {
            get;
            private set;
        }


        public AuthentificationServer() :
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

                logger.Info("Initializing Auth Database...");
                AuthDbAccessor = new DatabaseAccessor(AuthDatabaseConfiguration, Definitions.DatabaseRevision, typeof(AuthBaseRecord<>));

                logger.Info("Start Database Engine...");
                DatabaseAccessor.StartEngine();

                logger.Info("Opening Auth Database...");
                AuthDbAccessor.OpenDatabase();

                logger.Info("Initializing WorldServers Manager");
                WorldServerManager.Initialize();

                logger.Info("Register Messages...");
                MessageReceiver.Initialize();
                ProtocolTypeManager.Initialize();

                logger.Info("Register Packets Handlers...");
                HandlerManager.RegisterAll(typeof(AuthentificationServer).Assembly);

                logger.Info("Register Commands...");
                CommandManager.RegisterAll<AuthCommand, AuthSubCommand>(typeof(AuthCommand).Assembly);
            }
            catch (Exception ex)
            {
                logger.Fatal("Cannot initialize Server : " + ex);
                Shutdown();
            }
        }

        protected override void OnPluginAdded(PluginContext plugincontext)
        {
            CommandManager.RegisterAll<AuthCommand, AuthSubCommand>(plugincontext.PluginAssembly);

            base.OnPluginAdded(plugincontext);
        }

        public override void Start()
        {
            base.Start();

            logger.Info("Starting Console Handler Interface...");
            ConsoleInterface.Start();

            logger.Info("Start World Servers Manager");
            WorldServerManager.Start();

            logger.Info("Starting IPC Server..");
            IpcServer.Instance.Initialize();
        }

        public override void OnShutdown()
        {
        }

        public override BaseClient CreateClient(Socket s)
        {
            return new AuthClient(s);
        }

        public bool DisconnectClientsUsingAccount(AccountRecord account)
        {
            var clients = GetClientsUsingAccount(account).ToArray();

            foreach (var t in clients)
            {
                t.Disconnect();
            }

            if (IpcServer.Instance.GetIpcClients().Any(ipcclient => ipcclient.DisconnectConnectedAccount(account)))
            {
                return true;
            }

            return clients.Count() > 0;
        }

        public IEnumerable<AuthClient> GetClientsUsingAccount(AccountRecord account)
        {
            return (from AuthClient entry in MessageListener.ClientList
                    where entry.Account != null && entry.Account.Id == account.Id
                    select entry);
        }

        public IEnumerable<AuthClient> GetClientsLookingOfServers()
        {
            return (from AuthClient entry in MessageListener.ClientList
                    where entry.LookingOfServers
                    select entry);
        }
    }
}