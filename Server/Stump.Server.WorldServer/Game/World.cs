using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Database.World.Maps;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;
using Stump.Server.WorldServer.Database.World.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.BaseServer.Logging;
using System.Globalization;
using MongoDB.Bson;

namespace Stump.Server.WorldServer.Game
{
    public class World : DataManager<World>
    {
        readonly Logger logger = LogManager.GetCurrentClassLogger();

        readonly ConcurrentDictionary<int, Character> m_charactersById =
            new ConcurrentDictionary<int, Character>(Environment.ProcessorCount, ClientManager.MaxConcurrentConnections);

        readonly ConcurrentDictionary<string, Character> m_charactersByName =
            new ConcurrentDictionary<string, Character>(Environment.ProcessorCount, ClientManager.MaxConcurrentConnections, StringComparer.OrdinalIgnoreCase);

        readonly ConcurrentDictionary<int, WorldAccount> m_connectedAccounts =
            new ConcurrentDictionary<int, WorldAccount>();

        Dictionary<int, Area> m_areas = new Dictionary<int, Area>();

        int m_characterCount;

        Dictionary<int, Map> m_maps = new Dictionary<int, Map>();
        Dictionary<int, SubArea> m_subAreas = new Dictionary<int, SubArea>();
        Dictionary<int, SuperArea> m_superAreas = new Dictionary<int, SuperArea>();
        Dictionary<int, WorldMapGraveyardRecord> m_graveyards = new Dictionary<int, WorldMapGraveyardRecord>();

        readonly object m_saveLock = new object();
        readonly ConcurrentBag<ISaveable> m_saveablesInstances = new ConcurrentBag<ISaveable>();

        public event Action<Character> CharacterJoined;

        void OnCharacterEntered(Character character)
        {
            CharacterJoined?.Invoke(character);
        }

        public event Action<Character> CharacterLeft;

        void OnCharacterLeft(Character character)
        {
            CharacterLeft?.Invoke(character);
        }

        public int CharacterCount
        {
            get { return m_characterCount; }
        }

        public object SaveLock
        {
            get { return m_saveLock; }
        }

        #region Initialization

        private bool m_spacesLoaded;
        private bool m_spacesSpawned;

        [Initialization(InitializationPass.Seventh)]
        public override void Initialize()
        {
            // maps
            LoadSpaces();
            SpawnSpaces();
        }

        public void Reload()
        {
            SpawnSpaces();
        }

        public void LoadSpaces()
        {
            if (m_spacesLoaded)
            {
                UnSetLinks();
            }

            m_spacesLoaded = true;

            logger.Info("Load maps...");
            m_maps = Database.Query<MapRecord, MapPositionRecord, MapRecord>
                (new MapRecordRelator().Map, MapRecordRelator.FetchQuery).ToDictionary(entry => entry.Id, entry => new Map(entry));

            logger.Info("Load sub areas...");
            m_subAreas = Database.Query<SubAreaRecord>(SubAreaRecordRelator.FetchQuery).ToDictionary(entry => entry.Id, entry => new SubArea(entry));

            logger.Info("Load areas...");
            m_areas = Database.Query<AreaRecord>(AreaRecordRelator.FetchQuery).ToDictionary(entry => entry.Id, entry => new Area(entry));

            logger.Info("Load super areas...");
            m_superAreas = Database.Query<SuperAreaRecord>(SuperAreaRecordRelator.FetchQuery).ToDictionary(entry => entry.Id, entry => new SuperArea(entry));

            logger.Info("Load graveyards...");
            m_graveyards = Database.Query<WorldMapGraveyardRecord>(WorldMapGraveyardRelator.FetchQuery).ToDictionary(entry => entry.Id, entry => entry);

            SetLinks();
        }

        public void SpawnSpaces()
        {
            if (m_spacesSpawned)
            {
                UnSpawnSpaces();
            }

            m_spacesSpawned = true;

            logger.Info("Spawn npcs ...");
            SpawnNpcs();

            logger.Info("Spawn interactives ...");
            SpawnInteractives();

            logger.Info("Spawn cell triggers ...");
            SpawnCellTriggers();

            logger.Info("Spawn monsters ...");
            SpawnMonsters();

            logger.Info("Spawn merchants ...");
            SpawnMerchants();
        }

        private void SetLinks()
        {
            foreach (var map in m_maps.Values.Where(map => map.Record.Position != null))
            {
                SubArea subArea;
                if (m_subAreas.TryGetValue(map.Record.Position.SubAreaId, out subArea))
                {
                    subArea.AddMap(map);
                }
            }

            foreach (var subArea in m_subAreas.Values)
            {
                Area area;
                if (m_areas.TryGetValue(subArea.Record.AreaId, out area))
                {
                    area.AddSubArea(subArea);
                }
            }

            foreach (var area in m_areas.Values)
            {
                SuperArea superArea;
                if (m_superAreas.TryGetValue(area.Record.SuperAreaId, out superArea))
                {
                    superArea.AddArea(area);
                }
            }
        }

        private void UnSetLinks()
        {
            foreach (var map in m_maps.Values.Where(map => map.Record.Position != null))
            {
                SubArea subArea;
                if (m_subAreas.TryGetValue(map.Record.Position.SubAreaId, out subArea))
                {
                    subArea.RemoveMap(map);
                }
            }

            foreach (var subArea in m_subAreas.Values)
            {
                Area area;
                if (m_areas.TryGetValue(subArea.Record.AreaId, out area))
                {
                    area.RemoveSubArea(subArea);
                }
            }

            foreach (var area in m_areas.Values)
            {
                SuperArea superArea;
                if (m_superAreas.TryGetValue(area.Record.SuperAreaId, out superArea))
                {
                    superArea.RemoveArea(area);
                }
            }
        }

        private static void SpawnNpcs()
        {
            foreach (var npcSpawn in NpcManager.Instance.GetNpcSpawns())
            {
                var position = npcSpawn.GetPosition();

                position.Map.SpawnNpc(npcSpawn);
            }
        }

        public void UnSpawnSpaces()
        {
            foreach (var map in m_maps.Values)
            {
                foreach (var interactive in map.GetInteractiveObjects().ToArray())
                {
                    map.UnSpawnInteractive(interactive);
                }

                foreach (var pool in map.SpawningPools.ToArray())
                {
                    map.RemoveSpawningPool(pool);
                }

                foreach (var trigger in map.GetTriggers().ToArray())
                {
                    map.RemoveTrigger(trigger);
                }
            }

            foreach (var subArea in m_subAreas)
            {
                foreach (var monsterSpawn in subArea.Value.MonsterSpawns.ToArray())
                {
                    subArea.Value.RemoveMonsterSpawn(monsterSpawn);
                }
            }
        }

        public void SpawnInteractives()
        {
            foreach (var interactive in InteractiveManager.Instance.GetInteractiveSpawns())
            {
                var map = interactive.GetMap();

                if (map == null)
                {
                    logger.Error("Cannot spawn interactive id={0} : map {1} doesn't exist", interactive.Id, interactive.MapId);
                    continue;
                }

                map.SpawnInteractive(interactive);
            }

            foreach (var map in m_maps)
                map.Value.UpdateAvailableJobs();
        }

        public void UnSpawnInteractives()
        {
            foreach (var map in m_maps.Values)
            {
                var interactives = map.GetInteractiveObjects().ToArray();

                foreach (var interactive in interactives)
                {
                    map.UnSpawnInteractive(interactive);
                }
            }
        }

        public void SpawnCellTriggers()
        {
            foreach (var trigger in CellTriggerManager.Instance.GetCellTriggers().Select(cellTrigger => cellTrigger.GenerateTrigger()))
            {
                trigger.Position.Map.AddTrigger(trigger);
            }
        }

        public void UnSpawnCellTriggers()
        {
            foreach (var map in m_maps.Values)
            {
                var triggers = map.GetTriggers().ToArray();

                foreach (var trigger in triggers)
                {
                    map.RemoveTrigger(trigger);
                }
            }
        }

        private void SpawnMonsters()
        {
            foreach (var spawn in MonsterManager.Instance.GetMonsterSpawns())
            {
                if (spawn.IsDisabled)
                    continue;

                if (spawn.Map != null)
                {
                    spawn.Map.AddMonsterSpawn(spawn);
                }
                else if (spawn.SubArea != null)
                {
                    spawn.SubArea.AddMonsterSpawn(spawn);
                }
            }

            foreach (var spawn in MonsterManager.Instance.GetMonsterDungeonsSpawns().Where(spawn => spawn.Map != null))
            {
                spawn.Map.AddMonsterDungeonSpawn(spawn);
            }

            foreach (var spawn in MonsterManager.Instance.GetMonsterStaticSpawns().Where(spawn => spawn.Map != null))
            {
                spawn.Map.AddMonsterStaticSpawn(spawn);
            }

            foreach (var map in m_maps.Where(map => map.Value.MonsterSpawnsCount > 0))
            {
                map.Value.EnableClassicalMonsterSpawns();
            }
        }

        static void SpawnMerchants()
        {
            foreach (var merchant in from spawn in MerchantManager.Instance.GetMerchantSpawns() where spawn.Map != null select new Merchant(spawn))
            {
                merchant.LoadRecord();
                MerchantManager.Instance.ActiveMerchant(merchant);
                merchant.Map.Enter(merchant);
            }
        }

        void UnSpawnMerchants()
        {
            foreach (var merchant in MerchantManager.Instance.Merchants)
            {
                MerchantManager.Instance.UnActiveMerchant(merchant);
                merchant.Map.Leave(merchant);
            }
        }

        public void SpawnTaxCollectors()
        {
            foreach (var taxcollector in from spawn in TaxCollectorManager.Instance.GetTaxCollectorSpawns()
                                         where spawn.Map != null
                                         select new TaxCollectorNpc(spawn, spawn.Map.GetNextContextualId()))
            {
                taxcollector.Guild.AddTaxCollector(taxcollector);
                taxcollector.Map.Enter(taxcollector);
            }
        }
        #endregion

        #region Maps

        public Map GetMap(int id)
        {
            Map map;
            m_maps.TryGetValue(id, out map);

            return map;
        }

        public Map GetMap(int x, int y, bool outdoor = true)
        {
            return m_maps.Values.FirstOrDefault(entry => entry.Position.X == x && entry.Position.Y == y && entry.Outdoor == outdoor);
        }

        public IEnumerable<Map> GetMaps()
        {
            return m_maps.Values;
        }

        public Map[] GetMaps(int x, int y)
        {
            return m_maps.Values.Where(entry => entry.Position.X == x && entry.Position.Y == y).ToArray();
        }

        public Map[] GetMaps(int x, int y, bool outdoor)
        {
            return m_maps.Values.Where(entry => entry.Position.X == x && entry.Position.Y == y && entry.Outdoor == outdoor).ToArray();
        }

        public Map[] GetMaps(Map reference, int x, int y)
        {
            var maps = reference.SubArea.GetMaps(x, y);
            if (maps.Length > 0)
                return maps;

            maps = reference.Area.GetMaps(x, y);
            if (maps.Length > 0)
                return maps;

            maps = reference.SuperArea.GetMaps(x, y);
            return maps.Length > 0 ? maps : new Map[0];
        }

        public Map[] GetMaps(Map reference, int x, int y, bool outdoor)
        {
            var maps = reference.SubArea.GetMaps(x, y, outdoor);
            if (maps.Length > 0)
                return maps;

            maps = reference.Area.GetMaps(x, y, outdoor);
            if (maps.Length > 0)
                return maps;

            maps = reference.SuperArea.GetMaps(x, y, outdoor);
            return maps.Length > 0 ? maps : new Map[0];
        }

        public SubArea GetSubArea(int id)
        {
            SubArea subArea;
            m_subAreas.TryGetValue(id, out subArea);

            return subArea;
        }

        public SubArea GetSubArea(string name) => m_subAreas.Values.FirstOrDefault(entry => entry.Record.Name == name);

        public Area GetArea(int id)
        {
            Area area;
            m_areas.TryGetValue(id, out area);

            return area;
        }

        public Area GetArea(string name) => m_areas.Values.FirstOrDefault(entry => entry.Name == name);

        public SuperArea GetSuperArea(int id)
        {
            SuperArea superArea;
            m_superAreas.TryGetValue(id, out superArea);

            return superArea;
        }
        public SuperArea GetSuperArea(string name) => m_superAreas.Values.FirstOrDefault(entry => entry.Name == name);

        public IEnumerable<WorldMapGraveyardRecord> GetGraveyards() => m_graveyards.Values.ToArray();

        public WorldMapGraveyardRecord GetNearestGraveyard(Map currentMap)
        {
            var actualPoint = new MapPoint(currentMap.Position);
            return GetGraveyards().OrderBy(x => actualPoint.EuclideanDistanceTo(new MapPoint(x.Map.Position)))
                .OrderByDescending(x => x.Map.Area.Id == currentMap.Area.Id)
                .OrderByDescending(x => x.SubAreaId == currentMap.SubArea.Id).FirstOrDefault();
        }

        #endregion

        #region Actors

        public void Enter(Character character)
        {
            // note : to delete
            if (m_charactersById.ContainsKey(character.Id))
                Leave(character);

            if (m_charactersById.TryAdd(character.Id, character) && m_charactersByName.TryAdd(character.Name, character))
            {
                Interlocked.Increment(ref m_characterCount);
                OnCharacterEntered(character);
            }
            else
                logger.Error("Cannot add character {0} to the World", character);

            if (!m_connectedAccounts.ContainsKey(character.Account.Id))
                m_connectedAccounts.TryAdd(character.Account.Id, character.Client.WorldAccount);
        }

        public void Leave(Character character)
        {
            Character dummy;
            if (m_charactersById.TryRemove(character.Id, out dummy) && m_charactersByName.TryRemove(character.Name, out dummy))
            {
                Interlocked.Decrement(ref m_characterCount);
                OnCharacterLeft(character);
            }
            else
                logger.Error("Cannot remove character {0} from the World", character);

            WorldAccount dAccount;
            m_connectedAccounts.TryRemove(character.Account.Id, out dAccount);
        }

        public bool IsConnected(int id)
        {
            return m_charactersById.ContainsKey(id);
        }

        public bool IsConnected(string name)
        {
            return m_charactersByName.ContainsKey(name);
        }

        public bool IsAccountConnected(int id)
        {
            return m_connectedAccounts.ContainsKey(id);
        }

        public WorldAccount GetConnectedAccount(int id)
        {
            WorldAccount account;
            return m_connectedAccounts.TryGetValue(id, out account) ? account : null;
        }

        public Character GetCharacter(int id)
        {
            Character character;
            return m_charactersById.TryGetValue(id, out character) ? character : null;
        }

        public Character GetCharacter(string name)
        {
            Character character;
            return m_charactersByName.TryGetValue(name, out character) ? character : null;
        }

        public Character GetCharacter(Predicate<Character> predicate)
        {
            return m_charactersById.FirstOrDefault(k => predicate(k.Value)).Value;
        }

        public IEnumerable<Character> GetCharacters(Predicate<Character> predicate)
        {
            return m_charactersById.Values.Where(k => predicate(k));
        }

        /// <summary>
        /// Get a spell by a search pattern. *account = current spell used by account, name = spell by his name.
        /// </summary>
        /// <returns></returns>
        public Character GetCharacterByPattern(string pattern)
        {
            if (pattern[0] != '*')
                return GetCharacter(pattern);

            var name = pattern.Remove(0, 1);


            return ClientManager.Instance.FindAll<WorldClient>(entry => entry.Account.Login == name).
                Select(entry => entry.Character).SingleOrDefault();
        }
        public IEnumerable<Character> GetCharacters()
        {
            return m_charactersById.Values;
        }

        /// <summary>
        /// Get a spell by a search pattern. * = caller, *account = current spell used by account, name = spell by his name.
        /// </summary>
        /// <returns></returns>
        public Character GetCharacterByPattern(Character caller, string pattern)
        {
            return pattern == "*" ? caller : GetCharacterByPattern(pattern);
        }

        public void ForEachCharacter(Action<Character> action)
        {
            foreach (var key in m_charactersById)
                action(key.Value);
        }

        public void ForEachCharacter(Predicate<Character> predicate, Action<Character> action)
        {
            foreach (var key in m_charactersById.Where(k => predicate(k.Value)))
                action(key.Value);
        }

        public void SendAnnounce(string announce)
        {
            ForEachCharacter(character => character.SendServerMessage(announce));
        }

        public void SendAnnounce(string announce, Color color)
        {
            ForEachCharacter(character => character.SendServerMessage(announce, color));
        }

        public void SendAnnounce(TextInformationTypeEnum type, short messageId, params object[] parameters)
        {
            ForEachCharacter(character => character.SendInformationMessage(type, messageId, parameters));
        }

        #endregion

        public void RegisterSaveableInstance(ISaveable instance)
        {
            if (!m_saveablesInstances.Contains(instance))
                m_saveablesInstances.Add(instance);
        }

        public void Save()
        {
            lock (SaveLock)
            {
                logger.Info("Saving world ...");
                if (WorldServer.SaveMessage)
                    SendAnnounce(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 164);

                var sw = Stopwatch.StartNew();

                var clients = ClientManager.Instance.FindAll<WorldClient>();

                foreach (var client in clients)
                {
                    try
                    {
                        if (client.Character != null)
                        {
                            client.Character.SaveNow();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Cannot save {0} : {1}", client, ex);
                    }
                }

                foreach (var instance in m_saveablesInstances)
                {
                    try
                    {
                        instance.Save();
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Cannot save {0} : {1}", instance, ex);
                    }
                }

                if (WorldServer.SaveMessage)
                    SendAnnounce(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 165);

                var document = new BsonDocument
                    {
                        { "Id", WorldServer.ServerInformation.Id },
                        { "Name", WorldServer.ServerInformation.Name },
                        { "Players", WorldServer.Instance.ClientManager.Count },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

                MongoLogger.Instance.Insert("WorldStats", document);

                logger.Info("World server saved ! ({0} ms)", sw.ElapsedMilliseconds);
            }
        }

        public void Stop(bool wait = false)
        {
            foreach (var area in m_areas)
            {
                area.Value.Stop(wait);
            }
        }

        private readonly List<Area> m_pausedAreas = new List<Area>();
        /// <summary>
        /// Has to be called from another thread !!
        /// </summary>
        public void Pause()
        {
            logger.Info("World Paused !!");
            foreach (var area in m_areas.Where(x => x.Value.IsRunning))
            {
                if (area.Value.IsInContext)
                    throw new Exception("Has to be called from another thread !!");

                area.Value.Stop(true);
                m_pausedAreas.Add(area.Value);
            }

            if (WorldServer.Instance.IOTaskPool.IsInContext)
                throw new Exception("Has to be called from another thread !!");

            WorldServer.Instance.IOTaskPool.Stop(true);
        }

        public void Resume()
        {
            logger.Info("World Resumed");
            foreach (var pausedArea in m_pausedAreas)
            {
                pausedArea.Start();
            }

            m_pausedAreas.Clear();


            WorldServer.Instance.IOTaskPool.Start();
        }
    }
}