using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public static bool CheckPassword = false;
#else
        public static bool CheckPassword = true;
#endif

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public event Action<WorldServer> ServerAdded;

        private void NotifyServerAdded(WorldServer server)
        {
            OnServerAdded(server);

            Action<WorldServer> handler = ServerAdded;
            if (handler != null) 
                handler(server);
        }

        public event Action<WorldServer> ServerRemoved;

        private void NotifyServerRemoved(WorldServer server)
        {
            OnServerRemoved(server);

            Action<WorldServer> handler = ServerRemoved;
            if (handler != null) 
                handler(server);
        }

        public event Action<WorldServer> ServerStateChanged;

        private void NotifyServerStateChanged(WorldServer server)
        {
            OnServerStateChanged(server);

            Action<WorldServer> handler = ServerStateChanged;
            if (handler != null) 
                handler(server);
        }

        private Dictionary<int, WorldServer> m_realmlist;

        /// <summary>
        ///   Synchronization object.
        /// </summary>
        private object m_sync;

        #region Properties

        /// <summary>
        ///   List of registered world server
        /// </summary>
        public Dictionary<int, WorldServer> Realmlist
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
            m_realmlist = WorldServer.FindAll().ToDictionary(entry => entry.Id);

            foreach (var worldServer in m_realmlist)
            {
                worldServer.Value.Connected = false;
                worldServer.Value.Status = ServerStatusEnum.OFFLINE;
            }

            m_sync = new object();
        }

        /// <summary>
        ///   Start up a new task which will ping
        ///   connected world servers.
        /// </summary>
        public void Start()
        {
            Task.Factory.StartNew(CheckPing);
        }

        /// <summary>
        ///   Create a new world record and save it
        ///   directly in database.
        /// </summary>
        /// <param name = "record"></param>
        public void RegisterWorld(WorldServer record)
        {
            record.CreateAndFlush();

            lock (m_sync)
            {
                m_realmlist.Add(record.Id, record);
            }
        }

        /// <summary>
        ///   Create a new world record and save it
        ///   directly in database.
        /// </summary>
        public void CreateWorld(ref WorldServerData worldServerData)
        {
            // generate a new password to identify the world server
            string password = new Random().RandomString(32);
            worldServerData.Password = password;

            RegisterWorld(new WorldServer
                              {
                                  Id = worldServerData.Id,
                                  Ip = worldServerData.Address,
                                  Port = worldServerData.Port,
                                  Name = worldServerData.Name,
                                  Password = worldServerData.Password,
                                  RequireSubscription = false,
                                  RequiredRole = RoleEnum.Player,
                                  CharCapacity = 1000,
                                  ServerSelectable = true,
                              });
        }

        /// <summary>
        ///   Check and add a new world server to handle.
        ///   If server info differs from worldlist, it's rejected.
        /// </summary>
        /// <param name = "world"></param>
        /// <param name="channelPort"></param>
        /// <returns></returns>
        public bool AddWorld(ref WorldServerData world, int channelPort)
        {
            if (!m_realmlist.ContainsKey(world.Id))
            {
                if (AskAddWorldRecord(world))
                {
                    CreateWorld(ref world);
                }
                else
                {
                    logger.Error("Server <Id : {0}> is not registered in database. Check your worldlist's table.",
                                 world.Id);
                    return false;
                }
            }

            if (m_realmlist[world.Id].Ip != world.Address ||
                m_realmlist[world.Id].Port != world.Port ||
                m_realmlist[world.Id].Name != world.Name)
            {
                logger.Error(
                    "Server <Id : {0}> has unexpected properties.\nCheck your worldlist's table and your gameserver's configuration file. They may mismatch.",
                    world.Id);
                return false;
            }

            if (CheckPassword && m_realmlist[world.Id].Password != world.Password)
            {
                logger.Error(
                    "Server <Id : {0}> has an incorrect passsword",
                    world.Id);
                return false;
            }

            if (!m_realmlist[world.Id].Connected)
            {
                m_realmlist[world.Id].Connected = true;
                m_realmlist[world.Id].Status = ServerStatusEnum.ONLINE;
                IpcServer.Instance.RegisterTcpClient(world, channelPort);

                logger.Info("Registered World : \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Address);

                NotifyServerAdded(m_realmlist[world.Id]);
                return true;
            }
            logger.Error("Tried to register the server <Id : {0}> twice.", world.Id);
            return false;
        }


        public WorldServer GetWorldServer(int id)
        {
            return m_realmlist.ContainsKey(id) ? m_realmlist[id] : null;
        }

        public bool CanAccessToWorld(AuthClient client, WorldServer world)
        {
            return world != null && world.Status == ServerStatusEnum.ONLINE && client.Account.Role >= world.RequiredRole && world.CharsCount < world.CharCapacity &&
                   (!world.RequireSubscription || (client.Account.SubscriptionRemainingTime > 0));
        }

        public bool CanAccessToWorld(AuthClient client, int worldId)
        {
            WorldServer world = GetWorldServer(worldId);
            return world != null && world.Status == ServerStatusEnum.ONLINE && client.Account.Role >= world.RequiredRole && world.CharsCount < world.CharCapacity &&
                   (!world.RequireSubscription || (client.Account.SubscriptionRemainingTime > 0));
        }

        public void ChangeWorldState(int worldId, ServerStatusEnum state)
        {
            WorldServer world = GetWorldServer(worldId);
            if (world != null)
            {
                world.Status = state;
                NotifyServerStateChanged(world);
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
                                              client.Account.GetCharactersCountByWorld(world.Id));
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

        public bool CheckWorldAccess(WorldServerData server)
        {
            if (!m_realmlist.ContainsKey(server.Id))
                return false;

            if (CheckPassword && m_realmlist[server.Id].Password != server.Password)
                return false;

            return true;
        }

        public bool DoPing(int id)
        {
            if (!m_realmlist.ContainsKey(id))
                return false;

            m_realmlist[id].LastPing = DateTime.Now;

            return true;
        }

        /// <summary>
        ///   Remove a given world from our list
        ///   and set it off line.
        /// </summary>
        /// <param name = "world"></param>
        public void RemoveWorld(WorldServerData world)
        {
            IpcServer.Instance.UnRegisterTcpClient(world);

            lock (m_sync)
            {
                if (m_realmlist.ContainsKey(world.Id) && m_realmlist[world.Id].Connected)
                {
                    m_realmlist[world.Id].Connected = false;
                    m_realmlist[world.Id].Status = ServerStatusEnum.OFFLINE;

                    NotifyServerRemoved(m_realmlist[world.Id]);
                }
                logger.Info("Unregistered \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Address);
            }
        }

        /// <summary>
        ///   Remove a given world from our list
        ///   and set it off line.
        /// </summary>
        /// <param name = "world"></param>
        public void RemoveWorld(WorldServer world)
        {
            IpcServer.Instance.UnRegisterTcpClient(world.Id);

            lock (m_sync)
            {
                if (m_realmlist.ContainsKey(world.Id) && m_realmlist[world.Id].Connected)
                {
                    m_realmlist[world.Id].Connected = false;
                    m_realmlist[world.Id].Status = ServerStatusEnum.OFFLINE;

                    NotifyServerRemoved(m_realmlist[world.Id]);
                }
                logger.Info("Unregistered \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Ip);
            }
        }

        private static bool AskAddWorldRecord(WorldServerData worldServerData)
        {
            return
                AuthServer.Instance.ConsoleInterface.AskAndWait(
                    string.Format("Server {0} request to be registered. Accept Request ?",
                                  worldServerData.Name, WorldServerTimeout), WorldServerTimeout);
        }

        /// <summary>
        ///   Check each second if the world servers are alive
        /// </summary>
        private void CheckPing()
        {
            while (AuthServer.Instance.Running)
            {
                lock (m_sync)
                {
                    foreach (WorldServer worldServer in m_realmlist.Values)
                    {
                        if (!worldServer.Connected)
                            continue;

                        // check if the world server has pinged recently
                        if ((DateTime.Now - worldServer.LastPing).TotalMilliseconds > WorldServerTimeout*1000)
                        {
                            // the world server is disconnected
                            logger.Warn("WorldServer \"{0}\" <id:{1}> has timed out.", worldServer.Name, worldServer.Id);
                            RemoveWorld(worldServer);
                        }
                    }
                }

                Thread.Sleep(PingCheckInterval);
            }
        }

        private static void OnServerAdded(WorldServer worldServer)
        {
            ClientManager.Instance.FindAll<AuthClient>(entry => entry.LookingOfServers).
                DoForAll(entry => ConnectionHandler.SendServerStatusUpdateMessage(entry, worldServer));
        }

        private static void OnServerRemoved(WorldServer worldServer)
        {
            ClientManager.Instance.FindAll<AuthClient>(entry => entry.LookingOfServers).
                DoForAll(entry => ConnectionHandler.SendServerStatusUpdateMessage(entry, worldServer));
        }

        private static void OnServerStateChanged(WorldServer worldServer)
        {
            ClientManager.Instance.FindAll<AuthClient>(entry => entry.LookingOfServers).
                DoForAll(entry => ConnectionHandler.SendServerStatusUpdateMessage(entry, worldServer));
        }
    }
}