// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Npcs;
using Stump.Server.WorldServer.Skills;
using Point = System.Drawing.Point;

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

            foreach (CellData cell in map.CellsData)
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
                CellData celldata = reader.ReadCell();
                celldata.Id = i;
                celldata.ParrentMap = map;

                map.CellsData.Add(celldata);
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
    public partial class Map : WorldSpace
    {
        public const uint MaximumCellsCount = 560;

        private readonly Stack<int> m_freeContextualIds = new Stack<int>();
        private readonly Dictionary<int, MapNeighbour> m_mapsAround;

        private int m_nextContextualId;

        /// <summary>
        ///   Constructor
        /// </summary>
        public Map()
        {
            m_mapsAround = new Dictionary<int, MapNeighbour>();

            CellsData = new List<CellData>();
            Triggers = new Dictionary<CellData, CellTrigger>();
            MapElementsPositions = new Dictionary<uint, CellData>();
            InteractiveObjects = new Dictionary<uint, InteractiveObject>();

            NonFighters = new ConcurrentDictionary<long, Character>();
        }

        public void InitializeMapArrounds()
        {
            m_mapsAround.Add(TopNeighbourId, MapNeighbour.Top);
            m_mapsAround.Add(BottomNeighbourId, MapNeighbour.Bottom);
            m_mapsAround.Add(LeftNeighbourId, MapNeighbour.Left);
            m_mapsAround.Add(RightNeighbourId, MapNeighbour.Right);
        }

        public int PopNextContextualId()
        {
            if (m_freeContextualIds.Count > 0)
                return m_freeContextualIds.Pop();

            Interlocked.Decrement(ref m_nextContextualId);

            return m_nextContextualId;
        }

        public void SpawnNpc(GameRolePlayNpcInformations npcInformations)
        {
            NpcTemplate template = NpcManager.GetTemplate((int) npcInformations.npcId);

            if (template == null)
                throw new Exception(string.Format("NPC Template <id:{0}> doesn't exists", npcInformations.npcId));

            var npcSpawn = new NpcSpawn(
                this,
                template,
                PopNextContextualId(),
                new VectorIsometric(this, npcInformations.disposition),
                npcInformations.sex,
                (int) npcInformations.specialArtworkId,
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

        public override void AddEntity(Entity entity)
        {
            base.AddEntity(entity);
            ParentSpace.AddEntity(entity);

            Action<Character> action =
                charac => ContextHandler.SendGameRolePlayShowActorMessage(charac.Client, entity);

            CallOnAllCharacters(action);

            NotifySpawnedEntity(entity);
        }

        public override void RemoveEntity(Entity entity)
        {
            base.RemoveEntity(entity);
            ParentSpace.RemoveEntity(entity);

            Action<Character> action =
                charac => ContextHandler.SendGameContextRemoveElementMessage(charac.Client, entity);

            CallOnAllCharacters(action);

            if (entity is Character)
            {
                (entity as Character).NextMap = null;
            }

            NotifyUnSpawnedEntity(entity);
        }

        public void OnFightEnter(Entity entity)
        {
            Action<Character> action =
                charac => ContextHandler.SendGameContextRemoveElementMessage(charac.Client, entity);

            CallOnAllCharacters(action);
        }

        public void OnFightLeave(Entity entity)
        {
            Action<Character> action =
                charac => ContextHandler.SendGameRolePlayShowActorMessage(charac.Client, entity);

            CallOnAllCharacters(action);
        }

        public void AddTrigger(CellData cell, CellTrigger trigger)
        {
            Triggers.Add(cell, trigger);

            trigger.StartTrigger();
        }

        public void SetMapPosition(MapPosition mapPosition)
        {
            Position = new Point(mapPosition.posX, mapPosition.posY);
            Outdoor = mapPosition.outdoor;
        }

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

        public CellData GetCell(int index)
        {
            if (index < 0 || index >= MaximumCellsCount)
                throw new Exception("Index out of bounds : " + index);

            return CellsData[index];
        }

        public InteractiveObject GetInteractiveObject(uint elementId)
        {
            return InteractiveObjects.ContainsKey(elementId) ? InteractiveObjects[elementId] : null;
        }

        /// <summary>
        ///   Execute an action with every characters in this world space without fight's members.
        /// </summary>
        /// <param name = "action"></param>
        public override void CallOnAllCharacters(Action<Character> action)
        {
            Parallel.ForEach(NonFighters.Values, action);
        }

        public void CallOnAllCharactersWithFighters(Action<Character> action)
        {
            Parallel.ForEach(Characters.Values, action);
        }

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

        #region Properties

        public override WorldSpaceType Type
        {
            get { return WorldSpaceType.Map; }
        }

        public override ContextType ContextType
        {
            get { return ContextType.Map; }
        }

        /// <summary>
        ///   Map version of this map.
        /// </summary>
        public uint Version
        {
            get;
            internal set;
        }

        /// <summary>
        ///   Relative id of this map.
        /// </summary>
        public uint RelativeId
        {
            get;
            internal set;
        }

        /// <summary>
        ///   Type of this map.
        /// </summary>
        public int MapType
        {
            get;
            internal set;
        }

        /// <summary>
        ///   Zone Id which owns this map.
        /// </summary>
        public uint ZoneId
        {
            get;
            internal set;
        }

        public Point Position
        {
            get;
            private set;
        }

        public bool Outdoor
        {
            get;
            internal set;
        }

        public int TopNeighbourId
        {
            get;
            internal set;
        }

        public int BottomNeighbourId
        {
            get;
            internal set;
        }

        public int LeftNeighbourId
        {
            get;
            internal set;
        }

        public int RightNeighbourId
        {
            get;
            internal set;
        }

        public int ShadowBonusOnEntities
        {
            get;
            internal set;
        }

        public bool UseLowpassFilter
        {
            get;
            internal set;
        }

        public bool UseReverb
        {
            get;
            internal set;
        }

        public int PresetId
        {
            get;
            internal set;
        }

        public ConcurrentDictionary<long, Character> NonFighters
        {
            get;
            private set;
        }

        internal Dictionary<uint, CellData> MapElementsPositions
        {
            get;
            private set;
        }

        public Dictionary<uint, InteractiveObject> InteractiveObjects
        {
            get;
            private set;
        }

        public Dictionary<CellData, CellTrigger> Triggers
        {
            get;
            private set;
        }

        public List<CellData> CellsData
        {
            get;
            private set;
        }

        #endregion

        #region Entity Added/Removed

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

            if (entity is LivingEntity)
            {
                (entity as LivingEntity).EntityMovingStart -= OnEntityMovingStart;
                (entity as LivingEntity).EntityMovingEnd -= OnEntityMovingEnd;
                (entity as LivingEntity).EntityEnterFight -= OnEntityEnterFight;
                (entity as LivingEntity).EntityLeaveFight -= OnEntityLeaveFight;
            }
        }

        protected override void OnEntityAdded(Entity entity)
        {
            base.OnEntityAdded(entity);

            if (entity is Character)
                if (!NonFighters.TryAdd(entity.Id, entity as Character))
                {
                    throw new Exception("Couldn't add character as nonfighter in map");
                }

            if (entity is LivingEntity)
            {
                (entity as LivingEntity).EntityMovingStart += OnEntityMovingStart;
                (entity as LivingEntity).EntityMovingEnd += OnEntityMovingEnd;
                (entity as LivingEntity).EntityEnterFight += OnEntityEnterFight;
                (entity as LivingEntity).EntityLeaveFight += OnEntityLeaveFight;
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

        #endregion

        #region Movements

        private void OnEntityMovingStart(LivingEntity entity, MovementPath movementPath)
        {
            movementPath.Compress();
            List<uint> movementsKey = movementPath.GetServerMovementKeys();

            Action<Character> action = charac =>
            {
                ContextHandler.SendGameMapMovementMessage(charac.Client, movementsKey, entity);
                BasicHandler.SendBasicNoOperationMessage(charac.Client);
            };

            CallOnAllCharacters(action);
        }

        private void OnEntityMovingEnd(LivingEntity entity, MovementPath movementPath)
        {
            /* check cell trigger, monsters... */

            if (entity is Character)
                movementPath.End.Cell.NotifyCellReached(entity as Character);
        }

        #endregion
    }
}