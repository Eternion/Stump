using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Threading;
using Stump.Core.Timers;
using Stump.Core.Xml.Config;
using Stump.ORM;
using Stump.Server.BaseServer.Benchmark;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Exceptions;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;
using SharpRaven;
using SharpRaven.Data;

namespace Stump.Server.BaseServer
{
    // this methods should be accessible by the BaseServer assembly
    public abstract class ServerBase
    {
        internal static ServerBase InstanceAsBase;

        [Variable]
        public static int IOTaskInterval = 50;

        [Variable]
        public static bool ScheduledAutomaticShutdown = true;

        /// <summary>
        /// In minutes
        /// </summary>
        [Variable]
        public static int AutomaticShutdownTimer = 6*60;

        [Variable] public static string CommandsInfoFilePath = "./commands.xml";

        [Variable(Priority = 10, DefinableRunning = true)]
        public static bool IsExceptionLoggerEnabled = false;

        [Variable(Priority = 10)]
        public static string ExceptionLoggerDSN = "";

        public RavenClient ExceptionLogger
        {
            get;
            protected set;
        }

        protected Dictionary<string, Assembly> LoadedAssemblies;
        protected Logger logger;
        private bool m_ignoreReload;

        public event Action ServerStarted;

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

        public ConsoleBase ConsoleInterface
        {
            get;
            protected set;
        }

        public DatabaseAccessor DBAccessor
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

        public ClientManager ClientManager
        {
            get;
            protected set;
        }

        public SelfRunningTaskPool IOTaskPool
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

        public DateTime StartTime
        {
            get;
            protected set;
        }

        public TimeSpan UpTime
        {
            get
            {
                return DateTime.Now - StartTime;
            }
        }

        public bool Initializing
        {
            get;
            protected set;
        }

        public bool IsInitialized
        {
            get;
            protected set;
        }

        public bool IsShutdownScheduled
        {
            get;
            protected set;
        }

        public bool IsNoExitShutdown
        {
            get;
            set;
        }

        public DateTime ScheduledShutdownDate
        {
            get;
            protected set;
        }

        public TimedTimerEntry ScheduledShutdownTimer
        {
            get;
            protected set;
        }

        public string Version
        {
            get;
            protected set;
        }

        public virtual void Initialize()
        {
            InstanceAsBase = this;
            Initializing = true;

            Version = ((AssemblyInformationalVersionAttribute)System.Reflection.Assembly.GetExecutingAssembly()
                         .GetCustomAttributes<AssemblyInformationalVersionAttribute>().FirstOrDefault())
                         .InformationalVersion;

            /* Initialize Logger */
            NLogHelper.DefineLogProfile(true, true);
            NLogHelper.EnableLogging();
            logger = LogManager.GetCurrentClassLogger();

            if (!Debugger.IsAttached)
            {
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
                TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
                Contract.ContractFailed += OnContractFailed;
            }
            else
            {
                logger.Warn("Exceptions not handled cause Debugger is attatched");
            }

            PreLoadReferences(Assembly.GetCallingAssembly());
            LoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;

            if (Environment.GetCommandLineArgs().Contains("-config"))
            {
                UpdateConfigFiles();
            }

            ConsoleBase.DrawAsciiLogo();
            Console.WriteLine();

            InitializeGarbageCollector();

            logger.Info("Initializing Configuration...");
            /* Initialize Config File */
            Config = new XmlConfig(ConfigFilePath);
            Config.AddAssemblies(LoadedAssemblies.Values.ToArray());

            if (!File.Exists(ConfigFilePath))
            {
                Config.Create();
                logger.Info("Config file created");
            }
            else
                Config.Load();

            logger.Info("Initialize Task Pool");
            IOTaskPool = new BenchmarkedTaskPool(IOTaskInterval, "IO Task Pool");

            CommandManager = CommandManager.Instance;
            CommandManager.RegisterAll(Assembly.GetExecutingAssembly());

            logger.Info("Initializing Network Interfaces...");
            ClientManager = ClientManager.Instance;
            ClientManager.Initialize(CreateClient);

            if (Settings.InactivityDisconnectionTime.HasValue)
                IOTaskPool.CallPeriodically(Settings.InactivityDisconnectionTime.Value / 4 * 1000, DisconnectAfkClient);

            ClientManager.ClientConnected += OnClientConnected;
            ClientManager.ClientDisconnected += OnClientDisconnected;

            logger.Info("Register Plugins...");
            // the plugins add them themself to the initializer
            InitializationManager = InitializationManager.Instance;
            InitializationManager.AddAssemblies(AppDomain.CurrentDomain.GetAssemblies());

            PluginManager = PluginManager.Instance;
            PluginManager.PluginAdded += OnPluginAdded;
            PluginManager.PluginRemoved += OnPluginRemoved;

            logger.Info("Loading Plugins...");
            PluginManager.Instance.LoadAllPlugins();

            if (IsExceptionLoggerEnabled)
            {
                ExceptionLogger = new RavenClient(ExceptionLoggerDSN);
                ExceptionLogger.Release = Version;
#if DEBUG
                ExceptionLogger.Environment = "DEBUG";
#else
                 ExceptionLogger.Environment = "RELEASE";
#endif
            }
        }

        public virtual void UpdateConfigFiles()
        {
            logger.Info("Recreate server config file ...");

            if (File.Exists(ConfigFilePath))
            {
                logger.Info("Update {0} file", ConfigFilePath);

                Config = new XmlConfig(ConfigFilePath);
                Config.AddAssemblies(LoadedAssemblies.Values.ToArray());
                Config.Load();
                Config.Create(true);
            }
            else
            {
                logger.Info("Create {0} file", ConfigFilePath);

                Config = new XmlConfig(ConfigFilePath);
                Config.AddAssemblies(LoadedAssemblies.Values.ToArray()); 
                Config.Create();
            }

            logger.Info("Recreate plugins config files ...", ConfigFilePath);

            PluginManager = PluginManager.Instance;
            PluginManager.Instance.LoadAllPlugins();

            foreach (var plugin in PluginManager.GetPlugins().Select(entry => entry.Plugin).OfType<PluginBase>())
            {
                if (!plugin.UseConfig || !plugin.AllowConfigUpdate)
                    continue;

                var update = File.Exists(plugin.GetConfigPath());

                if (!update)
                {
                    logger.Info("Create '{0}' config file => '{1}'", plugin.Name, Path.GetFileName(plugin.GetConfigPath()));
                }

                plugin.LoadConfig();

                if (!update)
                    continue;

                logger.Info("Update '{0}' config file => '{1}'", plugin.Name, Path.GetFileName(plugin.GetConfigPath()));
                plugin.Config.Create(true);
            }

            logger.Info("All config files were correctly updated/created ! Shutdown ...");
            Thread.Sleep(TimeSpan.FromSeconds(2.0));
            Environment.Exit(0);
        }

        /// <summary>
        /// Load before the runtime all referenced assemblies
        /// </summary>
        private static void PreLoadReferences(Assembly executingAssembly)
        {
            foreach (var loadedAssembly in from assemblyName in executingAssembly.GetReferencedAssemblies() where AppDomain.CurrentDomain.GetAssemblies().Count(entry => entry.GetName().FullName == assemblyName.FullName) <= 0 select Assembly.Load(assemblyName))
            {
                PreLoadReferences(loadedAssembly);
            }
        }

        protected virtual void OnPluginRemoved(PluginContext plugincontext)
        {
            logger.Info("Plugin Unloaded : {0}", plugincontext.Plugin.GetDefaultDescription());
        }

        protected virtual void OnPluginAdded(PluginContext plugincontext)
        {
            logger.Info("Plugin Loaded : {0}", plugincontext.Plugin.GetDefaultDescription());
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
            var name = args.LoadedAssembly.GetName().Name;

            if (!LoadedAssemblies.ContainsKey(name))
                LoadedAssemblies.Add(name, args.LoadedAssembly);
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

        private void OnContractFailed(object sender, ContractFailedEventArgs e)
        {
            logger.Fatal("Contract failed : {0}", e.Condition);

            if (e.OriginalException != null)
                HandleCrashException(e.OriginalException);
            else
            {
                logger.Fatal(string.Format(" Stack Trace:\r\n{0}", Environment.StackTrace));
            }

            e.SetHandled();
        }

        public void HandleCrashException(Exception e)
        {
            while (true)
            {
                ExceptionManager.RegisterException(e);

                logger.Fatal(string.Format(" Crash Exception : {0}\r\n", e.Message) + string.Format(" Source: {0} -> {1}\r\n", e.Source, e.TargetSite) + string.Format(" Stack Trace:\r\n{0}", e.StackTrace));

                if (e.InnerException != null)
                {
                    e = e.InnerException;
                    continue;
                }
                break;
            }
        }

        public virtual void Start()
        {
            Running = true;
            Initializing = false;

            IOTaskPool.CallPeriodically((int)TimeSpan.FromSeconds(10).TotalMilliseconds, KeepSQLConnectionAlive);

            var evnt = ServerStarted;
            if (evnt != null)
                evnt();
        }


        /// <summary>
        /// Allow the server to ignore the next modification of the config file.
        /// Use it when you save the config
        /// </summary>
        public void IgnoreNextConfigReload()
        {
            m_ignoreReload = true;
        }

        protected virtual void KeepSQLConnectionAlive()
        {
            try
            {
                DBAccessor.Database.Execute("DO 1");
            }
            catch (Exception ex)
            {
                logger.Error("Cannot ping SQL connection : {0}", ex);
                logger.Warn("Try to Re-open the connection");
                try
                {
                    DBAccessor.CloseConnection();
                    DBAccessor.OpenConnection();
                }
                catch (Exception ex2)
                {
                    logger.Error("Cannot reopen the SQL connection : {0}", ex2);
                }
            }
        }

        protected virtual void DisconnectAfkClient()
        {
            // todo : this is not an afk check but a timeout check

            var afkClients = ClientManager.FindAll(client =>
                DateTime.Now.Subtract(client.LastActivity).TotalSeconds >= Settings.InactivityDisconnectionTime);

            foreach (var client in afkClients)
            {
                client.Disconnect();
            }
        }

        protected abstract BaseClient CreateClient(Socket s);

        protected virtual void OnShutdown()
        {
            IOTaskPool.Stop();
        }

        public virtual void ScheduleShutdown(TimeSpan timeBeforeShuttingDown)
        {
            IsShutdownScheduled = true;
            ScheduledShutdownDate = DateTime.Now + timeBeforeShuttingDown;

            if (ScheduledShutdownTimer != null)
                IOTaskPool.RemoveTimer(ScheduledShutdownTimer);

            ScheduledShutdownTimer = IOTaskPool.CallPeriodically((int)TimeSpan.FromSeconds(1).TotalMilliseconds, CheckScheduledShutdown);
        }

        public virtual void CancelScheduledShutdown()
        {
            IsShutdownScheduled = false;
            ScheduledShutdownDate = DateTime.MaxValue;

            IOTaskPool.RemoveTimer(ScheduledShutdownTimer);
            ScheduledShutdownTimer = null;
        }

        protected virtual void CheckScheduledShutdown()
        {
            if ((ScheduledAutomaticShutdown && UpTime.TotalMinutes > AutomaticShutdownTimer) || 
                (IsShutdownScheduled && ScheduledShutdownDate <= DateTime.Now))
            {
                Shutdown();
            }
        }

        public void Shutdown()
        {
            lock (this)
            {
                Running = false;

                OnShutdown();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                // We are done at this point.
                Console.WriteLine("Application is now terminated. Wait " + Definitions.ExitWaitTime +
                                  " seconds to exit ... or press any key to cancel");

                if (IsNoExitShutdown)
                    Console.ReadKey(true);

                if (ConditionWaiter.WaitFor(() => Console.KeyAvailable, Definitions.ExitWaitTime * 1000, 20))
                {
                    Console.ReadKey(true);

                    Thread.Sleep(500);
                    Console.WriteLine("Press now a key to exit...");
                    
                    Console.ReadKey(true);
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