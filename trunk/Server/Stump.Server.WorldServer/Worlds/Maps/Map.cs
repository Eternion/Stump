using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Interactives;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Interactives;
using Stump.Server.WorldServer.Worlds.Interactives.Skills;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Cells.Triggers;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Worlds.Maps
{
    public class Map : IContext
    {
        #region Events

        public event Action<Map, RolePlayActor> ActorEnter;

        private void NotifyActorEnter(RolePlayActor actor)
        {
            OnEnter(actor);

            Action<Map, RolePlayActor> handler = ActorEnter;
            if (handler != null)
                handler(this, actor);
        }

        public event Action<Map, RolePlayActor> ActorLeave;

        private void NotifyActorLeave(RolePlayActor actor)
        {
            OnLeave(actor);

            Action<Map, RolePlayActor> handler = ActorLeave;
            if (handler != null)
                handler(this, actor);
        }

        public event Action<Map, Fight> FightCreated;

        private void NotifyFightCreated(Fight fight)
        {
            Action<Map, Fight> handler = FightCreated;
            if (handler != null)
                handler(this, fight);
        }

        public event Action<Map, Fight> FightRemoved;

        private void NotifyFightRemoved(Fight fight)
        {
            Action<Map, Fight> handler = FightRemoved;
            if (handler != null)
                handler(this, fight);
        }

        public event Action<Map, InteractiveObject> InteractiveSpawned;

        private void NotifyInteractiveSpawned(InteractiveObject interactive)
        {
            OnInteractiveSpawned(interactive);

            Action<Map, InteractiveObject> handler = InteractiveSpawned;
            if (handler != null)
                handler(this, interactive);
        }

        public event Action<Map, InteractiveObject> InteractiveUnSpawned;

        private void NotifyInteractiveUnSpawned(InteractiveObject interactive)
        {
            OnInteractiveUnSpawned(interactive);

            Action<Map, InteractiveObject> handler = InteractiveUnSpawned;
            if (handler != null)
                handler(this, interactive);
        }

        public event Action<Map, Character, InteractiveObject, Skill> InteractiveUsed;

        private void NotifyInteractiveUsed(Character user, InteractiveObject interactive, Skill skill)
        {
            OnInteractiveUsed(user, interactive, skill);

            Action<Map, Character, InteractiveObject, Skill> handler = InteractiveUsed;
            if (handler != null)
                handler(this, user, interactive, skill);
        }

        #endregion

        #region Constructors

        static Map()
        {
            PointsGrid = new MapPoint[MapPoint.MapSize];

            for (short i = 0; i < MapPoint.MapSize; i++)
            {
                // i is a cell
                PointsGrid[i] = new MapPoint(i);
            }
        }

        public Map(MapRecord record)
        {
            Record = record;

            InitializeValidators();
            InitializeMapArrounds();
        }

        private void InitializeMapArrounds()
        {
            m_mapsAround.Add(TopNeighbourId, MapNeighbour.Top);
            m_mapsAround.Add(BottomNeighbourId, MapNeighbour.Bottom);
            m_mapsAround.Add(LeftNeighbourId, MapNeighbour.Left);
            m_mapsAround.Add(RightNeighbourId, MapNeighbour.Right);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Array that associate a cell to a map point
        /// </summary>
        public static MapPoint[] PointsGrid;

        private readonly ConcurrentDictionary<int, RolePlayActor> m_actors = new ConcurrentDictionary<int, RolePlayActor>();
        private readonly ReversedUniqueIdProvider m_contextualIds = new ReversedUniqueIdProvider(0);
        private readonly List<Fight> m_fights = new List<Fight>();
        private readonly Dictionary<int, InteractiveObject> m_interactives = new Dictionary<int, InteractiveObject>();
        private readonly Dictionary<int, MapNeighbour> m_mapsAround = new Dictionary<int, MapNeighbour>();
        private readonly Dictionary<Cell, List<CellTrigger>> m_cellsTriggers = new Dictionary<Cell, List<CellTrigger>>();

        protected internal MapRecord Record;
        private Map m_bottomNeighbour;
        private Map m_leftNeighbour;
        private Map m_rightNeighbour;
        private Map m_topNeighbour;

        public int Id
        {
            get { return Record.Id; }
        }

        public Cell[] Cells
        {
            get { return Record.Cells; }
        }

        public SubArea SubArea
        {
            get;
            internal set;
        }

        public Area Area
        {
            get { return SubArea.Area; }
        }

        public SuperArea SuperArea
        {
            get { return Area.SuperArea; }
        }

        public uint RelativeId
        {
            get { return Record.RelativeId; }
        }

        public int MapType
        {
            get { return Record.MapType; }
        }

        public Point Position
        {
            get { return Record.Position.Pos; }
        }

        public bool Outdoor
        {
            get { return Record.Outdoor; }
        }

        public int TopNeighbourId
        {
            get { return Record.TopNeighbourId; }
            set { Record.TopNeighbourId = value; }
        }

        public Map TopNeighbour
        {
            get { return m_topNeighbour ?? (m_topNeighbour = World.Instance.GetMap(TopNeighbourId)); }
        }

        public int BottomNeighbourId
        {
            get { return Record.BottomNeighbourId; }
            set { Record.BottomNeighbourId = value; }
        }

        public Map BottomNeighbour
        {
            get { return m_bottomNeighbour ?? (m_bottomNeighbour = World.Instance.GetMap(BottomNeighbourId)); }
        }

        public int LeftNeighbourId
        {
            get { return Record.LeftNeighbourId; }
            set { Record.LeftNeighbourId = value; }
        }

        public Map LeftNeighbour
        {
            get { return m_leftNeighbour ?? (m_leftNeighbour = World.Instance.GetMap(LeftNeighbourId)); }
        }

        public int RightNeighbourId
        {
            get { return Record.RightNeighbourId; }
            set { Record.RightNeighbourId = value; }
        }

        public Map RightNeighbour
        {
            get { return m_rightNeighbour ?? (m_rightNeighbour = World.Instance.GetMap(RightNeighbourId)); }
        }

        public int ShadowBonusOnEntities
        {
            get { return Record.ShadowBonusOnEntities; }
            set { Record.ShadowBonusOnEntities = value; }
        }

        public bool UseLowpassFilter
        {
            get { return Record.UseLowpassFilter; }
            set { Record.UseLowpassFilter = value; }
        }

        public bool UseReverb
        {
            get { return Record.UseReverb; }
            set { Record.UseReverb = value; }
        }

        public int PresetId
        {
            get { return Record.PresetId; }
        }

        #endregion

        #region Npcs

        public Npc SpawnNpc(NpcTemplate template, ObjectPosition position, EntityLook look)
        {
            if (position.Map != this)
                throw new Exception("Try to spawn a npc on the wrong map");

            sbyte id = GetNextContextualId();

            var npc = new Npc(id, template, position, look);

            Enter(npc);

            return npc;
        }

        public bool UnSpawnNpc(sbyte id)
        {
            var npc = GetActor<Npc>(id);

            if (npc == null)
                return false;

            Leave(npc);

            return true;
        }

        public void UnSpawnNpc(Npc npc)
        {
            if (GetActor<Npc>(npc.Id) != npc)
                throw new Exception(string.Format("Npc with id {0} not found, cannot unspawn an unexistant npc", npc.Id));

            Leave(npc);
        }

        #endregion

        #region Interactives

        public InteractiveObject SpawnInteractive(InteractiveSpawn spawn)
        {
            var interactiveObject = new InteractiveObject(spawn);

            m_interactives.Add(interactiveObject.Id, interactiveObject);

            NotifyInteractiveSpawned(interactiveObject);

            return interactiveObject;
        }

        private void OnInteractiveSpawned(InteractiveObject interactive)
        {
        }

        public void UnSpawnInteractive(InteractiveObject interactive)
        {
            m_interactives.Remove(interactive.Id);

            NotifyInteractiveUnSpawned(interactive);
        }

        private void OnInteractiveUnSpawned(InteractiveObject interactive)
        {
        }

        public void UseInteractiveObject(Character character, int interactiveId, int skillId)
        {
            InteractiveObject interactiveObject = GetInteractiveObject(interactiveId);
            Skill skill = interactiveObject.GetSkill(skillId);

            if (skill.IsEnabled(character))
            {
                skill.Execute(character);

                NotifyInteractiveUsed(character, interactiveObject, skill);
            }
        }

        private void OnInteractiveUsed(Character user, InteractiveObject interactiveObject, Skill skill)
        {
            ForEach(character => InteractiveHandler.SendInteractiveUsedMessage(character.Client, user, interactiveObject, skill));
        }

        #endregion

        #region Monsters

        public MonsterGroup SpawnMonsterGroup(MonsterGrade monster, ObjectPosition position)
        {
            if (position.Map != this)
                throw new Exception("Try to spawn a monster group on the wrong map");
            sbyte id = GetNextContextualId();

            var group = new MonsterGroup(id, position);

            group.AddMonster(new Monster(monster, group));

            Enter(group);

            return group;
        }

        public MonsterGroup SpawnMonsterGroup(IEnumerable<MonsterGrade> monsters, ObjectPosition position)
        {
            if (position.Map != this)
                throw new Exception("Try to spawn a monster group on the wrong map");
            sbyte id = GetNextContextualId();

            var group = new MonsterGroup(id, position);

            foreach (MonsterGrade grade in monsters)
                group.AddMonster(new Monster(grade, group));

            Enter(group);

            return group;
        }

        public bool UnSpawnMonsterGroup(sbyte id)
        {
            var group = GetActor<MonsterGroup>(id);

            if (group == null)
                return false;

            Leave(group);

            return true;
        }

        #endregion

        #region Triggers

        public void AddTrigger(CellTrigger trigger)
        {
            if (!m_cellsTriggers.ContainsKey(trigger.Position.Cell))
                m_cellsTriggers.Add(trigger.Position.Cell, new List<CellTrigger>());

            m_cellsTriggers[trigger.Position.Cell].Add(trigger);
        }

        public void RemoveTrigger(CellTrigger trigger)
        {
           if (!m_cellsTriggers.ContainsKey(trigger.Position.Cell))
                return;

           m_cellsTriggers[trigger.Position.Cell].Remove(trigger);
        }

        public void RemoveTriggers(Cell cell)
        {
            if (!m_cellsTriggers.ContainsKey(cell))
                return;

            m_cellsTriggers[cell].Clear();

        }

        public IEnumerable<CellTrigger> GetTriggers(Cell cell)
        {
            if (!m_cellsTriggers.ContainsKey(cell))
                return Enumerable.Empty<CellTrigger>();

            return m_cellsTriggers[cell];
        }

        public bool ExecuteTrigger(CellTriggerType triggerType, Cell cell, Character character)
        {
            bool applied = false;

            foreach (var trigger in GetTriggers(cell))
            {
                if (trigger.TriggerType == triggerType)
                {
                    trigger.Apply(character);
                    applied = true;
                }
            }

            return applied;
        }

        #endregion

        #region Fights

        public void AddFight(Fight fight)
        {
            if (fight.Map != this)
                return;

            m_fights.Add(fight);

            ForEach(character => ContextRoleplayHandler.SendMapFightCountMessage(character.Client,
                                                                                 (short) m_fights.Count));

            NotifyFightCreated(fight);
        }

        public void RemoveFight(Fight fight)
        {
            m_fights.Remove(fight);

            ForEach(character => ContextRoleplayHandler.SendMapFightCountMessage(character.Client,
                                                                                 (short) m_fights.Count));

            NotifyFightRemoved(fight);
        }

        #endregion

        #region Enter/Leave

        public void Enter(RolePlayActor actor)
        {
            if (m_actors.TryAdd(actor.Id, actor))
            {
                NotifyActorEnter(actor);
            }
            else
                throw new Exception(string.Format("Could not add actor '{0}' to the map", actor));
        }

        public void Leave(RolePlayActor actor)
        {
            if (m_actors.TryRemove(actor.Id, out actor))
            {
                NotifyActorLeave(actor);
            }
            else
                throw new Exception(string.Format("Could not remove actor '{0}' of the map", actor));
        }

        public void Refresh(RolePlayActor actor)
        {
            if (IsActor(actor))
                ForEach(entry => ContextRoleplayHandler.SendGameRolePlayShowActorMessage(entry.Client, actor));
        }

        private void OnEnter(RolePlayActor actor)
        {
            actor.StartMoving += OnActorStartMoving;
            actor.StopMoving += OnActorStopMoving;

            ForEach(entry => ContextRoleplayHandler.SendGameRolePlayShowActorMessage(entry.Client, actor));

            var character = actor as Character;

            if (character == null)
                return;

            ContextRoleplayHandler.SendCurrentMapMessage(character.Client, Id);

            if (m_fights.Count > 0)
                ContextRoleplayHandler.SendMapFightCountMessage(character.Client, (short) m_fights.Count);

            SendActorsActions(character);
            BasicHandler.SendBasicTimeMessage(character.Client);
        }

        private void SendActorsActions(Character character)
        {
            foreach (RolePlayActor actor in m_actors.Values)
            {
                if (actor.IsMoving())
                {
                    var moveKeys = actor.MovementPath.GetServerPathKeys();
                    RolePlayActor actorMoving = actor;

                    ContextHandler.SendGameMapMovementMessage(character.Client, moveKeys, actorMoving);
                    BasicHandler.SendBasicNoOperationMessage(character.Client);
                }
            }
        }

        private void OnLeave(RolePlayActor actor)
        {
            actor.StartMoving -= OnActorStartMoving;
            actor.StopMoving -= OnActorStopMoving;

            ForEach(entry => ContextHandler.SendGameContextRemoveElementMessage(entry.Client, actor));

            if (actor is MonsterGroup || actor is Npc)
                FreeContextualId((sbyte) actor.Id);
        }

        #endregion

        #region Actor Actions

        private void OnActorStartMoving(ContextActor actor, Path path)
        {
            var movementsKey = path.GetServerPathKeys();

            ForEach(delegate(Character entry)
                        {
                            ContextHandler.SendGameMapMovementMessage(entry.Client, movementsKey, actor);
                            BasicHandler.SendBasicNoOperationMessage(entry.Client);
                        });
        }

        private void OnActorStopMoving(ContextActor actor, Path path, bool canceled)
        {
            if (!(actor is Character))
                return;

            var character = actor as Character;

            if (ExecuteTrigger(CellTriggerType.END_MOVE_ON, actor.Cell, character))
                return;

            var monster = GetActor<MonsterGroup>(entry => entry.Cell.Id == character.Cell.Id);

            if (monster != null)
                monster.FightWith(character);
        }

        #endregion

        #region Gets

        public IEnumerable<Character> GetAllCharacters()
        {
            return GetActors<Character>();
        }

        public void ForEach(Action<Character> action)
        {
            foreach (Character character in GetAllCharacters())
            {
                action(character);
            }
        }

        public sbyte GetNextContextualId()
        {
            return (sbyte) m_contextualIds.Pop();
        }

        public void FreeContextualId(sbyte id)
        {
            m_contextualIds.Push(id);
        }

        public bool IsActor(int id)
        {
            return m_actors.ContainsKey(id);
        }

        public bool IsActor(RolePlayActor actor)
        {
            return IsActor(actor.Id);
        }

        public T GetActor<T>(int id)
            where T : RolePlayActor
        {
            RolePlayActor actor;
            if (m_actors.TryGetValue(id, out actor))
                return actor as T;

            return null;
        }

        public T GetActor<T>(Predicate<T> predicate)
            where T : RolePlayActor
        {
            return m_actors.Values.OfType<T>().Where(entry => predicate(entry)).SingleOrDefault();
        }

        public IEnumerable<T> GetActors<T>()
        {
            return m_actors.Values.OfType<T>(); // after a benchmark we conclued that it takes approximatively 10 ticks
        }

        public IEnumerable<T> GetActors<T>(Predicate<T> predicate)
        {
            return m_actors.Values.OfType<T>();
        }

        public InteractiveObject GetInteractiveObject(int id)
        {
            return m_interactives[id];
        }

        #region Neighbors

        public Map GetNeighbouringMap(MapNeighbour mapNeighbour)
        {
            switch (mapNeighbour)
            {
                case MapNeighbour.Top:
                    return TopNeighbour;
                case MapNeighbour.Bottom:
                    return BottomNeighbour;
                case MapNeighbour.Right:
                    return RightNeighbour;
                case MapNeighbour.Left:
                    return LeftNeighbour;
                default:
                    throw new ArgumentException("mapNeighbour");
            }
        }

        public MapNeighbour GetMapRelativePosition(int mapid)
        {
            return !m_mapsAround.ContainsKey(mapid) ? MapNeighbour.None : m_mapsAround[mapid];
        }

        /// <summary>
        ///   Calculate which cell our character will walk on once map changed. Returns 0 if not found
        /// </summary>
        public short GetCellAfterChangeMap(short currentCell, MapNeighbour mapneighbour)
        {
            switch (mapneighbour)
            {
                case MapNeighbour.Top:
                    return (short) (currentCell + 532);
                case MapNeighbour.Bottom:
                    return (short) (currentCell - 532);
                case MapNeighbour.Right:
                    return (short) (currentCell - 13);
                case MapNeighbour.Left:
                    return (short) (currentCell + 13);
                default:
                    return 0;
            }
        }

        #endregion

        #endregion

        public bool Equals(Map other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Map)) return false;
            return Equals((Map) obj);
        }

        public override int GetHashCode()
        {
            return (Record != null ? Record.GetHashCode() : 0);
        }

        public static bool operator ==(Map left, Map right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Map left, Map right)
        {
            return !Equals(left, right);
        }

        #region Network

        private void InitializeValidators()
        {
            // for later
        }

        #region MapComplementaryInformationsDataMessage

        public MapComplementaryInformationsDataMessage GetMapComplementaryInformationsDataMessage(Character character)
        {
            return new MapComplementaryInformationsDataMessage(
                (short) SubArea.Id,
                Id,
                0,
                new HouseInformations[0],
                m_actors.Select(entry => entry.Value.GetGameContextActorInformations() as GameRolePlayActorInformations),
                m_interactives.Select(entry => entry.Value.GetInteractiveElement(character)),
                new StatedElement[0],
                new MapObstacle[0],
                m_fights.Where(entry => entry.BladesVisible).Select(entry => entry.GetFightCommonInformations()));
        }

        #endregion

        #endregion
    }

    public class MapCellsInformationProvider : ICellsInformationProvider
    {
        public MapCellsInformationProvider(Map map)
        {
            Map = map;
        }

        public Map Map
        {
            get;
            private set;
        }

        public bool IsCellWalkable(short cell)
        {
            return Map.Cells[cell].Walkable && !Map.Cells[cell].NonWalkableDuringRP;
        }

        public CellInformation GetCellInformation(short cell)
        {
            return new CellInformation(Map.Cells[cell], IsCellWalkable(cell));
        }
    }
}