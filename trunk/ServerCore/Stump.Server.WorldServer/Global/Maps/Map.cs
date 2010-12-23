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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Global.Maps
{
    public static class MapExtensions
    {
        public static void WriteMap(this BigEndianWriter writer, Map map)
        {
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
        }

        public static Map ReadMap(this BigEndianReader reader)
        {
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

            map.InitializeMapArrounds();

            return map;
        }
    }

    /// <summary>
    ///   Represents a map where entities can walk for instance.
    /// </summary>
    // todo : seperate SpawnEntries and Characters ?
    public partial class Map : WorldSpace
    {
        public const uint MaximumCellsCount = 560;

        #region Fields

        private readonly Dictionary<int, MapNeighbour> m_mapsAround;

        #endregion

        /// <summary>
        ///   Constructor
        /// </summary>
        public Map()
        {
            CellsData = new List<CellData>();
            m_mapsAround = new Dictionary<int, MapNeighbour>();

            EntityAdded += WorldSpaceEntityAdded;
            EntityRemoved += WorldSpaceEntityRemoved;
        }

        public override WorldSpaceType Type
        {
            get { return WorldSpaceType.Map; }
        }

        public void InitializeMapArrounds()
        {
            m_mapsAround.Add(TopNeighbourId, MapNeighbour.Top);
            m_mapsAround.Add(BottomNeighbourId, MapNeighbour.Bottom);
            m_mapsAround.Add(LeftNeighbourId, MapNeighbour.Left);
            m_mapsAround.Add(RightNeighbourId, MapNeighbour.Right);
        }

        private void WorldSpaceEntityRemoved(Entity entity)
        {
            if (entity is LivingEntity)
            {
                (entity as LivingEntity).EntityMovingStart -= EntityMovingStart;
                (entity as LivingEntity).EntityMovingEnd -= EntityMovingEnd;
            }
        }

        private void WorldSpaceEntityAdded(Entity entity)
        {
            if (entity is LivingEntity)
            {
                (entity as LivingEntity).EntityMovingStart += EntityMovingStart;
                (entity as LivingEntity).EntityMovingEnd += EntityMovingEnd;
            }
        }

        private void EntityMovingStart(LivingEntity entity, MovementPath movementPath)
        {
            var movementsKey = MapMovementAdapter.GetServerMovement(movementPath);

            Action<Character> action = charac =>
            {
                ContextHandler.SendGameMapMovementMessage(charac.Client, movementsKey, entity);
                BasicHandler.SendBasicNoOperationMessage(charac.Client);
            };

            CallOnAllCharactersWithoutFighters(action);
        }

        private void EntityMovingEnd(LivingEntity entity, MovementPath movementPath)
        {
            /* check cell trigger, monsters... */
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

            CallOnAllCharactersWithoutFighters(action);

            NotifySpawnedEntity(entity);
        }

        public override void RemoveEntity(Entity entity)
        {
            base.RemoveEntity(entity);
            ParentSpace.RemoveEntity(entity);

            Action<Character> action =
                charac => ContextHandler.SendGameContextRemoveElementMessage(charac.Client, entity);

            CallOnAllCharactersWithoutFighters(action);

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

            CallOnAllCharactersWithoutFighters(action);
        }

        public void OnFightLeave(Entity entity)
        {
            Action<Character> action =
                charac => ContextHandler.SendGameRolePlayShowActorMessage(charac.Client, entity);

            CallOnAllCharactersWithoutFighters(action);
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

        public IEnumerable<Character> CharactersWithoutFighters
        {
            get
            {
                return
                    Entities.Values.OfType<Character>().Where(entity => !entity.IsInFight);
            }
        }


        /// <summary>
        ///   Execute an action of every characters in this world space included fight's members.
        /// </summary>
        /// <param name = "action"></param>
        public void CallOnAllCharactersWithoutFighters(Action<Character> action)
        {
            Parallel.ForEach(CharactersWithoutFighters, action);
        }

        public static bool operator ==(Map map1, Map map2)
        {
            return ReferenceEquals(map1, map2);
        }

        public static bool operator !=(Map map1, Map map2)
        {
            return !(map1 == map2);
        }

        #region Properties

        /// <summary>
        ///   Map version of this map.
        /// </summary>
        public uint Version
        {
            get;
            set;
        }

        /// <summary>
        ///   Relative id of this map.
        /// </summary>
        public uint RelativeId
        {
            get;
            set;
        }

        /// <summary>
        ///   Type of this map.
        /// </summary>
        public int MapType
        {
            get;
            set;
        }

        /// <summary>
        ///   Zone Id which owns this map.
        /// </summary>
        public uint ZoneId
        {
            get;
            set;
        }

        public int TopNeighbourId
        {
            get;
            set;
        }

        public int BottomNeighbourId
        {
            get;
            set;
        }

        public int LeftNeighbourId
        {
            get;
            set;
        }

        public int RightNeighbourId
        {
            get;
            set;
        }

        public int ShadowBonusOnEntities
        {
            get;
            set;
        }

        public bool UseLowpassFilter
        {
            get;
            set;
        }

        public bool UseReverb
        {
            get;
            set;
        }

        public int PresetId
        {
            get;
            set;
        }

        public List<CellData> CellsData
        {
            get;
            set;
        }

        #endregion
    }
}