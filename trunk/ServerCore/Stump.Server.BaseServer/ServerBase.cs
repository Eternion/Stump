// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime;
using System.Threading;
using NLog;
using Stump.BaseCore.Framework.IO;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;

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

        public XmlConfigFile ConfigFile
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

        public bool Running
        {
            get;
            set;
        }

        public virtual void Initialize()
        {
            Instance = this as T;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

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
            ConfigFile = new XmlConfigFile(
                ConfigFilePath,
                SchemaFilePath);
            ConfigFile.DefinesVariables(ref LoadedAssemblies);

            logger.Info("Initializing Network Interfaces...");
            QueueDispatcher = new QueueDispatcher(Settings.EnableBenchmarking);
            HandlerManager = new HandlerManager();
            WorkerManager = new WorkerManager(QueueDispatcher, HandlerManager);
          
            CommandManager = new CommandsManager();

            MessageListener = new MessageListener(QueueDispatcher, CreateClient);
            MessageListener.Initialize();

            MessageListener.ClientConnected += OnClientConnected;
            MessageListener.ClientDisconnected += OnClientDisconnected;
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

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            if (args.IsTerminating)
                logger.Fatal("Application has crashed. An Unhandled Exception has been thrown :");

            logger.Error("Unhandled Exception : " + ((Exception) args.ExceptionObject).Message);
            logger.Error("Source : {0} Method : {1}", ( (Exception)args.ExceptionObject ).Source,
                         ( (Exception)args.ExceptionObject ).TargetSite);
            logger.Error("Stack Trace : " + ( (Exception)args.ExceptionObject ).StackTrace);

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
            logger.Info("Start listening on port : " + MessageListener.Port + "...");
            MessageListener.Start();

            Running = true;
        }

        // todo : don't use Sleep
        public virtual void Update()
        {
            Thread.Sleep(10);
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