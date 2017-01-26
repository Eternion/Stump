using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.Core.Threading;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Database.World.Maps;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Maps.Spawns;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Interactives;
using MapRecord = Stump.Server.WorldServer.Database.World.Maps.MapRecord;
using Monster = Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters.Monster;
using MonsterGrade = Stump.Server.WorldServer.Database.Monsters.MonsterGrade;
using MonsterSpawn = Stump.Server.WorldServer.Database.Monsters.MonsterSpawn;
using NpcSpawn = Stump.Server.WorldServer.Database.Npcs.NpcSpawn;
using NpcTemplate = Stump.Server.WorldServer.Database.Npcs.NpcTemplate;

namespace Stump.Server.WorldServer.Game.Maps
{
    public class Map : WorldObjectsContext, ICharacterContainer
    {
        [Variable(true)]
        public static int MaxMerchantsPerMap = 5;

        [Variable(true)]
        public static int AutoMoveActorMaxInverval = 40;

        [Variable(true)]
        public static int AutoMoveActorMinInverval = 20;

        [Variable(true)]
        public static int InteractiveUseAdditionalDelay = 500;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Events

        public event Action<Map, RolePlayActor> ActorEnter;

        protected virtual void OnActorEnter(RolePlayActor actor)
        {
            OnEnter(actor);

            ActorEnter?.Invoke(this, actor);
        }

        public event Action<Map, RolePlayActor> ActorLeave;

        protected virtual void OnActorLeave(RolePlayActor actor)
        {
            OnLeave(actor);

            ActorLeave?.Invoke(this, actor);
        }

        public event Action<Map, WorldObjectItem> ObjectItemEnter;

        protected virtual void OnObjectItemEnter(WorldObjectItem objectItem)
        {
            OnObjectEnter(objectItem);

            ObjectItemEnter?.Invoke(this, objectItem);
        }

        public event Action<Map, WorldObjectItem> ObjectItemLeave;

        protected virtual void OnObjectItemLeave(WorldObjectItem objectItem)
        {
            OnObjectLeave(objectItem);

            ObjectItemLeave?.Invoke(this, objectItem);
        }

        public event Action<Map, IFight> FightCreated;

        protected virtual void OnFightCreated(IFight fight)
        {
            FightCreated?.Invoke(this, fight);
        }

        public event Action<Map, IFight> FightRemoved;

        protected virtual void OnFightRemoved(IFight fight)
        {
            FightRemoved?.Invoke(this, fight);
        }

        public event Action<Map, InteractiveObject> InteractiveSpawned;

        public event Action<Map, InteractiveObject> InteractiveUnSpawned;

        public event Action<Map, Character, InteractiveObject, Skill> InteractiveUsed;

        protected virtual void OnInteractiveUsed(Character user, InteractiveObject interactive, Skill skill)
        {
            InteractiveHandler.SendInteractiveUsedMessage(Clients, user, interactive, skill);

            InteractiveUsed?.Invoke(this, user, interactive, skill);
        }

        public event Action<Map, Character, InteractiveObject, Skill> InteractiveUseEnded;

        protected virtual void OnInteractiveUseEnded(Character user, InteractiveObject interactive, Skill skill)
        {
            InteractiveHandler.SendInteractiveUseEndedMessage(Clients, interactive, skill);

            InteractiveUseEnded?.Invoke(this, user, interactive, skill);
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

            if (Record.ClientTopNeighbourId != -1 && !m_clientMapsAround.ContainsKey(Record.ClientTopNeighbourId))
                m_clientMapsAround.Add(Record.ClientTopNeighbourId, MapNeighbour.Top);

            if (Record.ClientBottomNeighbourId != -1 && !m_clientMapsAround.ContainsKey(Record.ClientBottomNeighbourId))
                m_clientMapsAround.Add(Record.ClientBottomNeighbourId, MapNeighbour.Bottom);

            if (Record.ClientLeftNeighbourId != -1 && !m_clientMapsAround.ContainsKey(Record.ClientLeftNeighbourId))
                m_clientMapsAround.Add(Record.ClientLeftNeighbourId, MapNeighbour.Left);

            if (Record.ClientRightNeighbourId != -1 && !m_clientMapsAround.ContainsKey(Record.ClientRightNeighbourId))
                m_clientMapsAround.Add(Record.ClientRightNeighbourId, MapNeighbour.Right);
        }

        public void UpdateFightPlacements()
        {
            // todo : search for default placements
            if (Record.BlueFightCells.Length == 0 || Record.RedFightCells.Length == 0)
            {
                m_bluePlacement = new Cell[0];
                m_redPlacement = new Cell[0];
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

            var middle = new MapPoint((int)MapPoint.MapWidth / 2, (int)MapPoint.MapHeight / 2);
            m_freeCells = Cells.Where(entry => CellsInfoProvider.IsCellWalkable(entry.Id)).
                OrderBy(x => middle.ManhattanDistanceTo(x)).ToArray();
        }

        public void UpdateAvailableJobs()
        {
            AvailableJobs = m_interactives.Values.SelectMany(x => x.GetSkills()).OfType<SkillCraft>().
                Select(x => JobManager.Instance.GetJobTemplate(x.SkillTemplate.ParentJobId)).ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Array that associate a cell to a map point
        /// </summary>
        public static MapPoint[] PointsGrid;

        private readonly List<WorldObjectItem> m_objectItems = new List<WorldObjectItem>();
        private readonly List<RolePlayActor> m_actors = new List<RolePlayActor>();
        private readonly ConcurrentDictionary<int, RolePlayActor> m_actorsMap = new ConcurrentDictionary<int, RolePlayActor>();
        private readonly ReversedUniqueIdProvider m_contextualIds = new ReversedUniqueIdProvider(0);
        private readonly List<IFight> m_fights = new List<IFight>();
        private readonly Dictionary<int, InteractiveObject> m_interactives = new Dictionary<int, InteractiveObject>();
        private readonly Dictionary<int, MapNeighbour> m_clientMapsAround = new Dictionary<int, MapNeighbour>();
        private readonly Dictionary<Cell, List<CellTrigger>> m_cellsTriggers = new Dictionary<Cell, List<CellTrigger>>();
        private readonly List<MonsterSpawn> m_monsterSpawns = new List<MonsterSpawn>();
        private TimedTimerEntry m_autoMoveTimer;

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

        public override Cell[] Cells
        {
            get { return Record.Cells; }
        }

        protected override IReadOnlyCollection<WorldObject> Objects
        {
            get
            {
                return Actors;
            }
        }

        public IReadOnlyCollection<RolePlayActor> Actors
        {
            get { return m_actors.AsReadOnly(); }
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

        private readonly List<SpawningPoolBase> m_spawningPools = new List<SpawningPoolBase>();

        public ReadOnlyCollection<SpawningPoolBase> SpawningPools
        {
            get { return m_spawningPools.AsReadOnly(); }
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

        public int Capabilities
        {
            get
            {
                return Record.Position != null ? Record.Position.Capabilities : 0xFFFF;
            }
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
                return TopNeighbourId != -1 ? m_topNeighbour ?? (m_topNeighbour = World.Instance.GetMap(TopNeighbourId)) : null;
            }
            set
            {
                m_topNeighbour = value;
                TopNeighbourId = value != null ? value.Id : -1;
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
                return BottomNeighbourId != -1 ? m_bottomNeighbour ?? (m_bottomNeighbour = World.Instance.GetMap(BottomNeighbourId)) : null;
            }
            set
            {
                m_bottomNeighbour = value;
                BottomNeighbourId = value != null ? value.Id : -1;
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
                return LeftNeighbourId != -1
                    ? m_leftNeighbour ?? (m_leftNeighbour = World.Instance.GetMap(LeftNeighbourId))
                    : null;
            }
            set
            {
                m_leftNeighbour = value;
                LeftNeighbourId = value != null ? value.Id : -1;
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
                return RightNeighbourId != -1 ? m_rightNeighbour ?? (m_rightNeighbour = World.Instance.GetMap(RightNeighbourId)) : null;
            }
            set
            {
                m_rightNeighbour = value;
                RightNeighbourId = value != null ? value.Id : -1;
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

        public InteractiveObject Zaapi
        {
            get;
            private set;
        }

        public bool IsMuted
        {
            get;
            private set;
        }

        protected TimedTimerEntry DroppedItemsCleaner
        {
            get;
            set;
        }

        public JobTemplate[] AvailableJobs
        {
            get;
            private set;
        }

        #endregion

        #region Restrictions

        public bool AllowChallenge
        {
            get { return (Capabilities & 1) != 0; }
        }

        public bool AllowAggression
        {
            get { return (Capabilities & 2) != 0; }
        }

        public bool AllowTeleportTo
        {
            get { return (Capabilities & 4) != 0; }
        }

        public bool AllowTeleportFrom
        {
            get { return (Capabilities & 8) != 0; }
        }

        public bool AllowExchangesBetweenPlayers
        {
            get { return (Capabilities & 16) != 0; }
        }

        public bool AllowHumanVendor
        {
            get { return (Capabilities & 32) != 0; }
        }

        public bool AllowCollector
        {
            get { return (Capabilities & 64) != 0; }
        }

        public bool AllowSoulCapture
        {
            get { return (Capabilities & 128) != 0; }
        }

        public bool AllowSoulSummon
        {
            get { return (Capabilities & 256) != 0; }
        }

        public bool AllowTavernRegen
        {
            get { return (Capabilities & 512) != 0; }
        }

        public bool AllowTombMode
        {
            get { return (Capabilities & 1024) != 0; }
        }

        public bool AllowTeleportEverywhere
        {
            get { return (Capabilities & 2048) != 0; }
        }

        public bool AllowFightChallenges
        {
            get { return (Capabilities & 4096) != 0; }
        }

        #endregion

        #region Npcs

        public Npc SpawnNpc(NpcTemplate template, ObjectPosition position, ActorLook look)
        {
            if (position.Map != this)
                throw new Exception("Try to spawn a npc on the wrong map");

            var id = GetNextContextualId();

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

            var id = GetNextContextualId();

            var npc = new Npc(id, spawn);
            spawn.Template.OnNpcSpawned(npc);

            Enter(npc);

            return npc;
        }

        public bool UnSpawnNpc(short id)
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

        #region TaxCollector

        public TaxCollectorNpc TaxCollector
        {
            get;
            private set;
        }

        #endregion

        #region Interactives

        public InteractiveObject SpawnInteractive(InteractiveSpawn spawn)
        {
            var interactiveObject = new InteractiveObject(this, spawn);

            if (interactiveObject.Template != null && interactiveObject.Template.Type == InteractiveTypeEnum.TYPE_ZAAP)
            {
                if (Zaap != null)
                    logger.Error("Cannot add a second zaap on the map");

                Zaap = interactiveObject;
            }
            else if (interactiveObject.Template != null && interactiveObject.Template.Type == InteractiveTypeEnum.TYPE_ZAAPI)
            {
                if (Zaapi != null)
                    logger.Error("Cannot add a second zaapi on the map");

                Zaapi = interactiveObject;
            }

            if (m_interactives.ContainsKey(interactiveObject.Id))
            {
                logger.Error("Interactive object {0} already exists on map {1}", interactiveObject.Id, Id);
                return null;
            }

            m_interactives.Add(interactiveObject.Id, interactiveObject);
            Area.Enter(interactiveObject);

            OnInteractiveSpawned(interactiveObject);

            //logger.Debug("Spawn interactive {0}", interactiveObject.Id);

            return interactiveObject;
        }

        protected virtual void OnInteractiveSpawned(InteractiveObject interactive)
        {
            InteractiveSpawned?.Invoke(this, interactive);
        }

        public void UnSpawnInteractive(InteractiveObject interactive)
        {
            if (interactive.Template != null && interactive.Template.Type == InteractiveTypeEnum.TYPE_ZAAP && Zaap != null)
                Zaap = null;
            else if (interactive.Template != null && interactive.Template.Type == InteractiveTypeEnum.TYPE_ZAAPI && Zaapi != null)
                Zaapi = null;

            interactive.Delete();
            m_interactives.Remove(interactive.Id);
            Area.Leave(interactive);

            OnInteractiveUnSpawned(interactive);
        }

        protected virtual void OnInteractiveUnSpawned(InteractiveObject interactive)
        {
            InteractiveUnSpawned?.Invoke(this, interactive);
        }

        public bool UseInteractiveObject(Character character, int interactiveId, int skillId)
        {
            var interactiveObject = GetInteractiveObject(interactiveId);

            if (interactiveObject == null)
            {
                InteractiveHandler.SendInteractiveUseErrorMessage(character.Client, interactiveId, skillId);
                return false;
            }

            var adjacentCells = interactiveObject.Position.Point.GetAdjacentCells(true).
                Where(x => x.IsInMap()).Select(x => Cells[x.CellId]).ToArray();

            // user must be near the interactive if possible
            if (adjacentCells.Any(x => x.Walkable) && 
                character.Position.Point.ManhattanDistanceTo(interactiveObject.Position.Point) > 2)
            {
                InteractiveHandler.SendInteractiveUseErrorMessage(character.Client, interactiveId, skillId);
                return false;
            }

            var skill = interactiveObject.GetSkill(skillId);

            if (skill == null)
            {
                InteractiveHandler.SendInteractiveUseErrorMessage(character.Client, interactiveId, skillId);
                return false;
            }

            if (!skill.AreConditionsFilled(character))
            {
                // Conditions non satisfaites
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);
                return false;
            }

            if (!skill.CanUse(character) || character.CurrentUsedSkill != null)
            {
                InteractiveHandler.SendInteractiveUseErrorMessage(character.Client, interactiveId, skillId);
                return false;
            }

            OnInteractiveUsed(character, interactiveObject, skill);

            var delay = skill.StartExecute(character);

            if (delay > 0)
            {
                character.SetCurrentSkill(skill);
                Area.CallDelayed(delay + InteractiveUseAdditionalDelay, () => InteractiveUsedCallback(character, skill));
            }
            else if (delay == 0)
                InteractiveUsedCallback(character, skill);
            else if (delay < 0)
            {
                InteractiveHandler.SendInteractiveUseErrorMessage(character.Client, interactiveId, skillId);
                return false;
            }

            return true;
        }

        private void InteractiveUsedCallback(Character character, Skill skill)
        {
            skill.EndExecute(character);
            character.ResetCurrentSkill();
            OnInteractiveUseEnded(character, skill.InteractiveObject, skill);
        }

        public void Refresh(InteractiveObject interactive)
        {
            foreach(var character in GetAllCharacters())
            {
                InteractiveHandler.SendInteractiveElementUpdatedMessage(character.Client, character, interactive);
            }
        }

        #endregion

        #region Monsters

        public int MonsterSpawnsCount => m_monsterSpawns.Count;

        public ReadOnlyCollection<MonsterSpawn> MonsterSpawns => m_monsterSpawns.AsReadOnly();

        public bool CanSpawnMonsters() => m_bluePlacement.Length > 0 && m_redPlacement.Length > 0;

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
            if (!CanSpawnMonsters())
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
        }

        public void DisableClassicalMonsterSpawns()
        {
            foreach (var actor in GetActors<MonsterGroup>().Where(actor => actor.GetMonsters().All(entry => MonsterSpawns.Any(spawn => spawn.MonsterId == entry.Template.Id))).ToArray())
            {
                Leave(actor);
            }

            foreach (var spawningPool in SpawningPools.OfType<ClassicalSpawningPool>().Where(spawningPool => spawningPool.AutoSpawnEnabled))
            {
                spawningPool.StopAutoSpawn();
            }
        }

        public void AddMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Add(spawn);
        }

        public void RemoveMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Remove(spawn);
        }

        public void RemoveMonsterSpawns(int monsterId)
        {
            m_monsterSpawns.RemoveAll(x => x.MonsterId == monsterId);
        }

        public void RemoveMonsterSpawns()
        {
            m_monsterSpawns.Clear();
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

        public void AddMonsterStaticSpawn(MonsterStaticSpawn spawn)
        {
            var pool = m_spawningPools.FirstOrDefault(entry => entry is StaticSpawningPool) as StaticSpawningPool;

            if (pool == null)
                AddSpawningPool(pool = new StaticSpawningPool(this, StaticSpawningPool.StaticSpawnsInterval));

            pool.AddSpawn(spawn);

            if (!pool.AutoSpawnEnabled)
                pool.StartAutoSpawn();
        }

        public void RemoveMonsterStaticSpawn(MonsterStaticSpawn spawn)
        {
            var pool = m_spawningPools.FirstOrDefault(entry => entry is StaticSpawningPool) as StaticSpawningPool;

            if (pool == null)
                return;

            pool.RemoveSpawn(spawn);

            if (pool.SpawnsCount == 0)
                pool.StopAutoSpawn();
        }

        public MonsterGroup GenerateRandomMonsterGroup() => GenerateRandomMonsterGroup(SubArea.RollMonsterLengthLimit());

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

            for (var i = 0; i < length; i++)
            {
                var roll = rand.NextDouble(0, freqSum);
                var l = 0d;
                MonsterGrade monster = null;

                foreach (var spawn in MonsterSpawns)
                {
                    l += spawn.Frequency;

                    if (!(roll <= l))
                        continue;

                    monster = MonsterManager.Instance.GetMonsterGrade(spawn.MonsterId, SubArea.RollMonsterGrade(spawn.MinGrade, spawn.MaxGrade));

                    if (monster != null)
                        break;

                    /*if (CheckMonsterAI(monster))
                        break;*/
                }

                if (monster == null)
                    continue;

                group.AddMonster(new Monster(monster, group));
            }

            return @group.Count() <= 0 ? null : @group;
        }

        public MonsterGroup GenerateRandomMonsterGroup(MonsterGroup monsterGroup)
        {
            var group = new MonsterGroup(GetNextContextualId(), new ObjectPosition(this, GetRandomFreeCell(), GetRandomDirection()));

            foreach (var monster in monsterGroup.GetMonsters())
            {
                group.AddMonster(new Monster(monster.Grade, group));
            }

            return @group.Count() <= 0 ? null : @group;
        }

        /// <summary>
        /// Check the AI manage monster spells
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        private static bool CheckMonsterAI(MonsterGrade grade)
        {
            var categories = grade.Spells.Select(SpellIdentifier.GetSpellCategories);

            return
                categories.Any(
                    x =>
                    (x & SpellCategory.Damages) != 0 ||
                    x.HasFlag(SpellCategory.Healing));
        }

        public MonsterGroup SpawnMonsterGroup(MonsterGrade monster, ObjectPosition position)
        {
            if (position.Map != this)
                throw new Exception("Try to spawn a monster group on the wrong map");

            var id = GetNextContextualId();

            var group = new MonsterGroup(id, position);

            group.AddMonster(new Monster(monster, group));

            Enter(group);

            return group;
        }

        public MonsterGroup SpawnMonsterGroup(IEnumerable<MonsterGrade> monsters, ObjectPosition position)
        {
            if (position.Map != this)
                throw new Exception("Try to spawn a monster group on the wrong map");

            var id = GetNextContextualId();

            var group = new MonsterGroup(id, position);

            foreach (var grade in monsters)
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

        private void MoveRandomlyActors()
        {
            foreach (var actor in Actors.Where(x => x is IAutoMovedEntity && ((IAutoMovedEntity)x).NextMoveDate <= DateTime.Now))
            {
                if (actor is MonsterGroup)
                {
                    if (((MonsterGroup)actor).SpawningPool is StaticSpawningPool)
                        continue;
                }

                var circle = new Lozenge(1, 4);
                var dest = circle.GetCells(actor.Cell, this).
                    Where(entry => entry.Walkable && !entry.NonWalkableDuringRP && entry.MapChangeData == 0).RandomElementOrDefault();

                // no possible move :/
                if (dest == null)
                    return;

                var pathfinder = new Pathfinder(CellsInfoProvider);
                var path = pathfinder.FindPath(actor.Cell.Id, dest.Id, false);

                if (!path.IsEmpty())
                    actor.StartMove(path);

                (actor as IAutoMovedEntity).NextMoveDate =
                    DateTime.Now + TimeSpan.FromSeconds(new AsyncRandom().Next(AutoMoveActorMinInverval,
                        AutoMoveActorMaxInverval + 1));
            }

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

        public IEnumerable<CellTrigger> GetTriggers()
        {
            return m_cellsTriggers.Values.SelectMany(x => x);
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

        public ReadOnlyCollection<IFight> Fights => m_fights.AsReadOnly();

        public short GetFightCount() => (short)m_fights.Count;

        public void AddFight(IFight fight)
        {
            if (fight.Map != this)
                return;

            m_fights.Add(fight);

            ContextRoleplayHandler.SendMapFightCountMessage(Clients, (short)m_fights.Count);

            OnFightCreated(fight);
        }

        public void RemoveFight(IFight fight)
        {
            m_fights.Remove(fight);

            ContextRoleplayHandler.SendMapFightCountMessage(Clients, (short)m_fights.Count);

            OnFightRemoved(fight);
        }

        public Cell[] GetBlueFightPlacement() => m_bluePlacement;

        public Cell[] GetRedFightPlacement() => m_redPlacement;

        #endregion

        #region Enter/Leave

        public void Enter(RolePlayActor actor)
        {
#if DEBUG

            if (WorldServer.Instance.IsInitialized)
                Area.EnsureContext();

#endif

            if (m_actors.Contains(actor))
            {
                logger.Error("Map already contains actor {0}", actor);
                Leave(actor);
            }

            if (m_actorsMap.ContainsKey(actor.Id))
            {
                logger.Error("Map already contains actor {0}", actor.Id);
                Leave(actor.Id);
            }

            m_actors.Add(actor);
            m_actorsMap.TryAdd(actor.Id, actor);

            OnActorEnter(actor);
        }

        public void Leave(RolePlayActor actor)
        {
#if DEBUG

            if (WorldServer.Instance.IsInitialized)
                Area.EnsureContext();

#endif

            if (!m_actors.Remove(actor))
                return;

            RolePlayActor removedActor;
            if (m_actorsMap.TryRemove(actor.Id, out removedActor))
            {
                if (removedActor != actor)
                    logger.Error("Did not removed the expected actor !!");
                // todo : manage this errors better ..
            }

            OnActorLeave(actor);
        }

        public void Leave(int actorId)
        {
#if DEBUG

            if (WorldServer.Instance.IsInitialized)
                Area.EnsureContext();

#endif

            RolePlayActor removedActor;
            if (m_actorsMap.TryRemove(actorId, out removedActor) && m_actors.Remove(removedActor))
            {
                OnActorLeave(removedActor);
            }
        }

        public void Enter(WorldObjectItem objectItem)
        {
#if DEBUG

            if (WorldServer.Instance.IsInitialized)
                Area.EnsureContext();

#endif

            if (m_objectItems.Contains(objectItem))
            {
                logger.Error("Map already contains objectItem {0}", objectItem.Item.Id);
                Leave(objectItem);
            }

            if (IsObjectItemOnCell(objectItem.Cell.Id))
            {
                logger.Error("Cannot add {0} to the map, Cell {1} already occupied", objectItem, objectItem.Cell.Id);
                return;
            }

            m_objectItems.Add(objectItem);

            OnObjectItemEnter(objectItem);
        }

        public void Leave(WorldObjectItem objectItem)
        {
#if DEBUG

            if (WorldServer.Instance.IsInitialized)
                Area.EnsureContext();

#endif

            if (m_objectItems.Remove(objectItem))
            {
                OnObjectItemLeave(objectItem);
            }
        }

        public void Refresh(RolePlayActor actor)
        {
#if DEBUG

            if (WorldServer.Instance.IsInitialized)
                Area.EnsureContext();

#endif

            if (IsActor(actor))
                ForEach(x =>
                {
                    if (actor.CanBeSee(x))
                        ContextRoleplayHandler.SendGameRolePlayShowActorMessage(x.Client, x, actor);
                    else
                        ContextHandler.SendGameContextRemoveElementMessage(x.Client, actor);
                });
        }

        void CleanObjets()
        {
            foreach (var item in m_objectItems.Where(x => (DateTime.Now - x.SpawnDate).TotalMinutes >= 5).ToArray())
            {
                Leave(item);
            }
        }

        void OnObjectEnter(WorldObjectItem objectItem)
        {
            ForEach(x =>
            {
                ContextRoleplayHandler.SendObjectGroundAddedMessage(x.Client, objectItem);
            });

            if (DroppedItemsCleaner == null)
                DroppedItemsCleaner = Area.CallPeriodically(30000, CleanObjets);
        }

        void OnObjectLeave(WorldObjectItem objectItem)
        {
            ForEach(x =>
            {
                ContextRoleplayHandler.SendObjectGroundRemovedMessage(x.Client, objectItem);
            });

            if (DroppedItemsCleaner == null || IsAnyDroppedItems())
                return;

            DroppedItemsCleaner.Dispose();
            DroppedItemsCleaner = null;
        }

        void OnEnter(RolePlayActor actor)
        {
            // if the actor change from area we notify it
            if (actor.HasChangedZone())
                Area.Enter(actor);

            actor.StartMoving += OnActorStartMoving;
            actor.StopMoving += OnActorStopMoving;

            var character = actor as Character;

            if (character != null)
                Clients.Add(character.Client);

            if (actor is TaxCollectorNpc)
            {
                if (TaxCollector != null)
                {
                    logger.Error("There is already a Tax Collector on that map ({0}).", Id);
                    Leave(actor);
                    return;
                }
                TaxCollector = (TaxCollectorNpc)actor;
            }
            if (actor is IAutoMovedEntity)
            {
                (actor as IAutoMovedEntity).NextMoveDate =
                    DateTime.Now + TimeSpan.FromSeconds(new AsyncRandom().Next(AutoMoveActorMinInverval,
                        AutoMoveActorMaxInverval + 1));

                // if the timer wasn't active (=no actors)
                if (m_autoMoveTimer == null) // call every (max+min)/2/10 to have an average 5% accuracy
                    m_autoMoveTimer = Area.CallPeriodically((AutoMoveActorMaxInverval + AutoMoveActorMinInverval) / 20 * 1000,
                        MoveRandomlyActors);
            }

            actor.OnEnterMap(this);

            ForEach(x =>
            {
                if (actor.CanBeSee(x))
                    ContextRoleplayHandler.SendGameRolePlayShowActorMessage(x.Client, x, actor);
            });
        }

        void OnLeave(RolePlayActor actor)
        {
            if (actor == TaxCollector)
            {
                TaxCollector = null;
            }

            // if the actor will change of area we notify it
            if (actor.IsGonnaChangeZone())
                Area.Leave(actor);

            actor.StartMoving -= OnActorStartMoving;
            actor.StopMoving -= OnActorStopMoving;

            var character = actor as Character;
            if (character != null)
                Clients.Remove(character.Client);

            ContextHandler.SendGameContextRemoveElementMessage(Clients, actor);

            if (actor is IContextDependant)
                FreeContextualId((sbyte)actor.Id);

            if (m_autoMoveTimer != null && !Actors.OfType<Character>().Any())
            {
                m_autoMoveTimer.Dispose();
                m_autoMoveTimer = null;
            }

            actor.OnLeaveMap(this);
        }

        #endregion

        #region Actor Actions

        void OnActorStartMoving(ContextActor actor, Path path)
        {
            var movementsKey = path.GetServerPathKeys();

            var i = 0;
            var stop = false;

            foreach (var cell in path.GetPath())
            {
                if (stop)
                    continue;

                i++;
                foreach (var trigger in GetTriggers(cell))
                {
                    if (trigger.TriggerType == CellTriggerType.MOVE_ON)
                        stop = true;
                }
            }

            path.CutPath(i);
            movementsKey = path.GetServerPathKeys();

            if (path.Walk)
                ContextHandler.SendGameCautiousMapMovementMessage(Clients, movementsKey, actor);
            else
                ContextHandler.SendGameMapMovementMessage(Clients, movementsKey, actor);

            BasicHandler.SendBasicNoOperationMessage(Clients);

            actor.IsInMovement = true;
            actor.LastEmoteUsed = null;

            var character = actor as Character;
            if (character == null)
                return;
        }

        private void OnActorStopMoving(ContextActor actor, Path path, bool canceled)
        {
            actor.IsInMovement = false;

            var character = actor as Character;
            if (character == null)
                return;

            var objectItem = GetObjectItem(actor.Cell.Id);

            if (objectItem != null)
                character.GetDroppedItem(objectItem);

            if (ExecuteTrigger(CellTriggerType.END_MOVE_ON, actor.Cell, character) || ExecuteTrigger(CellTriggerType.MOVE_ON, actor.Cell, character))
                return;
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
            foreach (var character in GetAllCharacters())
            {
                action(character);
            }
        }

        public int GetNextContextualId()
        {
            lock (m_contextualIds)
            {
                return m_contextualIds.Pop();
            }
        }

        public void FreeContextualId(int id)
        {
            lock (m_contextualIds)
            {
                m_contextualIds.Push(id);
            }
        }

        public bool IsActor(int id)
        {
            return m_actorsMap.ContainsKey(id);
        }

        public bool IsActor(RolePlayActor actor)
        {
            return IsActor(actor.Id);
        }

        public bool IsCellFree(short cell)
        {
            return Objects.All(x => x.Cell.Id != cell);
        }

        public bool IsCellFree(short cell, WorldObject exclude)
        {
            return exclude != null && Objects.All(x => x == exclude || x.Cell.Id != cell);
        }

        public bool IsObjectItemOnCell(short cell)
        {
            return m_objectItems.Any(x => x.Cell.Id == cell);
        }

        public T GetActor<T>(int id)
            where T : RolePlayActor
        {
            RolePlayActor actor;
            if (m_actorsMap.TryGetValue(id, out actor))
                return actor as T;

            return null;
        }

        public T GetActor<T>(Predicate<T> predicate)
            where T : RolePlayActor
        {
            return m_actors.OfType<T>().FirstOrDefault(entry => predicate(entry));
        }

        public IEnumerable<T> GetActors<T>()
        {
            return m_actors.OfType<T>();
        }

        public IEnumerable<T> GetActors<T>(Predicate<T> predicate)
        {
            return m_actors.OfType<T>().Where(entry => predicate(entry));
        }

        public IEnumerable<WorldObjectItem> GetObjectItems()
        {
            return m_objectItems;
        }

        public WorldObjectItem GetObjectItem(short cell)
        {
            return GetObjectItems().FirstOrDefault(x => x.Cell.Id == cell);
        }

        public IEnumerable<WorldObjectItem> GetObjectItems(Predicate<WorldObjectItem> predicate)
        {
            return m_objectItems.Where(entry => predicate(entry));
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

        public IEnumerable<MapObstacle> GetMapObstacles()
        {
            var mapObstacles = new List<MapObstacle>();

            foreach (var trigger in GetTriggers().Where(x => x is AnimateTrigger))
            {
                mapObstacles.AddRange(((AnimateTrigger)trigger).Obstacles);
            }

            foreach (var skill in GetInteractiveObjects().SelectMany(x => x.GetSkills()).Where(skill => skill is SkillAnimate))
            {
                mapObstacles.AddRange(((SkillAnimate)skill).Obstacles);
            }

            return mapObstacles.OrderByDescending(x => x.state == (sbyte)MapObstacleStateEnum.OBSTACLE_OPENED).DistinctBy(x => x.obstacleCellId).ToList();
        }

        public void MoveCharactersToWalkableCell()
        {
            foreach (var character in GetAllCharacters())
            {
                if (IsCellWalkable(character.Cell))
                    continue;

                var cell = GetCell(character.Position.Point.GetCellInDirection(DirectionsEnum.DIRECTION_SOUTH_WEST, 1).CellId);

                if (!cell.Walkable)
                {
                    var adjacents = character.Position.Point.GetAdjacentCells(entry => GetCell(entry).Walkable).ToArray();

                    if (adjacents.Length == 0)
                        cell = GetFirstFreeCellNearMiddle();

                    cell = GetCell(adjacents[0].CellId);
                }

                character.Teleport(this, cell);
            }
        }

        public bool IsCellWalkable(Cell cell)
        {
            return cell.Walkable || GetMapObstacles().Any(x => x.obstacleCellId == cell.Id && x.state == (sbyte)MapObstacleStateEnum.OBSTACLE_OPENED);
        }

        public InteractiveObject GetInteractiveObject(int id)
        {
            return !m_interactives.ContainsKey(id) ? null : m_interactives[id];
        }

        public IEnumerable<InteractiveObject> GetInteractiveObjects()
        {
            return m_interactives.Values;
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

        public Cell GetRandomFreeCell(bool actorFree = false, bool nearMiddle = false)
        {
            var rand = new AsyncRandom();

            if (!actorFree)
                return m_freeCells[rand.Next(0, m_freeCells.Length)];

            var excludedCells = GetActors<RolePlayActor>().Select(entry => entry.Cell.Id);
            var cells = m_freeCells.Where(entry => !excludedCells.Contains(entry.Id)).ToArray();

            return cells[rand.Next(0, cells.Length)];
        }

        public Cell GetFirstFreeCellNearMiddle()
        {
            return m_freeCells.FirstOrDefault();
        }

        public Cell GetRandomAdjacentFreeCell(MapPoint cell, bool actorFree = false)
        {
            if (actorFree)
            {
                var excludedCells = GetActors<RolePlayActor>().Select(entry => entry.Cell.Id);
                var cells = cell.GetAdjacentCells(entry => CellsInfoProvider.IsCellWalkable(entry) && !excludedCells.Contains(entry));

                var pt = cells.RandomElementOrDefault();

                return pt != null ? Cells[pt.CellId] : null;
            }
            else
            {
                var cells = cell.GetAdjacentCells(CellsInfoProvider.IsCellWalkable);

                var pt = cells.RandomElementOrDefault();

                return pt != null ? Cells[pt.CellId] : null;
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
                    return Record.TopNeighbourCellId ?? (short)(currentCell + 532);
                case MapNeighbour.Bottom:
                    return Record.BottomNeighbourCellId ?? (short)(currentCell - 532);
                case MapNeighbour.Right:
                    return Record.RightNeighbourCellId ?? (short)(currentCell - 13);
                case MapNeighbour.Left:
                    return Record.LeftNeighbourCellId ?? (short)(currentCell + 13);
                default:
                    return 0;
            }
        }

        #endregion

        #endregion

        public bool Equals(Map other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return ReferenceEquals(this, other) || Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Map) && Equals((Map)obj);
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

        private static void InitializeValidators()
        {
            // for later
        }

        #region MapComplementaryInformationsDataMessage

        public MapComplementaryInformationsDataMessage GetMapComplementaryInformationsDataMessage(Character character)
        {
            return new MapComplementaryInformationsWithCoordsMessage(
                (short)SubArea.Id,
                Id,
                new HouseInformations[0],
                m_actors.Where(entry => entry.CanBeSee(character)).Select(entry => entry.GetGameContextActorInformations(character) as GameRolePlayActorInformations),
                m_interactives.Where(entry => entry.Value.CanBeSee(character)).Select(entry => entry.Value.GetInteractiveElementWithBonus(character)),
                m_interactives.Where(entry => entry.Value.CanBeSee(character)).Where(x => x.Value.Animated).Select(entry => entry.Value.GetStatedElement()),
                GetMapObstacles(),
                m_fights.Where(entry => entry.BladesVisible).Select(entry => entry.GetFightCommonInformations()),
                false,
                (short)Position.X,
                (short)Position.Y);
        }

        #endregion

        #endregion

        public bool IsMerchantLimitReached()
        {
            return m_actors.OfType<Merchant>().Count(x => !x.IsBagEmpty()) >= MaxMerchantsPerMap;
        }

        public bool ToggleMute()
        {
            return IsMuted = !IsMuted;
        }

        public bool IsAnyDroppedItems()
        {
            return m_objectItems.Any();
        }
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