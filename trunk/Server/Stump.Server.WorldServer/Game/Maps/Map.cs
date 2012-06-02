using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using NLog;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Database.World.Maps;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Maps.Spawns;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Interactives;

namespace Stump.Server.WorldServer.Game.Maps
{
    public class Map : ICharacterContainer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Events

        public event Action<Map, RolePlayActor> ActorEnter;

        protected virtual void OnActorEnter(RolePlayActor actor)
        {
            OnEnter(actor);

            Action<Map, RolePlayActor> handler = ActorEnter;
            if (handler != null)
                handler(this, actor);
        }

        public event Action<Map, RolePlayActor> ActorLeave;

        protected virtual void OnActorLeave(RolePlayActor actor)
        {
            OnLeave(actor);

            Action<Map, RolePlayActor> handler = ActorLeave;
            if (handler != null)
                handler(this, actor);
        }

        public event Action<Map, Fight> FightCreated;

        protected virtual void OnFightCreated(Fight fight)
        {
            Action<Map, Fight> handler = FightCreated;
            if (handler != null)
                handler(this, fight);
        }

        public event Action<Map, Fight> FightRemoved;

        protected virtual void OnFightRemoved(Fight fight)
        {
            Action<Map, Fight> handler = FightRemoved;
            if (handler != null)
                handler(this, fight);
        }

        public event Action<Map, InteractiveObject> InteractiveSpawned;


        public event Action<Map, InteractiveObject> InteractiveUnSpawned;



        public event Action<Map, Character, InteractiveObject, Skill> InteractiveUsed;

        protected virtual void OnInteractiveUsed(Character user, InteractiveObject interactive, Skill skill)
        {
            InteractiveHandler.SendInteractiveUsedMessage(Clients, user, interactive, skill);

            Action<Map, Character, InteractiveObject, Skill> handler = InteractiveUsed;
            if (handler != null)
                handler(this, user, interactive, skill);
        }

        public event Action<Map, Character, InteractiveObject, Skill> InteractiveUseEnded;

        protected virtual void OnInteractiveUseEnded(Character user, InteractiveObject interactive, Skill skill)
        {
            Action<Map, Character, InteractiveObject, Skill> handler = InteractiveUseEnded;
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
            UpdateMapArrounds();
            UpdateCells();
            UpdateFightPlacements();
        }

        public void UpdateMapArrounds()
        {
            m_clientMapsAround.Clear();

            if (TopNeighbourId != -1 && !m_clientMapsAround.ContainsKey(TopNeighbourId))
                m_clientMapsAround.Add(TopNeighbourId, MapNeighbour.Top);

            if (BottomNeighbourId != -1 && !m_clientMapsAround.ContainsKey(BottomNeighbourId))
                m_clientMapsAround.Add(BottomNeighbourId, MapNeighbour.Bottom);

            if (LeftNeighbourId != -1 && !m_clientMapsAround.ContainsKey(LeftNeighbourId))
                m_clientMapsAround.Add(LeftNeighbourId, MapNeighbour.Left);

            if (RightNeighbourId != -1 && !m_clientMapsAround.ContainsKey(RightNeighbourId))
                m_clientMapsAround.Add(RightNeighbourId, MapNeighbour.Right);
        }

        public void UpdateFightPlacements()
        {
            // todo : search for default placements
            if (Record.BlueFightCells.Length == 0 || Record.RedFightCells.Length == 0)
            {
                m_bluePlacement = new[] { Cells[328], Cells[356], Cells[357] };
                m_redPlacement = new[] { Cells[370], Cells[355], Cells[354] };
            }
            else
            {
                m_bluePlacement = Record.BlueFightCells.Select(entry => Cells[entry]).ToArray();
                m_redPlacement = Record.RedFightCells.Select(entry => Cells[entry]).ToArray();
            }
        }

        public void UpdateCells()
        {
            CellsInfoProvider = new MapCellsInformationProvider(this);
            m_freeCells = Cells.Where(entry => CellsInfoProvider.IsCellWalkable(entry.Id)).ToArray();
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
        private readonly Dictionary<int, MapNeighbour> m_clientMapsAround = new Dictionary<int, MapNeighbour>();
        private readonly Dictionary<Cell, List<CellTrigger>> m_cellsTriggers = new Dictionary<Cell, List<CellTrigger>>();
        private readonly List<MonsterSpawn> m_monsterSpawns = new List<MonsterSpawn>();

        private Map m_bottomNeighbour;
        private Map m_leftNeighbour;
        private Map m_rightNeighbour;
        private Map m_topNeighbour;
        private Cell[] m_redPlacement;
        private Cell[] m_bluePlacement;
        private Cell[] m_freeCells;

        public MapRecord Record
        {
            get;
            private set;
        }

        public int Id
        {
            get { return Record.Id; }
        }

        public Cell[] Cells
        {
            get { return Record.Cells; }
        }

        public MapCellsInformationProvider CellsInfoProvider
        {
            get;
            private set;
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

        private List<SpawningPoolBase> m_spawningPools = new List<SpawningPoolBase>();

        public ReadOnlyCollection<SpawningPoolBase> SpawningPools
        {
            get { return m_spawningPools.AsReadOnly(); }
        }

        public bool SpawnEnabled
        {
            get;
            private set;
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
            get
            {
                return TopNeighbourId != -1 ? m_topNeighbour ?? ( m_topNeighbour = World.Instance.GetMap(TopNeighbourId) ) : null;
            }
        }

        public int BottomNeighbourId
        {
            get { return Record.BottomNeighbourId; }
            set { Record.BottomNeighbourId = value; }
        }

        public Map BottomNeighbour
        {
            get
            {
                return BottomNeighbourId != -1 ? m_bottomNeighbour ?? ( m_bottomNeighbour = World.Instance.GetMap(BottomNeighbourId) ) : null;
            }
        }

        public int LeftNeighbourId
        {
            get { return Record.LeftNeighbourId; }
            set { Record.LeftNeighbourId = value; }
        }

        public Map LeftNeighbour
        {
            get
            {
                return LeftNeighbourId != -1 ? m_leftNeighbour ?? ( m_leftNeighbour = World.Instance.GetMap(LeftNeighbourId) ) : null;
            }
        }

        public int RightNeighbourId
        {
            get { return Record.RightNeighbourId; }
            set { Record.RightNeighbourId = value; }
        }

        public Map RightNeighbour
        {
            get
            {
                return RightNeighbourId != -1 ? m_rightNeighbour ?? ( m_rightNeighbour = World.Instance.GetMap(RightNeighbourId) ) : null;
            }
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

        public InteractiveObject Zaap
        {
            get;
            private set;
        }

        #endregion

        #region Npcs

        public Npc SpawnNpc(NpcTemplate template, ObjectPosition position, EntityLook look)
        {
            if (position.Map != this)
                throw new Exception("Try to spawn a npc on the wrong map");

            sbyte id = GetNextContextualId();

            var npc = new Npc(id, template, position, look);
            template.OnNpcSpawned(npc);

            Enter(npc);

            return npc;
        }

        public Npc SpawnNpc(NpcSpawn spawn)
        {
            var position = spawn.GetPosition();

            if (position.Map != this)
                throw new Exception("Try to spawn a npc on the wrong map");

            sbyte id = GetNextContextualId();

            var npc = new Npc(id, spawn);
            spawn.Template.OnNpcSpawned(npc);

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

            if (interactiveObject.Template != null && interactiveObject.Template.Type == InteractiveTypeEnum.TYPE_ZAAP)
            {
                if (Zaap != null)
                    throw new Exception("Cannot add a second zaap on the map");

                Zaap = interactiveObject;
            }

            m_interactives.Add(interactiveObject.Id, interactiveObject);
            Area.Enter(interactiveObject);

            OnInteractiveSpawned(interactiveObject);

            return interactiveObject;
        }

        protected virtual void OnInteractiveSpawned(InteractiveObject interactive)
        {
            Action<Map, InteractiveObject> handler = InteractiveSpawned;
            if (handler != null)
                handler(this, interactive);
        }

        public void UnSpawnInteractive(InteractiveObject interactive)
        {
            if (interactive.Template != null && interactive.Template.Type == InteractiveTypeEnum.TYPE_ZAAP && Zaap != null)
                Zaap = null;

            interactive.Delete();
            m_interactives.Remove(interactive.Id);
            Area.Leave(interactive);

            OnInteractiveUnSpawned(interactive);
        }

        protected virtual void OnInteractiveUnSpawned(InteractiveObject interactive)
        {
            Action<Map, InteractiveObject> handler = InteractiveUnSpawned;
            if (handler != null)
                handler(this, interactive);
        }

        public bool UseInteractiveObject(Character character, int interactiveId, int skillId)
        {
            InteractiveObject interactiveObject = GetInteractiveObject(interactiveId);

            if (interactiveObject == null)
                return false;

            Skill skill = interactiveObject.GetSkill(skillId);

            if (skill == null)
                return false;


            if (skill.IsEnabled(character))
            {
                skill.Execute(character);

                OnInteractiveUsed(character, interactiveObject, skill);

                return true;
            }

            return false;
        }

        public bool NotifyInteractiveObjectUseEnded(Character character, int interactiveId, int skillId)
        {
            InteractiveObject interactiveObject = GetInteractiveObject(interactiveId);

            if (interactiveObject == null)
                return false; 
            
            Skill skill = interactiveObject.GetSkill(skillId);

            if (skill == null)
                return false;

            skill.PostExecute(character);

            OnInteractiveUseEnded(character, interactiveObject, skill);

            return true;
        }

        #endregion

        #region Monsters

        public int MonsterSpawnsCount
        {
            get { return m_monsterSpawns.Count; }
        }

        public ReadOnlyCollection<MonsterSpawn> MonsterSpawns
        {
            get
            {
                return m_monsterSpawns.AsReadOnly();
            }
        }

        public void AddSpawningPool(SpawningPoolBase spawningPool)
        {
            m_spawningPools.Add(spawningPool);
        }

        public bool RemoveSpawningPool(SpawningPoolBase spawningPool)
        {
            spawningPool.StopAutoSpawn();

            return m_spawningPools.Remove(spawningPool);
        }

        public void ClearSpawningPools()
        {
            foreach (var pool in SpawningPools.ToArray())
            {
                RemoveSpawningPool(pool);
            }
        }

        public void EnableClassicalMonsterSpawns()
        {
            if (SpawnEnabled)
                return;

            var pools = SpawningPools.OfType<ClassicalSpawningPool>().ToArray();

            if (pools.Length == 0)
            {
                var pool = new ClassicalSpawningPool(this, SubArea.GetMonsterSpawnInterval());

                AddSpawningPool(pool);
                pool.StartAutoSpawn();
            }

            foreach (var pool in pools)
            {
                pool.StartAutoSpawn();
            }

            SpawnEnabled = true;
        }

        public void DisableClassicalMonsterSpawns()
        {
            if (!SpawnEnabled)
                return;

            foreach (var actor in GetActors<MonsterGroup>())
            {
                if (actor.GetMonsters().All(entry => MonsterSpawns.Any(spawn => spawn.MonsterId == entry.Template.Id)))
                    Leave(actor);
            }

            foreach (var spawningPool in SpawningPools.OfType<ClassicalSpawningPool>())
            {
                if (spawningPool.AutoSpawnEnabled)
                    spawningPool.StopAutoSpawn();
            }

            SpawnEnabled = false;
        }

        public void AddMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Add(spawn);
        }

        public void RemoveMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Remove(spawn);
        }

        public void AddMonsterDungeonSpawn(MonsterDungeonSpawn spawn)
        {
            var pool = m_spawningPools.FirstOrDefault(entry => entry is DungeonSpawningPool) as DungeonSpawningPool;

            if (pool == null)
                AddSpawningPool(pool = new DungeonSpawningPool(this));

            pool.AddSpawn(spawn);

            if (!pool.AutoSpawnEnabled)
                pool.StartAutoSpawn();
        }

        public void RemoveMonsterDungeonSpawn(MonsterDungeonSpawn spawn)
        {
            var pool = m_spawningPools.FirstOrDefault(entry => entry is DungeonSpawningPool) as DungeonSpawningPool;

            if (pool == null)
                return;

            pool.RemoveSpawn(spawn);

            if (pool.SpawnsCount == 0)
                pool.StopAutoSpawn();
        }

        public MonsterGroup GenerateRandomMonsterGroup()
        {
            return GenerateRandomMonsterGroup(SubArea.RollMonsterLengthLimit());
        }

        public MonsterGroup GenerateRandomMonsterGroup(int minLength, int maxLength)
        {
            if (minLength == maxLength)
                GenerateRandomMonsterGroup(minLength);

            return GenerateRandomMonsterGroup(new AsyncRandom().Next(minLength, maxLength + 1));
        }

        public MonsterGroup GenerateRandomMonsterGroup(int length)
        {
            var rand = new AsyncRandom();

            if (MonsterSpawns.Count <= 0)
                return null;

            var freqSum = MonsterSpawns.Sum(entry => entry.Frequency);

            var group = new MonsterGroup(GetNextContextualId(), new ObjectPosition(this, GetRandomFreeCell(), GetRandomDirection()));

            for (int i = 0; i < length; i++)
            {
                var roll = rand.NextDouble(0, freqSum);
                var l = 0d;
                MonsterGrade monster = null;

                foreach (var spawn in MonsterSpawns)
                {
                    l += spawn.Frequency;

                    if (roll <= l)
                    {
                        monster = MonsterManager.Instance.GetMonsterGrade(spawn.MonsterId, SubArea.RollMonsterGrade(spawn.MinGrade, spawn.MaxGrade));
                        break;
                    }

                }

                if (monster == null)
                    continue;

                group.AddMonster(new Monster(monster, group));
            }

            if (group.Count() <= 0)
            {
#if DEBUG
                throw new Exception("An empty monster group has been generated !");
#else
                return null;
#endif
            }

            return group;
        }

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
            group.Delete();

            return true;
        }

        public int GetMonsterSpawnsCount()
        {
            return GetActors<MonsterGroup>().Count();
        }

        public int GetMonsterSpawnsLimit()
        {
            return SubArea.SpawnsLimit;
        }

        private void MoveRandomlyMonsterGroup(MonsterGroup group)
        {
            var circle = new Lozenge(1, 4);
            var dest = circle.GetCells(group.Cell, this).Where(entry => entry.Walkable && !entry.NonWalkableDuringRP && entry.MapChangeData == 0).RandomElementOrDefault();


            var pathfinder = new Pathfinder(CellsInfoProvider);
            var path = pathfinder.FindPath(group.Cell.Id, dest.Id, false);

            if (!path.IsEmpty())
                group.StartMove(path);

            group.MoveTimer = Area.CallDelayed(new Random().Next(MonsterGroup.MinMoveInterval, MonsterGroup.MaxMoveInterval + 1) * 1000,
                () => MoveRandomlyMonsterGroup(group));
        }

        private void IncrementMonsterGroupBonus(MonsterGroup group)
        {
            group.IncrementBonus(MonsterGroup.StarsBonusIncrementation);
            Refresh(group);

            group.StarsTimer = Area.CallDelayed(MonsterGroup.StarsBonusInterval * 1000,
                () => IncrementMonsterGroupBonus(group));
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

        public IEnumerable<Fight> GetFights()
        {
            return m_fights;
        }

        public short GetFightCount()
        {
            return (short) m_fights.Count;
        }

        public void AddFight(Fight fight)
        {
            if (fight.Map != this)
                return;

            m_fights.Add(fight);

            ContextRoleplayHandler.SendMapFightCountMessage(Clients, (short) m_fights.Count);

            OnFightCreated(fight);
        }

        public void RemoveFight(Fight fight)
        {
            m_fights.Remove(fight);

            ContextRoleplayHandler.SendMapFightCountMessage(Clients, (short) m_fights.Count);

            OnFightRemoved(fight);
        }

        public Cell[] GetBlueFightPlacement()
        {
            return m_bluePlacement;
        }

        public Cell[] GetRedFightPlacement()
        {
            return m_redPlacement;
        }

        #endregion

        #region Enter/Leave

        public void Enter(RolePlayActor actor)
        {
            if (m_actors.ContainsKey(actor.Id))
                Leave(actor);

            if (m_actors.TryAdd(actor.Id, actor))
            {
                OnActorEnter(actor);
            }
            else
                logger.Error("Could not add actor '{0}' to the map", actor);
        }

        public void Leave(RolePlayActor actor)
        {
            if (m_actors.TryRemove(actor.Id, out actor))
            {
                OnActorLeave(actor);
            }
            else
                logger.Error(string.Format("Could not remove actor '{0}' of the map", actor));
        }

        public void Refresh(RolePlayActor actor)
        {
            if (IsActor(actor))
                ContextRoleplayHandler.SendGameRolePlayShowActorMessage(Clients, actor);
        }

        private void OnEnter(RolePlayActor actor)
        {
            // if the actor change from area we notify it
            if (actor.HasChangedZone())
                Area.Enter(actor);

            actor.StartMoving += OnActorStartMoving;
            actor.StopMoving += OnActorStopMoving;

            var character = actor as Character;

            if (character != null)
                Clients.Add(character.Client);

            ContextRoleplayHandler.SendGameRolePlayShowActorMessage(Clients, actor);

            if (character != null)
            {
                ContextRoleplayHandler.SendCurrentMapMessage(character.Client, Id);

                if (m_fights.Count > 0)
                    ContextRoleplayHandler.SendMapFightCountMessage(character.Client, (short) m_fights.Count);

                SendActorsActions(character);
                BasicHandler.SendBasicTimeMessage(character.Client);

                if (Zaap != null && !character.KnownZaaps.Contains(this))
                    character.DiscoverZaap(this);
            }

            var monsterGroup = actor as MonsterGroup;
            if (monsterGroup != null)
            {
                monsterGroup.MoveTimer = Area.CallDelayed(new Random().Next(MonsterGroup.MinMoveInterval, MonsterGroup.MaxMoveInterval + 1) * 1000,
                    () => MoveRandomlyMonsterGroup(monsterGroup));
                monsterGroup.StarsTimer = Area.CallDelayed(MonsterGroup.StarsBonusInterval * 1000,
                    () => IncrementMonsterGroupBonus(monsterGroup));
            }
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
            // if the actor will change of area we notify it
            if (actor.IsGonnaChangeZone())
                Area.Leave(actor);

            actor.StartMoving -= OnActorStartMoving;
            actor.StopMoving -= OnActorStopMoving;

            if (actor is Character)
                Clients.Remove(( (Character)actor ).Client);

            ContextHandler.SendGameContextRemoveElementMessage(Clients, actor);

            if (actor is MonsterGroup || actor is Npc)
                FreeContextualId((sbyte) actor.Id);


            var monsterGroup = actor as MonsterGroup;
            if (monsterGroup != null)
            {
                monsterGroup.MoveTimer.Stop();
                monsterGroup.MoveTimer = null;
                monsterGroup.StarsTimer.Stop();
                monsterGroup.StarsTimer = null;
            }
        }

        #endregion

        #region Actor Actions

        private void OnActorStartMoving(ContextActor actor, Path path)
        {
            var movementsKey = path.GetServerPathKeys();

             ContextHandler.SendGameMapMovementMessage(Clients, movementsKey, actor);
             BasicHandler.SendBasicNoOperationMessage(Clients);
        }

        private void OnActorStopMoving(ContextActor actor, Path path, bool canceled)
        {
            var character = actor as Character;

            if (ExecuteTrigger(CellTriggerType.END_MOVE_ON, actor.Cell, character))
                return;

            var monster = GetActor<MonsterGroup>(entry => entry.Cell.Id == character.Cell.Id);

            if (monster != null)
                monster.FightWith(character);
        }

        #endregion

        #region Gets

        private readonly WorldClientCollection m_clients = new WorldClientCollection();

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        public WorldClientCollection Clients
        {
            get { return m_clients; }
        }

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
            return m_actors.Values.OfType<T>().Where(entry => predicate(entry)).FirstOrDefault();
        }

        public IEnumerable<T> GetActors<T>()
        {
            return m_actors.Values.OfType<T>(); // after a benchmark we conclued that it takes approximatively 10 ticks
        }

        public IEnumerable<T> GetActors<T>(Predicate<T> predicate)
        {
            return m_actors.Values.OfType<T>().Where(entry => predicate(entry));
        }

        public Cell GetCell(int id)
        {
            return Cells[id];
        }

        public Cell GetCell(int x, int y)
        {
            return Cells[MapPoint.CoordToCellId(x, y)];
        }

        public Cell GetCell(Point pos)
        {
            return GetCell(pos.X, pos.Y);
        }

        public InteractiveObject GetInteractiveObject(int id)
        {
            return m_interactives[id];
        }

        public ObjectPosition GetRandomFreePosition(bool actorFree = false)
        {
            return new ObjectPosition(this, GetRandomFreeCell(actorFree), GetRandomDirection());
        }

        public DirectionsEnum GetRandomDirection()
        {
            var array = Enum.GetValues(typeof(DirectionsEnum));
            var rand = new AsyncRandom();

            return (DirectionsEnum)array.GetValue(rand.Next(0, array.Length));
        }

        public Cell GetRandomFreeCell(bool actorFree = false)
        {
            var rand = new AsyncRandom();

            if (actorFree)
            {
                var excludedCells = GetActors<RolePlayActor>().Select(entry => entry.Cell.Id);
                var cells = m_freeCells.Where(entry => !excludedCells.Contains(entry.Id)).ToArray();

                return cells[rand.Next(0, cells.Length)];
            }

            return m_freeCells[rand.Next(0, m_freeCells.Length)];
        }

        public Cell GetRandomAdjacentFreeCell(MapPoint cell, bool actorFree = false)
        {
            if (actorFree)
            {
                var excludedCells = GetActors<RolePlayActor>().Select(entry => entry.Cell.Id);
                var cells = cell.GetAdjacentCells(entry => CellsInfoProvider.IsCellWalkable(entry) && !excludedCells.Contains(entry));

                var pt = cells.RandomElementOrDefault();

                return pt != null ? Cells[pt.CellId] : Cell.Null;
            }
            else
            {
                var cells = cell.GetAdjacentCells(CellsInfoProvider.IsCellWalkable);

                var pt = cells.RandomElementOrDefault();

                return pt != null ? Cells[pt.CellId] : Cell.Null;
            }
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

        public MapNeighbour GetClientMapRelativePosition(int mapid)
        {
            return !m_clientMapsAround.ContainsKey(mapid) ? MapNeighbour.None : m_clientMapsAround[mapid];
        }

        /// <summary>
        ///   Calculate which cell our character should walk on once map changed. Returns 0 if not found
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