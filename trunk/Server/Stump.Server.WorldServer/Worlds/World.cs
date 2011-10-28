using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NLog;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Database.World.Triggers;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Worlds.Interactives;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells.Triggers;

namespace Stump.Server.WorldServer.Worlds
{
    public class World : Singleton<World>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ConcurrentDictionary<int, Character> m_charactersById =
            new ConcurrentDictionary<int, Character>(WorkerManager.WorkerThreadNumber, ClientManager.MaxConcurrentConnections);

        private readonly ConcurrentDictionary<string, Character> m_charactersByName =
            new ConcurrentDictionary<string, Character>(WorkerManager.WorkerThreadNumber, ClientManager.MaxConcurrentConnections);

        private int m_characterCount;

        private Dictionary<int, Map> m_maps = new Dictionary<int, Map>();
        private Dictionary<int, SubArea> m_subAreas = new Dictionary<int, SubArea>();
        private Dictionary<int, Area> m_areas = new Dictionary<int, Area>();
        private Dictionary<int, SuperArea> m_superAreas = new Dictionary<int, SuperArea>();

        public int CharacterCount
        {
            get { return m_characterCount; }
        }

        #region Initialization

        [Initialization(InitializationPass.Seventh)]
        public void Initialize()
        {
            // maps
            LoadSpaces();
            SetLinks();
        }

        private void LoadSpaces()
        {
            logger.Info("Load maps...");
            m_maps = MapRecord.FindAll().ToDictionary(entry => entry.Id, entry => new Map(entry));

            logger.Info("Load sub areas...");
            m_subAreas = SubAreaRecord.FindAll().ToDictionary(entry => entry.Id, entry => new SubArea(entry));

            logger.Info("Load areas...");
            m_areas = AreaRecord.FindAll().ToDictionary(entry => entry.Id, entry => new Area(entry));

            logger.Info("Load super areas...");
            m_superAreas = SuperAreaRecord.FindAll().ToDictionary(entry => entry.Id, entry => new SuperArea(entry));

            logger.Info("Spawn npcs ...");
            SpawnNpcs();

            logger.Info("Spawn interactives ...");
            SpawnInteractives();

            logger.Info("Spawn cell triggers ...");
            SpawnCellTriggers();
        }

        private void SetLinks()
        {
            foreach (var map in m_maps.Values)
            {
                if (map.Record.Position == null)
                    continue;

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

        private void SpawnNpcs()
        {
            foreach (var npcSpawn in NpcManager.Instance.GetNpcSpawns())
            {
                var position = npcSpawn.GetPosition();

                position.Map.SpawnNpc(npcSpawn.Template, position, npcSpawn.Look);
            }
        }

        private void SpawnInteractives()
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
        }

        private void SpawnCellTriggers()
        {
            foreach (var cellTrigger in CellTriggerManager.Instance.GetCellTriggers())
            {
                var trigger = cellTrigger.GenerateTrigger();

                trigger.Position.Map.AddTrigger(trigger);
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
            return m_maps.Values.Where(entry => entry.Position.X == x && entry.Position.Y == y && entry.Outdoor == outdoor).FirstOrDefault();
        }

        public IEnumerable<Map> GetMaps(int x, int y)
        {
            return m_maps.Values.Where(entry => entry.Position.X == x && entry.Position.Y == y);
        }

        public SubArea GetSubArea(int id)
        {
            SubArea subArea;
            m_subAreas.TryGetValue(id, out subArea);

            return subArea;
        }

        public Area GetArea(int id)
        {
            Area area;
            m_areas.TryGetValue(id, out area);

            return area;
        }

        public SuperArea GetSuperArea(int id)
        {
            SuperArea superArea;
            m_superAreas.TryGetValue(id, out superArea);

            return superArea;
        }

        #endregion

        #region Actors
        public void Enter(Character character)
        {
            if (m_charactersById.TryAdd(character.Id, character) && m_charactersByName.TryAdd(character.Name, character))
                Interlocked.Increment(ref m_characterCount);
            else
                throw new Exception(string.Format("Cannot add character {0} to the World", character));
        }

        public void Leave(Character character)
        {
            Character dummy;
            if (m_charactersById.TryRemove(character.Id, out dummy) && m_charactersByName.TryRemove(character.Name, out dummy))
                Interlocked.Decrement(ref m_characterCount);
            else
                throw new Exception(string.Format("Cannot remove character {0} to the World", character));
        }

        public bool IsConnected(int id)
        {
            return m_charactersById.ContainsKey(id);
        }

        public bool IsConnected(string name)
        {
            return m_charactersByName.ContainsKey(name);
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
        /// Get a character by a search pattern. *account = current character used by account, name = character by his name.
        /// </summary>
        /// <returns></returns>
        public Character GetCharacterByPattern(string pattern)
        {
            if (pattern[0] == '*')
            {
                string name = pattern.Remove(0, 1);



                return ClientManager.Instance.FindAll<WorldClient>(entry => entry.Account.Login == name).
                    Select(entry => entry.ActiveCharacter).SingleOrDefault();
            }

            return GetCharacter(pattern);
        }

        /// <summary>
        /// Get a character by a search pattern. * = caller, *(account) = current character used by account, name = character by his name.
        /// </summary>
        /// <returns></returns>
        public Character GetCharacterByPattern(Character caller, string pattern)
        {
            if (pattern == "*")
                return caller;

            return GetCharacterByPattern(pattern);
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
        #endregion

        public void Save()
        {
            foreach (var character in m_charactersById.Values)
            {
                character.SaveNow();
            }
        }
    }
}