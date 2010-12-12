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

            for (short i = 0; i < 560; i++)
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
    public class Map : WorldSpace
    {
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

        public void HandleCharacterMovement(Character character, List<uint> keymovements)
        {
            Action<Character> action = (Character charac) =>
            {
                ContextHandler.SendGameMapMovementMessage(charac.Client, keymovements, character);
                BasicHandler.SendBasicNoOperationMessage(charac.Client);
            };

            CallOnAllCharactersWithoutFighters(action);
        }

        public override void OnMonsterSpawning()
        {
            // hey we don't have childrens :D
            // Lets spawn those monsters.
//             MonsterGroup monsters = Monster.GenerateGroup((MapId)this.Id);
//             foreach (GroupMember member in monsters.Members)
//             {
//                 OnEnter(member.Entity);
//             }
        }

        public override void OnEnter(Entity entity)
        {
            base.OnEnter(entity);
            ParentSpace.OnEnter(entity);

            Action<Character> action =
                charac => ContextHandler.SendGameRolePlayShowActorMessage(charac.Client, entity);

            CallOnAllCharactersWithoutFighters(action);
        }

        public override void OnLeave(Entity entity)
        {
            base.OnLeave(entity);
            ParentSpace.OnLeave(entity);

            Action<Character> action =
                charac => ContextHandler.SendGameContextRemoveElementMessage(charac.Client, entity);

            CallOnAllCharactersWithoutFighters(action);

            if (entity is Character)
            {
                (entity as Character).NextMap = null;
            }
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
            return m_mapsAround[mapid];
        }

        /// <summary>
        ///   Calculate which cell our character will walk on once map changed.
        /// </summary>
        public int GetCellAfterChangeMap(int currentCell, MapNeighbour mapneighbour)
        {
            int cell = 0;

            switch (mapneighbour)
            {
                case MapNeighbour.Top:
                {
                    cell = currentCell + 532;
                    break;
                }
                case MapNeighbour.Bottom:
                {
                    cell = currentCell - 532;
                    break;
                }
                case MapNeighbour.Right:
                {
                    cell = currentCell - 13;
                    break;
                }
                case MapNeighbour.Left:
                {
                    cell = currentCell + 13;
                    break;
                }
            }
            return cell;
        }

        public CellData GetCell(int index)
        {
            if (index < 0 || index > 560)
                throw new Exception("Index en dehors des limites : " + index);

            return CellsData[index];
        }

        public List<Character> GetAllCharactersWithoutFighters()
        {
            return
                Entities.Values.Where(entity => entity is Character && !entity.IsInFight).Cast<Character>().ToList();
        }


        /// <summary>
        ///   Execute an action of every characters in this world space included fight's members.
        /// </summary>
        /// <param name = "action"></param>
        public void CallOnAllCharactersWithoutFighters(Action<Character> action)
        {
            List<Character> chars = GetAllCharactersWithoutFighters();

            Parallel.For(0, chars.Count, i => action(chars[i]));
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