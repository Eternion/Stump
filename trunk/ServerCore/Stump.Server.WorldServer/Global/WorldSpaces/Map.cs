
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.Database.Data.World;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Global.WorldSpaces;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Npcs;
using Stump.Server.WorldServer.Skills;

namespace Stump.Server.WorldServer.Global.Maps
{
    public static class MapExtensions
    {
        public static void WriteMap(this BigEndianWriter writer, Map map)
        {
            writer.WriteByte(0x96);
            writer.WriteByte((byte) map.Version);
            writer.WriteInt(map.Id);
            writer.WriteInt((int) map.RelativeId);

            writer.WriteByte((byte) map.MapType);
            writer.WriteInt(map.ParentSpace.Id);

            writer.WriteInt(map.TopNeighbourId);
            writer.WriteInt(map.BottomNeighbourId);
            writer.WriteInt(map.LeftNeighbourId);
            writer.WriteInt(map.RightNeighbourId);

            writer.WriteInt(map.ShadowBonusOnEntities);

            writer.WriteBoolean(map.UseLowpassFilter);

            writer.WriteBoolean(map.UseReverb);

            if (map.UseReverb)
            {
                writer.WriteInt(map.PresetId);
            }

            foreach (CellLinked cell in map.Cells)
            {
                writer.WriteCell(cell);
            }

            writer.WriteInt(map.MapElementsPositions.Count);
            foreach (var elementsPosition in map.MapElementsPositions)
            {
                writer.WriteUInt(elementsPosition.Key);
                writer.WriteUShort(elementsPosition.Value.Id);
            }
        }

        public static Map ReadMap(this BigEndianReader reader)
        {
            int header = reader.ReadByte();

            if (header != 0x96)
                throw new FileLoadException("Wrong header file");

            var map = new Map
                          {
                              Version = reader.ReadByte(),
                              Id = reader.ReadInt(),
                              RelativeId = (uint) reader.ReadInt(),
                              MapType = reader.ReadByte(),
                              ZoneId = (uint) reader.ReadInt(),
                              TopNeighbourId = reader.ReadInt(),
                              BottomNeighbourId = reader.ReadInt(),
                              LeftNeighbourId = reader.ReadInt(),
                              RightNeighbourId = reader.ReadInt(),
                              ShadowBonusOnEntities = reader.ReadInt(),
                              UseLowpassFilter = reader.ReadByte() == 1,
                              UseReverb = reader.ReadByte() == 1
                          };

            map.PresetId = map.UseReverb ? reader.ReadInt() : -1;

            for (ushort i = 0; i < Map.MaximumCellsCount; i++)
            {
                CellLinked celldata = reader.ReadCell();
                celldata.Id = i;
                celldata.ParrentMap = map;

                map.Cells.Add(celldata);
            }

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                uint key = reader.ReadUInt();
                ushort cell = reader.ReadUShort();

                // objects can be superposed, so we ignore it
                if (map.MapElementsPositions.ContainsKey(key) && map.MapElementsPositions[key].Id == cell)
                    continue;

                if (key > 0)
                    map.MapElementsPositions.Add(key, map.GetCell(cell));
            }

            map.InitializeMapArrounds();

            return map;
        }
    }

    /// <summary>
    ///   Represents a map where entities can walk for instance.
    /// </summary>
    public partial class Map : IContext
    {
        public const uint MaximumCellsCount = 560;

        private readonly Stack<int> m_freeContextualIds = new Stack<int>();
        private readonly Dictionary<int, MapNeighbour> m_mapsAround = new Dictionary<int, MapNeighbour>();

        private int m_nextContextualId;

        #region Constructors

        public Map(MapRecord record, Zone parent)
        {
            Cells = new List<CellLinked>();
            Triggers = new Dictionary<CellLinked, CellTrigger>();
            MapElementsPositions = new Dictionary<uint, CellLinked>();
            InteractiveObjects = new Dictionary<uint, InteractiveObject>();

            NonFighters = new ConcurrentDictionary<long, Character>();

            Record = record;

            Parent = parent;
            Parent.Childrens.Add(this);

            Cells.AddRange(from cell in record.Cells
                           select new CellLinked(this, ref cell));

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

        public Zone Parent
        {
            get;
            private set;
        }
        public ContextType ContextType
        {
            get { return ContextType.RolePlay; }
        }

        public MapRecord Record
        {
            get;
            set;
        }

        /// <summary>
        ///   Map version of this map.
        /// </summary>
        public uint Version
        {
            get { return Record.Version; }
            set { Record.Version = value; }
        }

        /// <summary>
        ///   Relative id of this map.
        /// </summary>
        public uint RelativeId
        {
            get { return Record.RelativeId; }
            set { Record.RelativeId = value; }
        }

        /// <summary>
        ///   Type of this map.
        /// </summary>
        public int MapType
        {
            get { return Record.MapType; }
            set { Record.MapType = value; }
        }

        /// <summary>
        ///   Zone Id which owns this map.
        /// </summary>
        public uint ZoneId
        {
            get { return Record.ZoneId; }
            set { Record.ZoneId = value; }
        }

        public Point Position
        {
            get { return Record.Position.Pos; }
            set { Record.Position.Pos = value; }
        }

        public bool Outdoor
        {
            get { return Record.Outdoor; }
            set { Record.Outdoor = value; }
        }

        public int TopNeighbourId
        {
            get { return Record.TopNeighbourId; }
            set { Record.TopNeighbourId = value; }
        }

        public int BottomNeighbourId
        {
            get { return Record.BottomNeighbourId; }
            set { Record.BottomNeighbourId = value; }
        }

        public int LeftNeighbourId
        {
            get { return Record.LeftNeighbourId; }
            set { Record.LeftNeighbourId = value; }
        }

        public int RightNeighbourId
        {
            get { return Record.RightNeighbourId; }
            set { Record.RightNeighbourId = value; }
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
            set { Record.PresetId = value; }
        }

        public ConcurrentDictionary<long, NpcSpawn> Npcs
        {
            get;
            private set;
        }

        public ConcurrentDictionary<long, Character> NonFighters
        {
            get;
            private set;
        }

        internal Dictionary<uint, CellLinked> MapElementsPositions
        {
            get;
            private set;
        }

        public Dictionary<uint, InteractiveObject> InteractiveObjects
        {
            get;
            private set;
        }

        public Dictionary<CellLinked, CellTrigger> Triggers
        {
            get;
            private set;
        }

        public List<CellLinked> Cells
        {
            get;
            private set;
        }

        #endregion

        #region Spawn Methods
        public void SpawnNpc(GameRolePlayNpcInformations npcInformations)
        {
            NpcTemplate template = NpcManager.GetTemplate((int)npcInformations.npcId);

            if (template == null)
                throw new Exception(string.Format("NPC Template <id:{0}> doesn't exists", npcInformations.npcId));

            var npcSpawn = new NpcSpawn(
                this,
                template,
                PopNextContextualId(),
                new ObjectPosition(this, npcInformations.disposition),
                npcInformations.sex,
                (int)npcInformations.specialArtworkId,
                npcInformations.look);

            AddEntity(npcSpawn);
        }

        public void SpawnInteractiveObject(InteractiveElement interactiveElement)
        {
            var interactiveObject = new InteractiveObject(interactiveElement.elementId, interactiveElement.elementTypeId,
                                                          new Dictionary<uint, SkillBase>(),
                                                          MapElementsPositions[interactiveElement.elementId]);

            InteractiveObjects.Add(interactiveElement.elementId, interactiveObject);
        }

        public void SpawnTrigger(CellLinked cell, CellTrigger trigger)
        {
            Triggers.Add(cell, trigger);

            trigger.StartTrigger();
        }

        public int PopNextContextualId()
        {
            if (m_freeContextualIds.Count > 0)
                return m_freeContextualIds.Pop();

            Interlocked.Decrement(ref m_nextContextualId);

            return m_nextContextualId;
        }

        public override void OnMonsterSpawning()
        {
            // hey we don't have childrens :D
            // Lets spawn those monsters.
//             MonsterGroup monsters = Monster.GenerateGroup((MapId)this.Id);
//             foreach (GroupMember member in monsters.Members)
//             {
//                 AddEntity(member.Entity);
//             }
        }

        #endregion

        #region Get Methods

        public MapNeighbour GetMapNeighbourByMapid(int mapid)
        {
            if (!m_mapsAround.ContainsKey(mapid))
                return MapNeighbour.None;

            return m_mapsAround[mapid];
        }

        /// <summary>
        ///   Calculate which cell our character will walk on once map changed.
        /// </summary>
        public ushort GetCellAfterChangeMap(ushort currentCell, MapNeighbour mapneighbour)
        {
            ushort cell = 0;

            switch (mapneighbour)
            {
                case MapNeighbour.Top:
                    {
                        cell = (ushort) (currentCell + 532);
                        break;
                    }
                case MapNeighbour.Bottom:
                    {
                        cell = (ushort) (currentCell - 532);
                        break;
                    }
                case MapNeighbour.Right:
                    {
                        cell = (ushort) (currentCell - 13);
                        break;
                    }
                case MapNeighbour.Left:
                    {
                        cell = (ushort) (currentCell + 13);
                        break;
                    }
            }
            return cell;
        }

        public CellLinked GetCell(int index)
        {
            if (index < 0 || index >= MaximumCellsCount)
                throw new Exception("Index out of bounds : " + index);

            return Cells[index];
        }

        public InteractiveObject GetInteractiveObject(uint elementId)
        {
            return InteractiveObjects.ContainsKey(elementId) ? InteractiveObjects[elementId] : null;
        }

        #endregion

        #region IContext Members

        /// <summary>
        ///   Execute an action with every characters in this world space without fight's members.
        /// </summary>
        /// <param name = "action"></param>
        public override void Do(Action<Character> action)
        {
            Parallel.ForEach(NonFighters.Values, action);
        }

        public void DoWithFighters(Action<Character> action)
        {
            Parallel.ForEach(Characters.Values, action);
        }

        #endregion

        #region Entity Add/Remove

        public override void AddEntity(Entity entity)
        {
            base.AddEntity(entity);

            Action<Character> action =
                charac => ContextHandler.SendGameRolePlayShowActorMessage(charac.Client, entity);

            Do(action);

            NotifySpawnedEntity(entity);
        }

        public override void RemoveEntity(Entity entity)
        {
            base.RemoveEntity(entity);

            Action<Character> action =
                charac => ContextHandler.SendGameContextRemoveElementMessage(charac.Client, entity);

            Do(action);

            NotifyUnSpawnedEntity(entity);
        }

        protected override void OnEntityAdded(Entity entity)
        {
            base.OnEntityAdded(entity);

            if (entity is Character)
                if (!NonFighters.TryAdd(entity.Id, entity as Character))
                {
                    throw new Exception("Couldn't add character as nonfighter in map");
                }

            if (entity is NpcSpawn)
                if (!Npcs.TryAdd(entity.Id, entity as NpcSpawn))
                {
                    throw new Exception("Couldn't add npc in world space");
                }

            if (entity is LivingEntity)
            {
                (entity as LivingEntity).EntityMovingStart += OnEntityMovingStart;
                (entity as LivingEntity).EntityMovingEnd += OnEntityMovingEnd;
                (entity as LivingEntity).EntityEnterFight += OnEntityEnterFight;
                (entity as LivingEntity).EntityLeaveFight += OnEntityLeaveFight;
            }
        }

        protected override void OnEntityRemoved(Entity entity)
        {
            base.OnEntityRemoved(entity);

            if (entity is Character)
            {
                Character outvalue;
                if (!NonFighters.TryRemove(entity.Id, out outvalue))
                {
                    throw new Exception("Couldn't remove character as nonfighter in map");
                }
            }

            if (entity is NpcSpawn)
            {
                NpcSpawn removedNpc;
                if (!Npcs.TryRemove(entity.Id, out removedNpc))
                {
                    throw new Exception("Couldn't remove npc in world space");
                }
            }

            if (entity is LivingEntity)
            {
                (entity as LivingEntity).EntityMovingStart -= OnEntityMovingStart;
                (entity as LivingEntity).EntityMovingEnd -= OnEntityMovingEnd;
                (entity as LivingEntity).EntityEnterFight -= OnEntityEnterFight;
                (entity as LivingEntity).EntityLeaveFight -= OnEntityLeaveFight;
            }
        }

        private void OnEntityEnterFight(LivingEntity entity, Fight fight)
        {
            entity.EntityMovingStart -= OnEntityMovingStart;
            entity.EntityMovingEnd -= OnEntityMovingEnd;

            if (entity is Character)
            {
                Character outvalue;
                if (!NonFighters.TryRemove(entity.Id, out outvalue))
                {
                    throw new Exception("Couldn't remove character as nonfighter in map");
                }
            }
        }

        private void OnEntityLeaveFight(LivingEntity entity, Fight fight)
        {
            entity.EntityMovingStart += OnEntityMovingStart;
            entity.EntityMovingEnd += OnEntityMovingEnd;

            if (entity is Character)
                if (!NonFighters.TryAdd(entity.Id, entity as Character))
                {
                    throw new Exception("Couldn't add character as nonfighter in map");
                }
        }

        public void OnFightEnter(Entity entity)
        {
            Action<Character> action =
                charac => ContextHandler.SendGameContextRemoveElementMessage(charac.Client, entity);

            Do(action);
        }

        public void OnFightLeave(Entity entity)
        {
            Action<Character> action =
                charac => ContextHandler.SendGameRolePlayShowActorMessage(charac.Client, entity);

            Do(action);
        }

        #endregion

        #region Movements

        private void OnEntityMovingStart(LivingEntity entity, MovementPath movementPath)
        {
            movementPath.Compress();
            List<uint> movementsKey = movementPath.GetServerMovementKeys();

            Action<Character> action = charac =>
                                           {
                                               ContextHandler.SendGameMapMovementMessage(charac.Client, movementsKey,
                                                                                         entity);
                                               BasicHandler.SendBasicNoOperationMessage(charac.Client);
                                           };

            Do(action);
        }

        private void OnEntityMovingEnd(LivingEntity entity, MovementPath movementPath)
        {
            /* check cell trigger, monsters... */

            if (entity is Character)
                movementPath.End.Cell.NotifyCellReached(entity as Character);
        }

        #endregion

        #region Equality Operators

        public static bool operator ==(Map map1, Map map2)
        {
            return ReferenceEquals(map1, map2);
        }

        public static bool operator !=(Map map1, Map map2)
        {
            return !(map1 == map2);
        }

        public bool Equals(Map other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
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
            return Id.GetHashCode();
        }

        #endregion
    }
}