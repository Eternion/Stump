
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Stump.Core.Attributes;
using Stump.Core.Mathematics;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.ORM;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;
using Stump.Server.WorldServer.Core.IO;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using ServiceStack.Text;
using DatabaseConfiguration = Stump.ORM.DatabaseConfiguration;

namespace Stump.Server.WorldServer
{
    public class WorldServer : ServerBase<WorldServer>
    {
        /// <summary>
        /// Current server adress
        /// </summary>
        [Variable]
        public readonly static string Host = "127.0.0.1";

        /// <summary>
        /// Server port
        /// </summary>
        [Variable]
        public readonly static int Port = 3467;

        [Variable(true)]
        public static WorldServerData ServerInformation = new WorldServerData
        {
            Id = 1,
            Name = "Jiva",
            Address = "localhost",
            Port = 3467,
            Capacity = 2000,
            RequiredRole = RoleEnum.Player,
            RequireSubscription = false,
        };

        [Variable(Priority = 10)]
        public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            Host = "localhost",
            Port = "3306",
            DbName = "stump_world",
            User = "root",
            Password = "",
            ProviderName = "MySql.Data.MySqlClient",
            //UpdateFileDir = "./sql_update/",
        };

        [Variable(true)]
        public static int AutoSaveInterval  = 3 * 60;

        [Variable(true)]
        public static bool SaveMessage = true;

        public WorldVirtualConsole VirtualConsoleInterface
        {
            get;
            protected set;
        }

        public WorldPacketHandler HandlerManager
        {
            get;
            private set;
        }
        public WorldServer()
            : base(Definitions.ConfigFilePath, Definitions.SchemaFilePath)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
            ConsoleInterface = new WorldConsole();
            VirtualConsoleInterface = new WorldVirtualConsole();
            ConsoleBase.SetTitle("#Stump World Server : " + ServerInformation.Name);

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

            logger.Info("Register Packet Handlers...");
            HandlerManager = WorldPacketHandler.Instance;
            HandlerManager.RegisterAll(Assembly.GetExecutingAssembly());

            logger.Info("Register Commands...");
            CommandManager.RegisterAll(Assembly.GetExecutingAssembly());

            InitializationManager.InitializeAll();
            CommandManager.LoadOrCreateCommandsInfo(CommandsInfoFilePath);
            IsInitialized = true;
        }

        protected override void OnPluginAdded(PluginContext plugincontext)
        {
            CommandManager.RegisterAll(plugincontext.PluginAssembly);

            base.OnPluginAdded(plugincontext);
        }

        public override void Start()
        {
            base.Start();

            logger.Info("Start Auto-Save Cyclic Task");
            IOTaskPool.CallPeriodically(AutoSaveInterval * 1000, World.Instance.Save);

            logger.Info("Starting Console Handler Interface...");
            ConsoleInterface.Start();

            logger.Info("Starting IPC Communications ...");
            IPCAccessor.Instance.Start();

            logger.Info("Start listening on port : " + Port + "...");
            ClientManager.Start(Host, Port);

            StartTime = DateTime.Now;
        }

        protected override BaseClient CreateClient(Socket s)
        {
            return new WorldClient(s);
        }

        protected override void DisconnectAfkClient()
        {
            // todo : this is not an afk check but a timeout check

            var afkClients = FindClients(client =>
                DateTime.Now.Subtract(client.LastActivity).TotalSeconds >= BaseServer.Settings.InactivityDisconnectionTime);

            foreach (var client in afkClients)
            {
                client.DisconnectAfk();
            }
        }

        public bool DisconnectClient(int accountId)
        {
            IEnumerable<WorldClient> clients = FindClients(client => client.Account != null && client.Account.Id == accountId);

            foreach (var client in clients)
            {
                client.Disconnect();
            }

            return clients.Any();
        }

        public WorldClient[] FindClients(Predicate<WorldClient> predicate)
        {
            return ClientManager.FindAll(predicate);
        }

        private TimeSpan? m_lastAnnouncedTime;

        public override void ScheduleShutdown(TimeSpan timeBeforeShuttingDown)
        {
            base.ScheduleShutdown(timeBeforeShuttingDown);

            AnnounceTimeBeforeShutdown(timeBeforeShuttingDown, false);
        }

        public override void CancelScheduledShutdown()
        {
            base.CancelScheduledShutdown();

            World.Instance.SendAnnounce("Reboot canceled !", Color.Red);
        }

        protected override void CheckScheduledShutdown()
        {
            var diff = TimeSpan.FromMinutes(AutomaticShutdownTimer) - UpTime;
            var automatic = true;

            if (IsShutdownScheduled && diff > ScheduledShutdownDate - DateTime.Now)
            {
                diff = ScheduledShutdownDate - DateTime.Now;
                automatic = false;
            }

            if (diff < TimeSpan.FromMinutes(30))
            {
                var announceDiff = m_lastAnnouncedTime.HasValue ? TimeSpan.MaxValue : m_lastAnnouncedTime - diff;

                if (diff > TimeSpan.FromMinutes(10) && announceDiff >= TimeSpan.FromMinutes(5))
                {
                    AnnounceTimeBeforeShutdown(TimeSpan.FromMinutes(diff.TotalMinutes.RoundToNearest(5)), automatic);
                }
                if (diff > TimeSpan.FromMinutes(5) && diff <= TimeSpan.FromMinutes(10) && announceDiff >= TimeSpan.FromMinutes(1))
                {
                    AnnounceTimeBeforeShutdown(TimeSpan.FromMinutes(diff.TotalMinutes), automatic);
                }
                if (diff > TimeSpan.FromMinutes(1) && diff <= TimeSpan.FromMinutes(5) && announceDiff >= TimeSpan.FromSeconds(30))
                {
                    AnnounceTimeBeforeShutdown(new TimeSpan(0, 0, 0, (int)diff.TotalSeconds.RoundToNearest(30)), automatic);
                }
                if (diff > TimeSpan.FromSeconds(10) && diff <= TimeSpan.FromMinutes(1) && announceDiff >= TimeSpan.FromSeconds(10))
                {
                    AnnounceTimeBeforeShutdown(new TimeSpan(0, 0, 0, (int)diff.TotalSeconds.RoundToNearest(10)), automatic);
                }
                if (diff <= TimeSpan.FromSeconds(10) && diff > TimeSpan.Zero)
                {
                    AnnounceTimeBeforeShutdown(TimeSpan.FromSeconds(diff.Seconds.RoundToNearest(5)), automatic);
                }
            }

            base.CheckScheduledShutdown();
        }

        private void AnnounceTimeBeforeShutdown(TimeSpan time, bool automatic)
        {
            var message = automatic ? @"Automatic reboot in <b>{0:mm\:ss}</b>" : @"Reboot in <b>{0:mm\:ss}</b>";

            if (!automatic && !string.IsNullOrEmpty(ScheduledShutdownReason))
                message += " : " + ScheduledShutdownReason;

            World.Instance.SendAnnounce(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 15, time);

            m_lastAnnouncedTime = time;
        }

        protected override void OnShutdown()
        {
            if (IsInitialized)
            {
                var wait = new AutoResetEvent(false);
                IOTaskPool.ExecuteInContext(() =>
                {
                        World.Instance.Stop(true);
                        World.Instance.Save();
                        wait.Set();
                    });

                wait.WaitOne();
            }

            IPCAccessor.Instance.Stop();

            if (IOTaskPool != null)
                IOTaskPool.Stop();

            ClientManager.Pause();

            foreach (var client in ClientManager.Clients.ToArray())
            {
                client.Disconnect();
            }

            ClientManager.Close();
        }
    }
}