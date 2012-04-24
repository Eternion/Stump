
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Castle.ActiveRecord.Framework.Config;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Pool.Task;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.Core.Xml.Config;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Handlers.Auth;
using Stump.Tools.Proxy.Handlers.World;
using Stump.Tools.Proxy.Network;
using Definitions = Stump.Server.BaseServer.Definitions;

namespace Stump.Tools.Proxy
{
    public class Proxy : Singleton<Proxy>
    {
        private static Logger logger;

        [Variable(Priority = 20)]
        public static string AuthAddress = "127.0.0.1";

        [Variable]
        public static int AuthPort = 5555;

        [Variable]
        public static string WorldAddress = "127.0.0.1";

        [Variable(Priority = 21)]
        public static int WorldPort = 5556;

        [Variable]
        public static string RealServerAddress = "213.248.126.180";

        [Variable]
        public static int RealServerPort = 5555;

        [Variable]
        public static int IOTaskPoolInterval = 90;

        [Variable(Priority = 10)]
        public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            DatabaseType = DatabaseType.MySql,
            Host = "localhost",
            Name = "stump_world",
            User = "root",
            Password = "",
            UpdateFileDir = "./sql_update/",
        };


        private Dictionary<string, Assembly> m_loadedAssemblies;

        public XmlConfig Config
        {
            get;
            private set;
        }

        public ClientManager AuthClientManager
        {
            get;
            private set;
        }

        public ClientManager WorldClientManager
        {
            get;
            private set;
        }

        public DatabaseAccessor DatabaseAccessor
        {
            get;
            private set;
        }

        public SelfRunningTaskQueue IOTaskPool
        {
            get;
            private set;
        }

        public bool Running
        {
            get;
            set;
        }

        public AuthPacketHandler AuthHandler
        {
            get;
            private set;
        }

        public WorldPacketHandler WorldHandler
        {
            get;
            private set;
        }

        public void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += (OnUnhandledException);

            NLogHelper.DefineLogProfile(true, true);
            NLogHelper.EnableLogging();
            logger = LogManager.GetCurrentClassLogger();

            logger.Info("Load all assemblies...");

            PreLoadReferences(Assembly.GetExecutingAssembly());
            m_loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;

            logger.Info("Initializing Configuration...");
            Config = new XmlConfig("proxy_config.xml");
            Config.AddAssemblies(typeof(Proxy).Assembly);

            if (!File.Exists("proxy_config.xml"))
                Config.Create();
            else
                Config.Load();

            logger.Info("Initializing Network Interfaces...");
            AuthHandler = AuthPacketHandler.Instance;
            AuthHandler.RegisterAll(Assembly.GetExecutingAssembly());
            WorldHandler = WorldPacketHandler.Instance;
            WorldHandler.RegisterAll(Assembly.GetExecutingAssembly());
            IOTaskPool = new SelfRunningTaskQueue(IOTaskPoolInterval, "IO Task Pool");

            logger.Info("Initializing Network Messages...");
            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();
            AuthClientManager = new ClientManager();
            AuthClientManager.Initialize(CreateClientAuth);
            WorldClientManager = new ClientManager();
            WorldClientManager.Initialize(CreateClientWorld);

            logger.Info("Loading Database...");
            DatabaseAccessor = new DatabaseAccessor(
                DatabaseConfiguration,
                Server.WorldServer.Definitions.DatabaseRevision,
                typeof(WorldBaseRecord<>),
                typeof(WorldBaseRecord<>).Assembly, false);
            DatabaseAccessor.Initialize();

            logger.Info("Open Database...");
            DatabaseAccessor.OpenDatabase();

            logger.Info("Loading some others stuff...");
            TextManager.Instance.Initialize();
            EffectManager.Instance.Initialize();
            ItemManager.Instance.Initialize();
            InteractiveManager.Instance.Initialize();
            NpcManager.Instance.Initialize();
            MonsterManager.Instance.Initialize();
            CellTriggerManager.Instance.Initialize();
            World.Instance.LoadSpaces();

            Start();
        }

        
        /// <summary>
        /// Load before the runtime all referenced assemblies
        /// </summary>
        private static void PreLoadReferences(Assembly executingAssembly)
        {
            foreach (var assemblyName in executingAssembly.GetReferencedAssemblies())
            {
                if (AppDomain.CurrentDomain.GetAssemblies().Count(entry => entry.GetName().FullName == assemblyName.FullName) <= 0)
                {
                    var loadedAssembly = Assembly.Load(assemblyName);

                    PreLoadReferences(loadedAssembly);
                }
            }

        }

        public void Start()
        {
            logger.Info("Start listening on port : " + AuthPort + "...");
            AuthClientManager.Start(AuthAddress, AuthPort);

            logger.Info("Start listening on port : " + WorldPort + "...");
            WorldClientManager.Start(WorldAddress, WorldPort);

            WorldClientManager.ClientConnected += client => logger.Info("Client connected : " + client.ToString());
            WorldClientManager.ClientDisconnected += client => logger.Info("Client disconnected : " + client.ToString());

            Running = true;
        }

        public void Update()
        {
        }

        public BaseClient CreateClientAuth(Socket s)
        {
            return new AuthClient(s, new IPEndPoint(IPAddress.Parse(RealServerAddress), RealServerPort));
        }

        public BaseClient CreateClientWorld(Socket s)
        {
            return new WorldClient(s);
        }

        private void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            m_loadedAssemblies.Add(args.LoadedAssembly.GetName().Name, args.LoadedAssembly);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            if (args.IsTerminating)
                logger.Fatal("Application has crashed. An Unhandled Exception has been thrown :");

            logger.Error("Unhandled Exception : " + ((Exception) args.ExceptionObject).Message);
            logger.Error("Source : {0} Method : {1}", ((Exception) args.ExceptionObject).Source,
                         ((Exception) args.ExceptionObject).TargetSite);
            logger.Error("Stack Trace : " + ((Exception) args.ExceptionObject).StackTrace);

            if (args.IsTerminating)
                Shutdown();
        }

        public void Shutdown()
        {
            lock (this)
            {
                if (Running)
                    Running = false;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                // We are done at this point.
                Console.WriteLine("Application is now terminated. Wait " + Definitions.ExitWaitTime +
                                  " seconds to exit ... or press any key to cancel");

                if (ConditionWaiter.WaitFor(() => Console.KeyAvailable, Definitions.ExitWaitTime*1000, 20))
                {
                    Console.ReadKey(false);

                    Console.WriteLine("Press now a key to exit...");
                    Thread.Sleep(100);

                    Console.ReadKey(false);
                }

                Environment.Exit(0);
            }
        }
    }
}