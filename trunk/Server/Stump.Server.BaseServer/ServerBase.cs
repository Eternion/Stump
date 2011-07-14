using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Core.IO;
using Stump.Core.IO.Watchers;
using Stump.Core.Pool.Task;
using Stump.Core.Threading;
using Stump.Core.Xml;
using Stump.Core.Xml.Config;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;

namespace Stump.Server.BaseServer
{
    // this methods should be accessible by the BaseServer assembly
    public abstract class ServerBase
    {
        internal static ServerBase InstanceAsBase;

        protected Dictionary<string, Assembly> LoadedAssemblies;
        protected Logger logger;
        private bool m_ignoreReload;

        protected ServerBase(string configFile, string schemaFile)
        {
            ConfigFilePath = configFile;
            SchemaFilePath = schemaFile;
        }

        public string ConfigFilePath
        {
            get;
            protected set;
        }

        public string SchemaFilePath
        {
            get;
            protected set;
        }

        public XmlConfig Config
        {
            get;
            protected set;
        }

        public DatabaseAccessor DatabaseAccessor
        {
            get;
            protected set;
        }

        public ConsoleBase ConsoleInterface
        {
            get;
            protected set;
        }

        /// <summary>
        ///   Manage commands
        /// </summary>
        public CommandManager CommandManager
        {
            get;
            protected set;
        }

        public HandlerManager HandlerManager
        {
            get;
            protected set;
        }

        /// <summary>
        ///   Manage tasks, that handle packets
        /// </summary>
        public WorkerManager WorkerManager
        {
            get;
            protected set;
        }

        public ClientManager ClientManager
        {
            get;
            protected set;
        }

        public TaskPool IOTaskPool
        {
            get;
            protected set;
        }

        public InitializationManager InitializationManager
        {
            get;
            protected set;
        }

        public PluginManager PluginManager
        {
            get;
            protected set;
        }

        public bool Running
        {
            get;
            protected set;
        }

        public virtual void Initialize()
        {
            InstanceAsBase = this;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            PreLoadReferences(Assembly.GetCallingAssembly());
            LoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;

            ConsoleBase.DrawAsciiLogo();
            Console.WriteLine();

            /* Initialize Logger */
            NLogHelper.DefineLogProfile(true, true);
            NLogHelper.EnableLogging();
            logger = LogManager.GetCurrentClassLogger();

            InitializeGarbageCollector();

            logger.Info("Initializing Configuration...");
            /* Initialize Config File */
            Config = new XmlConfig(ConfigFilePath, SchemaFilePath);
            Config.AddAssemblies(LoadedAssemblies.Values.ToArray());
            Config.Load();

            /* Set Config Watcher */
            FileWatcherManager.Watch(ConfigFilePath, WatcherType.Modification,
                 () =>
                 {
                     if (!m_ignoreReload && ConsoleInterface.AskAndWait("Config has been modified, do you want to reload it ?", 20))
                     {
                         Config.Reload();
                         logger.Warn("Config has been reloaded sucessfully");
                     }

                     m_ignoreReload = false;
                 });

            logger.Info("Initialize Task Pool");
            IOTaskPool = new TaskPool();
            IOTaskPool.Initialize(Assembly.GetCallingAssembly());

            CommandManager = CommandManager.Instance;
            CommandManager.RegisterAll(Assembly.GetExecutingAssembly());

            logger.Info("Initializing Network Interfaces...");
            HandlerManager = HandlerManager.Instance;
            ClientManager = ClientManager.Instance;
            ClientManager.Initialize(CreateClient);
            WorkerManager = WorkerManager.Instance;
            WorkerManager.Initialize();


            if (Settings.InactivityDisconnectionTime.HasValue)
                IOTaskPool.RegisterCyclicTask(DisconnectAfkClient, Settings.InactivityDisconnectionTime.Value / 4, null, null);

            ClientManager.ClientConnected += OnClientConnected;
            ClientManager.ClientDisconnected += OnClientDisconnected;

            PluginManager = PluginManager.Instance;
            PluginManager.PluginAdded += OnPluginAdded;
            PluginManager.PluginRemoved += OnPluginRemoved;

            InitializationManager = InitializationManager.Instance;
            InitializationManager.AddAssemblies(AppDomain.CurrentDomain.GetAssemblies());
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

        protected virtual void OnPluginRemoved(PluginContext plugincontext)
        {
            logger.Info("Plugins Unloaded : {0}", plugincontext.Plugin.GetDefaultDescription());
        }

        protected virtual void OnPluginAdded(PluginContext plugincontext)
        {
            logger.Info("Plugins Loaded : {0}", plugincontext.Plugin.GetDefaultDescription());
        }

        private void OnClientConnected(BaseClient client)
        {
            logger.Info("Client {0} connected", client);
        }

        private void OnClientDisconnected(BaseClient client)
        {
            logger.Info("Client {0} disconnected", client);
        }

        private static void InitializeGarbageCollector()
        {
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;
        }

        private void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            LoadedAssemblies.Add(args.LoadedAssembly.GetName().Name, args.LoadedAssembly);
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleCrashException(e.Exception);

            e.SetObserved();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            HandleCrashException((Exception) args.ExceptionObject);

            if (args.IsTerminating)
                Shutdown();
        }

        public void HandleCrashException(Exception e)
        {
            logger.Fatal(
                string.Format(" Crash Exception : {0}\r\n", e.Message) +
                string.Format(" Source: {0} -> {1}\r\n", e.Source,
                        e.TargetSite) +
                string.Format(" Stack Trace:\r\n{0}", e.StackTrace));

            if (e.InnerException != null)
                HandleCrashException(e.InnerException);
        }

        public virtual void Start()
        {
            logger.Info("Loading Plugins...");
            PluginManager.Instance.LoadAllPlugins();

            Running = true;
        }

        public virtual void Update()
        {
            IOTaskPool.ProcessUpdate();
        }

        /// <summary>
        /// Allow the server to ignore the next modification of the config file.
        /// Use it when you save the config
        /// </summary>
        public void IgnoreNextConfigReload()
        {
            m_ignoreReload = true;
        }

        private void DisconnectAfkClient()
        {
            // todo : this is not an afk check but a timeout check
            logger.Info("Disconnect AFK Clients");

            IEnumerable<BaseClient> afkClients = ClientManager.FindAll(client =>
                DateTime.Now.Subtract(client.LastActivity).TotalSeconds >= Settings.InactivityDisconnectionTime);

            foreach (BaseClient client in afkClients)
                client.Disconnect();
        }

        protected abstract BaseClient CreateClient(Socket s);

        public abstract void OnShutdown();

        public void Shutdown()
        {
            lock (this)
            {
                if (Running)
                    Running = false;

                OnShutdown();

                //StopTcp();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                // We are done at this point.
                Console.WriteLine("Application is now terminated. Wait " + Definitions.ExitWaitTime +
                                  " seconds to exit ... or press any key to cancel");

                if (ConditionWaiter.WaitFor(() => Console.KeyAvailable, Definitions.ExitWaitTime * 1000, 20))
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

    public abstract class ServerBase<T> : ServerBase
        where T : class
    {
        /// <summary>
        ///   Class singleton
        /// </summary>
        public static T Instance;


        protected ServerBase(string configFile, string schemaFile)
            : base(configFile, schemaFile)
        {
        }

        public override void Initialize()
        {
            Instance = this as T;
            base.Initialize();
        }
    }
}