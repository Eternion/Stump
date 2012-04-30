using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.AuthServer.Database.World;
using Stump.Server.AuthServer.Handlers.Connection;
using Stump.Server.AuthServer.IPC;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Network;
using System.Net;

namespace Stump.Server.AuthServer.Managers
{
    /// <summary>
    ///   Manager for handling different connected worlds
    ///   as well as the database's worldlist.
    /// </summary>
    public class WorldServerManager : Singleton<WorldServerManager>
    {
        /// <summary>
        ///   Defines after how many seconds a world server is considered as timed out.
        /// </summary>
        [Variable(true)]
        public static int WorldServerTimeout = 20;

        /// <summary>
        /// Interval between two ping to check if world server is still alive (in milliseconds)
        /// </summary>
        [Variable(true)]
        public static int PingCheckInterval = 2000;

        [Variable(true)]
#if DEBUG
        public static bool CheckPassword;
#else
        public static bool CheckPassword = true;
#endif

        [Variable(true)]
        public static List<string> AllowedServerIps = new List<string>
        {
            "127.0.0.1",
        };

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public event Action<WorldServer> ServerAdded;

        private void OnServerAdded(WorldServer server)
        {
            ClientManager.Instance.FindAll<AuthClient>(entry => entry.LookingOfServers).
                ForEach(entry => ConnectionHandler.SendServerStatusUpdateMessage(entry, server));

            Action<WorldServer> handler = ServerAdded;
            if (handler != null)
                handler(server);
        }

        public event Action<WorldServer> ServerRemoved;

        private void OnServerRemoved(WorldServer server)
        {
            ClientManager.Instance.FindAll<AuthClient>(entry => entry.LookingOfServers).
                ForEach(entry => ConnectionHandler.SendServerStatusUpdateMessage(entry, server));

            Action<WorldServer> handler = ServerRemoved;
            if (handler != null)
                handler(server);
        }

        public event Action<WorldServer> ServerStateChanged;

        private void OnServerStateChanged(WorldServer server)
        {
            ClientManager.Instance.FindAll<AuthClient>(entry => entry.LookingOfServers).
                ForEach(entry => ConnectionHandler.SendServerStatusUpdateMessage(entry, server));

            Action<WorldServer> handler = ServerStateChanged;
            if (handler != null)
                handler(server);
        }

        private ConcurrentDictionary<int, WorldServer> m_realmlist;


        #region Properties

        /// <summary>
        ///   List of registered world server
        /// </summary>
        public ConcurrentDictionary<int, WorldServer> Realmlist
        {
            get { return m_realmlist; }
        }

        #endregion

        /// <summary>
        ///   Initialize up our list and get all
        ///   world registered in our database in
        ///   "world list".
        /// </summary>
        public void Initialize()
        {
            m_realmlist = new ConcurrentDictionary<int, WorldServer>(WorldServer.FindAll().ToDictionary(entry => entry.Id));

            foreach (var worldServer in m_realmlist)
            {
                worldServer.Value.SetOffline();
            }
        }

        /// <summary>
        ///   Start up a new task which will ping
        ///   connected world servers.
        /// </summary>
        public void Start()
        {
            //AuthServer.Instance.IOTaskPool.CallPeriodically(PingCheckInterval, CheckPing);
        }

        /// <summary>
        ///   Create a new world record and save it
        ///   directly in database.
        /// </summary>
        public WorldServer CreateWorld(WorldServerData worldServerData)
        {
            var record = new WorldServer
                              {
                                  Id = worldServerData.Id,
                                  Name = worldServerData.Name,
                                  RequireSubscription = false,
                                  RequiredRole = RoleEnum.Player,
                                  CharCapacity = 1000,
                                  ServerSelectable = true,
                              };

            if (!m_realmlist.TryAdd(record.Id, record))
                throw new Exception("Server already registered");

            record.Create();

            return record;
        }

        /// <summary>
        ///   Check and add a new world server to handle.
        ///   If server info differs from worldlist, it's rejected.
        /// </summary>
        /// <returns></returns>
        public RegisterResultEnum RequestConnection(WorldServerData world, IContextChannel channel, RemoteEndpointMessageProperty endpoint, string sessionId)
        {
            WorldServer server;
            if (!m_realmlist.ContainsKey(world.Id))
            {
                //if (AskAddWorldRecord(world))
                IPAddress[] addresses = Dns.GetHostAddresses(world.Address);
                foreach (IPAddress ipAddress in addresses)
                {
                    logger.Debug(ipAddress.ToString());
                }

                if (AllowedServerIps.Any(entry => addresses.Any(subentry => entry == subentry.ToString())))
                {
                    server = CreateWorld(world);
                }
                else
                {
                    logger.Error("Server <Id : {0}> ({1}) Try to connect self on the authenfication server but its ip is not allowed (see WorldServerManager.AllowedServerIps)",
                                 world.Id, endpoint.Address);
                    return RegisterResultEnum.IpNotAllowed;
                }
            }
            else
                server = m_realmlist[world.Id];

            if (server.Name != world.Name)
            {
                logger.Error(
                    "Server <Id : {0}> has unexpected properties.\nCheck your worldlist's table and your gameserver's configuration file. They may mismatch.",
                    world.Id);
                return RegisterResultEnum.PropertiesMismatch;
            }

            server.SetOnline(world.Address, world.Port);
            server.SetSession(channel, sessionId, endpoint);

            logger.Info("Registered World : \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Address);

            OnServerAdded(server);
            return RegisterResultEnum.OK;
            
        }


        public WorldServer GetServerById(int id)
        {
            return m_realmlist.ContainsKey(id) ? m_realmlist[id] : null;
        }

        public WorldServer GetServerBySessionId(string sessionId)
        {
            return (from server in Realmlist where !string.IsNullOrEmpty(server.Value.SessionId) && server.Value.SessionId == sessionId select server.Value).FirstOrDefault();
        }

        public bool CanAccessToWorld(AuthClient client, WorldServer world)
        {
            return world != null && world.Status == ServerStatusEnum.ONLINE && client.Account.Role >= world.RequiredRole && world.CharsCount < world.CharCapacity &&
                   (!world.RequireSubscription || (client.Account.SubscriptionRemainingTime > 0));
        }

        public bool CanAccessToWorld(AuthClient client, int worldId)
        {
            WorldServer world = GetServerById(worldId);
            return world != null && world.Status == ServerStatusEnum.ONLINE && client.Account.Role >= world.RequiredRole && world.CharsCount < world.CharCapacity &&
                   (!world.RequireSubscription || (client.Account.SubscriptionRemainingTime > 0));
        }

        public void ChangeWorldState(int worldId, ServerStatusEnum state)
        {
            WorldServer world = GetServerById(worldId);
            if (world != null)
            {
                world.Status = state;
                OnServerStateChanged(world);
            }
        }

        public void ChangeWorldState(WorldServer server, ServerStatusEnum state)
        {
            if (server != null)
            {
                server.Status = state;
                OnServerStateChanged(server);
            }
        }

        public IEnumerable<GameServerInformations> GetServersInformationArray(AuthClient client)
        {
            return m_realmlist.Values.Select(
                world => GetServerInformation(client, world));
        }

        public GameServerInformations GetServerInformation(AuthClient client, WorldServer world)
        {
            return new GameServerInformations((ushort) world.Id, (sbyte) world.Status,
                                              (sbyte) world.Completion,
                                              world.ServerSelectable,
                                              client.Account.GetCharactersCountByWorld(world.Id),
                                              DateTime.Now.Ticks);
        }

        /// <summary>
        ///   Check if we have got a world identified
        ///   by given id.
        /// </summary>
        /// <param name = "id">World's identifier to check.</param>
        /// <returns></returns>
        public bool HasWorld(int id)
        {
            return m_realmlist.ContainsKey(id);
        }

        /// <summary>
        ///   Remove a given world from our list
        ///   and set it off line.
        /// </summary>
        /// <param name = "world"></param>
        public void RemoveWorld(WorldServerData world)
        {
            var server = GetServerById(world.Id);

            if (server != null && server.Connected)
            {
                server.SetOffline();
                server.CloseSession();

                OnServerRemoved(m_realmlist[world.Id]);

                logger.Info("Unregistered \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Address);
            }

        }

        /// <summary>
        ///   Remove a given world from our list
        ///   and set it offline.
        /// </summary>
        /// <param name = "world"></param>
        public void RemoveWorld(WorldServer world)
        {
            var server = GetServerById(world.Id);

            if (server != null && server.Connected)
            {
                server.SetOffline();
                server.CloseSession();

                OnServerRemoved(m_realmlist[world.Id]);

                logger.Info("Unregistered \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Address);
            }

        }

        private static bool AskAddWorldRecord(WorldServerData worldServerData)
        {
            return
                AuthServer.Instance.ConsoleInterface.AskAndWait(
                    string.Format("Server {0} request to be registered. Accept Request ?",
                                  worldServerData.Name), WorldServerTimeout);
        }
    }
}