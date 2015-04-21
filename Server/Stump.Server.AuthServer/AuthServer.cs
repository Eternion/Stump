
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.ORM;
using Stump.Server.AuthServer.IO;
using Stump.Server.AuthServer.IPC;
using Stump.Server.AuthServer.Managers;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using DatabaseConfiguration = Stump.ORM.DatabaseConfiguration;

namespace Stump.Server.AuthServer
{
    public class AuthServer : ServerBase<AuthServer>
    {
        [Variable]
        public static readonly bool HostAutoDefined = true;

        /// <summary>
        /// Current server address. Used if HostAutoDefined = false
        /// </summary>
        [Variable]
        public static readonly string CustomHost = "127.0.0.1";

        /// <summary>
        /// Server port
        /// </summary>
        [Variable]
        public static readonly int Port = 443;

        [Variable]
        public static string IpcAddress = "localhost";

        [Variable]
        public static int IpcPort = 9100;


        [Variable]
        public static string ConnectionSwfPatch = "./swf_patchs/StumpPatch.swf";

        private string m_host;

        [Variable(Priority = 10)]
        public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            ProviderName = "MySql.Data.MySqlClient",
            Host = "localhost",
            Port = "3306",
            DbName = "stump_auth",
            User = "root",
            Password = "",
        };

        private byte[] m_patchBuffer;
        public bool m_maintenanceMode;

        public IPCHost IpcHost
        {
            get;
            private set;
        }

        public AuthPacketHandler HandlerManager
        {
            get;
            private set;
        }

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
                DBAccessor = new DatabaseAccessor(DatabaseConfiguration);
                DBAccessor.RegisterMappingAssembly(Assembly.GetExecutingAssembly());
                InitializationManager.Initialize(InitializationPass.Database);
                DBAccessor.Initialize();

                logger.Info("Opening Database...");
                DBAccessor.OpenConnection();
                DataManager.DefaultDatabase = DBAccessor.Database;
                DataManagerAllocator.Assembly = Assembly.GetExecutingAssembly();

                logger.Info("Register Messages...");
                MessageReceiver.Initialize();
                ProtocolTypeManager.Initialize();

                logger.Info("Register Packets Handlers...");
                HandlerManager = AuthPacketHandler.Instance;
                HandlerManager.RegisterAll(Assembly.GetExecutingAssembly());

                logger.Info("Register Commands...");
                CommandManager.RegisterAll(Assembly.GetExecutingAssembly());

                logger.Info("Start World Servers Manager");
                WorldServerManager.Instance.Initialize();

                logger.Info("Initialize Account Manager");
                AccountManager.Instance.Initialize();

                logger.Info("Initialize IPC Server..");
                IpcHost = new IPCHost(IpcAddress, IpcPort);

                InitializationManager.InitializeAll();
                IsInitialized = true;

                if (Environment.GetCommandLineArgs().Contains("-maintenance"))
                    m_maintenanceMode = true;
            }
            catch (Exception ex)
            {
                HandleCrashException(ex);
                Shutdown();
            }
        }

        public override void Start()
        {
            base.Start();

            logger.Info("Start Ipc Server");
            IpcHost.Start();

            logger.Info("Starting Console Handler Interface...");
            ConsoleInterface.Start();

            logger.Info("Start listening on port : " + Port + "...");
            m_host = HostAutoDefined ? IPAddress.Loopback.ToString() : CustomHost;
            ClientManager.Start(m_host, Port);

            StartTime = DateTime.Now;
        }

        public byte[] GetConnectionSwfPatch()
        {
            if (m_patchBuffer != null)
                return m_patchBuffer;

            if (File.Exists(ConnectionSwfPatch))
                return m_patchBuffer = File.ReadAllBytes(ConnectionSwfPatch);

            logger.Warn("SWF Patch for connection not found ({0}", ConnectionSwfPatch);
            return null;
        }


        protected override void OnShutdown()
        {
            DBAccessor.CloseConnection();
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