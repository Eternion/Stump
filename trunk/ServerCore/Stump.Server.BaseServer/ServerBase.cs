
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
using Stump.Core.Pool.Task;
using Stump.Core.Threading;
using Stump.Core.Xml;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;

namespace Stump.Server.BaseServer
{
    public abstract class ServerBase<T> where T : class
    {
        /// <summary>
        ///   Class singleton
        /// </summary>
        public static T Instance;

        protected Dictionary<string, Assembly> LoadedAssemblies;
        protected Logger logger;

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

        public XmlConfigReader ConfigReader
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
        ///   Classe de base de l'execution des commandes.
        /// </summary>
        public CommandsManager CommandManager
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
        ///   Classe de gestion du traitement séquencielle et prioritisé des paquetqs
        /// </summary>
        public QueueDispatcher QueueDispatcher
        {
            get;
            protected set;
        }

        /// <summary>
        ///   Classe de Gestion MultiThreading des paquets
        /// </summary>
        public WorkerManager WorkerManager
        {
            get;
            protected set;
        }

        protected MessageListener MessageListener
        {
            get;
            set;
        }

        public TaskPool TaskPool
        {
            get;
            private set;
        }

        public bool Running
        {
            get;
            set;
        }

        public virtual void Initialize()
        {
            Instance = this as T;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

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
            ConfigReader = new XmlConfigReader(ConfigFilePath, SchemaFilePath);
            ConfigReader.DefinesVariables(ref LoadedAssemblies);

            /* Set Config Watcher */
            FileWatcher.RegisterFileModification(ConfigFilePath, () =>
                {
                    if (ConsoleInterface.AskForSomething("Config has been modified, do you want to reload it ?", 20))
                    {
                        ConfigReader = new XmlConfigReader(ConfigFilePath, SchemaFilePath);
                        ConfigReader.DefinesVariables(ref LoadedAssemblies);
                        logger.Warn("Config has been reloaded sucessfully");
                    }
                });

            logger.Info("Initialize Task Pool");
            TaskPool = new TaskPool();
            TaskPool.Initialize(Assembly.GetCallingAssembly());

            logger.Info("Initializing Network Interfaces...");
            QueueDispatcher = new QueueDispatcher(Settings.EnableBenchmarking);
            HandlerManager = new HandlerManager();
            WorkerManager = new WorkerManager(QueueDispatcher, HandlerManager);

            CommandManager = new CommandsManager();

            MessageListener = new MessageListener(QueueDispatcher, CreateClient);
            MessageListener.Initialize();

            if (Settings.InactivityDisconnectionTime.HasValue)
                TaskPool.RegisterCyclicTask(DisconnectAfkClient, Settings.InactivityDisconnectionTime.Value / 4, null, null);

            MessageListener.ClientConnected += OnClientConnected;
            MessageListener.ClientDisconnected += OnClientDisconnected;

            PluginManager.PluginAdded += OnPluginAdded;
            PluginManager.PluginRemoved += OnPluginRemoved;
        }

        protected virtual void OnPluginRemoved(PluginContext plugincontext)
        {
            logger.Info("Plugins Unloaded : {0}", plugincontext.Plugin.GetDefaultDescription());
        }

        protected virtual void OnPluginAdded(PluginContext plugincontext)
        {
            logger.Info("Plugins Loaded : {0}", plugincontext.Plugin.GetDefaultDescription());
        }

        private void OnClientConnected(MessageListener messageListener, BaseClient client)
        {
            logger.Info("Client {0} connected", client);
        }

        private void OnClientDisconnected(MessageListener messageListener, BaseClient client)
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
            logger.Error("Unobserved Exception : " + e);

            e.SetObserved();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            if (args.IsTerminating)
                logger.Fatal("Application has crashed. An Unhandled Exception has been thrown :");

            logger.Error("Unhandled Exception : " + ((Exception)args.ExceptionObject).Message);
            logger.Error("Source : {0} Method : {1}", ((Exception)args.ExceptionObject).Source,
                         ((Exception)args.ExceptionObject).TargetSite);
            logger.Error("Stack Trace : " + ((Exception)args.ExceptionObject).StackTrace);

            if (args.IsTerminating)
                Shutdown();
        }

        public void HandleCrashException(Exception e)
        {
            logger.Fatal("An exception occurred ! {0} : {1}", e.GetType().Name, e.Message);
            logger.Fatal("Source : {0} Method : {1}", e.Source, e.TargetSite);
            logger.Fatal("Stack Trace : " + e.StackTrace);
        }

        public virtual void Start()
        {
            logger.Info("Loading Plugins...");
            PluginManager.LoadAllPlugins();

            logger.Info("Start listening on port : " + MessageListener.Port + "...");
            MessageListener.Start();

            Running = true;
        }

        public virtual void Update()
        {
            TaskPool.ProcessUpdate();
        }

        private void DisconnectAfkClient()
        {
            logger.Info("Disconnect AFK Clients");
            var afkClients = MessageListener.ClientList.Where(c => DateTime.Now.Subtract(c.LastActivity).TotalSeconds >= Settings.InactivityDisconnectionTime);
            foreach (BaseClient client in afkClients)
                client.Disconnect();
        }

        public abstract BaseClient CreateClient(Socket s);

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
}