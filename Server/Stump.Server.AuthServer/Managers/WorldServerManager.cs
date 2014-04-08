using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Handlers.Connection;
using Stump.Server.AuthServer.IPC;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Managers
{
    /// <summary>
    ///   Manager for handling different connected worlds
    ///   as well as the database's worldlist.
    /// </summary>
    public class WorldServerManager : DataManager<WorldServerManager>
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

            var handler = ServerAdded;
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

        /// <summary>
        ///   Initialize up our list and get all
        ///   world registered in our database in
        ///   "world list".
        /// </summary>
        public override void Initialize()
        {
            var servers = Database.Query<WorldServer>(WorldServerRelator.FetchQuery);
            m_realmlist = new ConcurrentDictionary<int, WorldServer>(servers.ToDictionary(entry => entry.Id));

            foreach (var worldServer in m_realmlist)
            {
                worldServer.Value.SetOffline();
                Database.Update(worldServer.Value);
            }
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
                                  RequireSubscription = worldServerData.RequireSubscription,
                                  RequiredRole = worldServerData.RequiredRole,
                                  CharCapacity = worldServerData.Capacity,
                                  ServerSelectable = true,
                                  Address = worldServerData.Address,
                                  Port = worldServerData.Port,
                              };

            if (!m_realmlist.TryAdd(record.Id, record))
                throw new Exception("Server already registered");

            Database.Insert(record);

            logger.Info(string.Format("World {0} created", worldServerData.Name));

            return record;
        }

        public void UpdateWorld(WorldServer record, WorldServerData worldServerData, bool save = true)
        {
            if (record.Id != worldServerData.Id)
                throw new Exception("Ids don't match");

            record.Name = worldServerData.Name;
            record.Address = worldServerData.Address;
            record.Port = worldServerData.Port;
            record.CharCapacity = worldServerData.Capacity;
            record.RequiredRole = worldServerData.RequiredRole;
            record.RequireSubscription = worldServerData.RequireSubscription;

            if (save)
                Database.Update(record);
        }

        public WorldServer RequestConnection(IPCClient client, WorldServerData world)
        {
            //check ip
            if (!IsIPAllowed(client.Address))
            {
                logger.Error("Server <Id : {0}> ({1}) Try to connect self on the authenfication server but its ip is not allowed (see WorldServerManager.AllowedServerIps)",
                                world.Id, client.Address);
                throw new Exception(string.Format("Ip {0} not allow by auth. server", client.Address));
            }

            WorldServer server;
            if (!m_realmlist.ContainsKey(world.Id))
                server = CreateWorld(world);
            else
            {
                server = m_realmlist[world.Id];
                UpdateWorld(server, world, false);
            }

            server.SetOnline(client);
            Database.Update(server);

            logger.Info("Registered World : \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Address);

            OnServerAdded(server);
            return server;
        }

        public bool IsIPAllowed(IPAddress ip)
        {
            return AllowedServerIps.Select(IPAddressRange.Parse).Any(x => x.Match(ip));
        }

        public WorldServer GetServerById(int id)
        {
            return m_realmlist.ContainsKey(id) ? m_realmlist[id] : null;
        }

        public bool CanAccessToWorld(AuthClient client, WorldServer world)
        {
            if (world == null)
                return false;

            if (world.Status != ServerStatusEnum.ONLINE)
                return false;

            if (!client.UserGroup.CanAccessWorld(world))
                return false;

            if (world.CharsCount >= world.CharCapacity)
                return false;

            if (world.RequireSubscription && (client.Account.SubscriptionEnd < DateTime.Now))
                return false;

            return true;
        }

        public bool CanAccessToWorld(AuthClient client, int worldId)
        {
            var world = GetServerById(worldId);
            return CanAccessToWorld(client, world);
        }

        public void ChangeWorldState(WorldServer server, ServerStatusEnum state, bool save = true)
        {
            server.Status = state;
            OnServerStateChanged(server);

            if (save)
                Database.Update(server);
        }

        public GameServerInformations[] GetServersInformationArray(AuthClient client)
        {
            return m_realmlist.Values.Where(x => client.UserGroup.CanAccessWorld(x)).Select(
                world => GetServerInformation(client, world)).ToArray();
        }

        public GameServerInformations GetServerInformation(AuthClient client, WorldServer world)
        {
            return new GameServerInformations((ushort) world.Id, (sbyte) world.Status,
                                              (sbyte) world.Completion,
                                              world.ServerSelectable,
                                              client.Account.GetCharactersCountByWorld(world.Id),
                                              DateTime.Now.GetUnixTimeStampLong());
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
        ///   and set it offline.
        /// </summary>
        /// <param name = "world"></param>
        public void RemoveWorld(WorldServerData world)
        {
            var server = GetServerById(world.Id);

            if (server != null && server.Connected)
            {
                server.SetOffline();
                Database.Update(server);

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
                Database.Update(server);

                OnServerRemoved(m_realmlist[world.Id]);

                logger.Info("Unregistered \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Address);
            }

        }
    }
}