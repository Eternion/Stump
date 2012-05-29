
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Castle.ActiveRecord.Framework.Config;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Mathematics;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool.D2p;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;
using Stump.Server.WorldServer.Core.IO;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Conditions;
using TreeSharp;

namespace Stump.Server.WorldServer
{
    public class WorldServer : ServerBase<WorldServer>, IRemoteWorldOperations
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
        };

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

        [Variable(true)]
        public static int AutoSaveInterval  = 3 * 60;

#if DEBUG
        [DllImport("user32.dll")]
        static extern short GetKeyState(int nVirtKey);
#endif

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
            ConsoleBase.SetTitle("#Stump World Server : " + ServerInformation.Name);

            logger.Info("Initializing Database...");
            DatabaseAccessor = new DatabaseAccessor(DatabaseConfiguration, Definitions.DatabaseRevision, typeof(WorldBaseRecord<>), Assembly.GetExecutingAssembly(), true);
            DatabaseAccessor.Initialize();

            logger.Info("Opening Database...");
            DatabaseAccessor.OpenDatabase();

            logger.Info("Register Messages...");
            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();

            logger.Info("Register Packet Handlers...");
            HandlerManager = WorldPacketHandler.Instance;
            HandlerManager.RegisterAll(Assembly.GetExecutingAssembly());

            logger.Info("Register Commands...");
            CommandManager.RegisterAll(Assembly.GetExecutingAssembly());

            logger.Info("Initializing IPC Client..");
            IpcAccessor.Instance.Initialize();

#if DEBUG
            if (( GetKeyState(0x41) & 0x8000 ) == 0)
#endif
                InitializationManager.InitializeAll();
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
            IpcAccessor.Instance.Start();

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

            foreach (WorldClient client in afkClients)
            {
                client.DisconnectAfk();
            }
        }

        public bool DisconnectClient(uint accountId)
        {
            IEnumerable<WorldClient> clients = WorldServer.Instance.FindClients(client => client.Account != null && client.Account.Id == accountId);

            foreach (WorldClient client in clients)
            {
                client.Disconnect();
            }

            return clients.Any();
        }

        protected override void OnShutdown()
        {
            if (IsInitialized)
            {
                World.Instance.Save();
                World.Instance.Stop();
            }

            IpcAccessor.Instance.Stop();

            if (IOTaskPool != null)
                IOTaskPool.Stop();

            ClientManager.Pause();

            foreach (var client in ClientManager.Clients.ToArray())
            {
                client.Disconnect();
            }

            ClientManager.Close();
        }

        public WorldClient[] FindClients(Predicate<WorldClient> predicate)
        {
            return ClientManager.FindAll(predicate);
        }

        public void RegisterAllSaveableInstances(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.HasInterface(typeof(ISaveable)))
                {
                    if (type.IsDerivedFromGenericType(typeof (Singleton<>)))
                    {
                        var instanceProp = type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        var instance = instanceProp.GetValue(null, new object[0]) as ISaveable;

                        if (instance != null)
                        {
                            World.Instance.RegisterSaveableInstance(instance);
                        }
                    }
                }
            }
        }
    }
}