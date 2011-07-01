
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.Core.Xml;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.DofusProtocol;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy
{
    public class Proxy : Singleton<Proxy>
    {
        private static Logger logger;

        [Variable]
        public static string AuthAddress = "127.0.0.1";

        [Variable]
        public static int AuthPort = 5555;

        [Variable]
        public static string WorldAddress = "127.0.0.1";

        [Variable]
        public static int WorldPort = 5556;

        [Variable]
        public static string RealServerAddress = "213.248.126.180";

        [Variable]
        public static int RealServerPort = 5555;


        private Dictionary<string, Assembly> m_loadedAssemblies;

        public XmlConfigReader ConfigReader
        {
            get;
            private set;
        }

        public MessageListener MessageListenerAuth
        {
            get;
            private set;
        }

        public MessageListener MessageListenerWorld
        {
            get;
            private set;
        }

        public WorkerManager WorkerManager
        {
            get;
            private set;
        }

        public MessageQueue MessageQueue
        {
            get;
            private set;
        }

        public HandlerManager HandlerManager
        {
            get;
            private set;
        }

        public bool Running
        {
            get;
            set;
        }

        public void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += (OnUnhandledException);

            m_loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;

            NLogHelper.DefineLogProfile(true, true);
            NLogHelper.EnableLogging();
            logger = LogManager.GetCurrentClassLogger();

            logger.Info("Initializing Configuration...");
            ConfigReader = new XmlConfigReader(
                "proxy_config.xml",
                "proxy_config.xsd");
            ConfigReader.DefinesVariables(ref m_loadedAssemblies);

            logger.Info("Initializing Network Interfaces...");
            MessageQueue = new MessageQueue(false);
            HandlerManager = new HandlerManager();
            WorkerManager = new WorkerManager(MessageQueue, HandlerManager);

            MessageListenerAuth = new MessageListener(MessageQueue, CreateClientAuth, AuthAddress, AuthPort);
            MessageListenerAuth.Initialize();

            MessageListenerWorld = new MessageListener(MessageQueue, CreateClientWorld, WorldAddress, WorldPort);
            MessageListenerWorld.Initialize();

            logger.Info("Initializing Network Messages...");
            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();
            HandlerManager.RegisterAll(typeof(Proxy).Assembly);

            logger.Info("Loading Static Data...");
            ZonesManager.Initialize();

            Start();
        }

        public void Start()
        {
            logger.Info("Start listening on port : " + AuthPort + "...");
            MessageListenerAuth.Start();

            logger.Info("Start listening on port : " + WorldPort + "...");
            MessageListenerWorld.Start();

            Running = true;
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