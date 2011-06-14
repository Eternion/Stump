
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.Database.AuthServer.World;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Handlers;
using Stump.Server.AuthServer.IPC;
using Stump.Server.BaseServer.IPC;

namespace Stump.Server.AuthServer
{
    /// <summary>
    ///   Manager for handling different connected worlds
    ///   as well as the database's worldlist.
    /// </summary>
    public static class WorldServerManager
    {
        /// <summary>
        ///   Defines after how many seconds a world server is considered as timed out.
        /// </summary>
        [Variable]
        public static int WorldServerTimeout = 20;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   List containing worlds registered and handled by authserver.
        /// </summary>
        private static List<WorldServerInformation> m_registeredWorlds;

        /// <summary>
        ///   List of world by Id taken from database.
        /// </summary>
        private static Dictionary<int, WorldRecord> m_realmlist;

        /// <summary>
        ///   Synchronization object.
        /// </summary>
        private static object m_sync;

        #region Properties

        public static List<WorldServerInformation> Worlds
        {
            get { return m_registeredWorlds; }
        }

        public static Dictionary<int, WorldRecord> Realmlist
        {
            get { return m_realmlist; }
        }

        #endregion

        /// <summary>
        ///   Initialize up our list and get all
        ///   world registered in our database in
        ///   "world list".
        /// </summary>
        public static void Initialize()
        {
            m_registeredWorlds = new List<WorldServerInformation>();
            m_realmlist = WorldRecord.FindAll().ToDictionary(entry => entry.Id) ?? new Dictionary<int, WorldRecord>();

            foreach (var worldRecord in m_realmlist)
            {
                worldRecord.Value.Status = ServerStatusEnum.OFFLINE;
            }

            m_sync = new object();
        }

        /// <summary>
        ///   Start up a new task which will ping
        ///   connected world servers.
        /// </summary>
        public static void Start()
        {
            Task.Factory.StartNew(CheckPing);
        }

        /// <summary>
        ///   Create a new world record and save it
        ///   directly in database.
        /// </summary>
        /// <param name = "record"></param>
        public static void CreateWorld(WorldRecord record)
        {
            record.CreateAndFlush();

            m_realmlist.Add(record.Id, record);
        }

        /// <summary>
        ///   Create a new world record and save it
        ///   directly in database.
        /// </summary>
        public static void CreateWorld(WorldServerInformation worldServerInformation)
        {
            CreateWorld(new WorldRecord
                {
                    Id = worldServerInformation.Id,
                    Ip = worldServerInformation.Address,
                    Port = worldServerInformation.Port,
                    Name = worldServerInformation.Name,
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
        /// <returns></returns>
        public static bool AddWorld(WorldServerInformation world)
        {
            lock (m_sync)
            {
                if (!m_realmlist.ContainsKey(world.Id))
                {
                    if (AskAddWorldRecord(world))
                    {
                        CreateWorld(world);
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

                if (Worlds.Count(entry => entry.Id == world.Id) == 0)
                {
                    Worlds.Add(world);
                    m_realmlist[world.Id].Connected = true;

                    m_realmlist[world.Id].Status = ServerStatusEnum.ONLINE;

                    logger.Info("Registered World : \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Address);

                    OnServerChange(m_realmlist[world.Id]);
                    return true;
                }
                else
                {
                    logger.Error("Tried to register the server <Id : {0}> twice.", world.Id);
                    return false;
                }
            }
        }

        /// <summary>
        ///   Get a world from our list.
        /// </summary>
        /// <param name = "id">identifier of the world.</param>
        /// <returns></returns>
        public static WorldServerInformation GetWorld(int id)
        {
            lock (m_sync)
            {
                WorldServerInformation gs = Worlds.Find(o => o.Id == id);

                return gs;
            }
        }

        public static WorldRecord GetWorldRecord(int id)
        {
            lock (m_sync)
            {
                if (m_realmlist.ContainsKey(id))
                    return m_realmlist[id];
                else
                    return null;
            }
        }

        public static bool CanAccessToWorld(AuthClient client, WorldRecord world)
        {
            return world != null && world.Status == ServerStatusEnum.ONLINE && client.Account.Role >= world.RequiredRole && world.CharsCount < world.CharCapacity &&
                    (!world.RequireSubscription || (client.Account.SubscriptionRemainingTime > 0));
        }

        public static bool CanAccessToWorld(AuthClient client, int worldId)
        {
            var world = GetWorldRecord(worldId);
            return world != null && world.Status == ServerStatusEnum.ONLINE && client.Account.Role >= world.RequiredRole && world.CharsCount < world.CharCapacity &&
                    (!world.RequireSubscription || (client.Account.SubscriptionRemainingTime > 0));
        }

        public static void ChangeWorldState(int worldId, ServerStatusEnum state)
        {
            var world = GetWorldRecord(worldId);
            if (world != null)
            {
                world.Status = state;
                OnServerChange(world);
            }
        }

        public static List<GameServerInformations> GetServersInformationList(AuthClient client)
        {
            return m_realmlist.Values.Select(
                    world =>
                    new GameServerInformations((uint)world.Id, (uint)world.Status,
                                               (uint)world.Completion,
                                               world.ServerSelectable,
                                               client.Account.GetCharactersCountByWorld(world.Id))).ToList();
        }

        public static GameServerInformations GetServerInformation(AuthClient client, WorldRecord world)
        {
            return new GameServerInformations((uint)world.Id, (uint)world.Status,
                                               (uint)world.Completion,
                                               world.ServerSelectable,
                                               client.Account.GetCharactersCountByWorld(world.Id));
        }

        /// <summary>
        ///   Check if we have got a world identified
        ///   by given id.
        /// </summary>
        /// <param name = "id">World's identifier to check.</param>
        /// <returns></returns>
        public static bool HasWorld(int id)
        {
            lock (m_sync)
            {
                return Worlds.Exists(o => o.Id == id);
            }
        }

        /// <summary>
        ///   Remove a given world from our list
        ///   and set it off line.
        /// </summary>
        /// <param name = "world"></param>
        public static void RemoveWorld(WorldServerInformation world)
        {
            IpcServer.Instance.UnRegisterTcpClient(world);

            lock (m_sync)
            {
                if (Worlds.Count(entry => entry.Id == world.Id) > 0)
                {
                    Worlds.RemoveAll(entry => entry.Id == world.Id);

                    if (m_realmlist.ContainsKey(world.Id))
                    {
                        m_realmlist[world.Id].Connected = false;

                        m_realmlist[world.Id].Status = ServerStatusEnum.OFFLINE;

                        OnServerChange(m_realmlist[world.Id]);
                    }
                }
                logger.Info("Unregistered \"{0}\" <Id : {1}> <{2}>", world.Name, world.Id, world.Address);
            }
        }

        private static bool AskAddWorldRecord(WorldServerInformation worldServerInformation)
        {
            return
                AuthentificationServer.Instance.ConsoleInterface.AskForSomething(
                    string.Format("Server {0} request to be registered. Accept Request ?",
                                  worldServerInformation.Name, WorldServerTimeout), WorldServerTimeout);
        }

        /// <summary>
        ///   Check each second if the world servers are alive
        /// </summary>
        private static void CheckPing()
        {
            while (AuthentificationServer.Instance.Running)
            {
                for (int i = 0; i < Worlds.Count; i++)
                {
                    // check if the world server has pinged recently
                    if ((DateTime.Now - Worlds[i].LastPing).TotalMilliseconds > WorldServerTimeout * 1000)
                    {
                        // the world server is disconnected
                        logger.Warn("WorldServer \"{0}\" <id:{1}> has timed out.", Worlds[i].Name, Worlds[i].Id);
                        RemoveWorld(Worlds[i]);
                    }
                }

                Thread.Sleep(200);
            }
        }

        private static void OnServerListChange()
        {
            Parallel.ForEach(AuthentificationServer.Instance.GetClientsLookingOfServers(),
                             ConnectionHandler.SendServersListMessage);
        }

        private static void OnServerChange(WorldRecord world)
        {
            Parallel.ForEach(AuthentificationServer.Instance.GetClientsLookingOfServers(),
                client => ConnectionHandler.SendServerStatusUpdateMessage(client,world));
        }
    }
}