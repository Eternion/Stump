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
using System.Globalization;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public class Entity : IEntity
    {
        #region Fields

        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Name of the character.
        /// </summary>
        private string m_name;

        #endregion

        /// <summary>
        ///   Constructor
        /// </summary>
        protected Entity()
        {
            BaseHealth = 0;
            DamageTaken = 0;
            Colors = new List<int>();
            Skins = new List<short>();
        }

        public virtual void OnCreate()
        {
        }

        public virtual void OnDamage()
        {
        }

        public virtual void OnDeath()
        {
        }

        public virtual bool IsVisible()
        {
            return true;
        }

        public virtual EntityLook ToNetworkEntityLook()
        {
            return new EntityLook(
                1, // bones id
                Skins.Cast<uint>().ToList(),
                ColorsIndexed,
                Scales,
                new List<SubEntity>());
        }

        public virtual GameRolePlayActorInformations ToNetworkActor()
        {
            return new GameRolePlayActorInformations((int) Id,
                                                     ToNetworkEntityLook(),
                                                     GetEntityDisposition());
        }

        public virtual EntityDispositionInformations GetEntityDisposition()
        {
            return new EntityDispositionInformations(CellId, (uint) Direction);
        }

        #region Properties

        /// <summary>
        ///   Set or get Level of the character.
        /// </summary>
        public int Level
        {
            get;
            set;
        }

        public int BonesId
        {
            get;
            set;
        }

        public Map Map
        {
            get;
            set;
        }

        public Zone Zone
        {
            get;
            set;
        }

        public Region Region
        {
            get;
            set;
        }

        /// <summary>
        ///   The name of this character.
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set
            {
                if (InWorld)
                    throw new NotImplementedException("Dynamic renaming of Characters is not implemented.");

                m_name = value;
            }
        }

        /// <summary>
        ///   Indicate or set if this character is currently in world.
        /// </summary>
        public bool InWorld
        {
            get;
            set;
        }

        public List<short> Skins
        {
            get;
            set;
        }

        public List<int> Colors
        {
            get;
            set;
        }

        public List<int> ColorsIndexed
        {
            get
            {
                return
                    Colors.Select(
                        (color, index) => int.Parse((index + 1) + color.ToString("X6"), NumberStyles.HexNumber)).ToList();
            }
        }

        public int Scale
        {
            get;
            set;
        }

        public List<int> Scales
        {
            get { return new List<int> {Scale}; }
        }

        public int Direction
        {
            get;
            set;
        }

        public int CellId
        {
            get;
            set;
        }

        public int BaseHealth
        {
            get;
            set;
        }

        public int DamageTaken
        {
            get;
            set;
        }

        public StatsFields Stats
        {
            get;
            protected set;
        }

        /// <summary>
        ///   Spell container of this entity.
        /// </summary>
        public SpellCollection Spells
        {
            get;
            protected set;
        }

        public GroupMember GroupMember
        {
            get;
            set;
        }

        /// <summary>
        ///   Indicate if the character is in a group.
        /// </summary>
        public bool IsInGroup
        {
            get { return GroupMember != null; }
        }

        public bool IsInFight
        {
            get
            {
                return GroupMember != null && GroupMember is FightGroupMember &&
                       (((FightGroup) (GroupMember as FightGroupMember).GroupOwner).Fight.FightState ==
                        FightState.Fighting ||
                        ((GroupMember as FightGroupMember).GroupOwner as FightGroup).Fight.FightState ==
                        FightState.PreparePosition);
            }
        }

        public Fight CurrentFight
        {
            get { return !IsInFight ? null : ((FightGroup) GroupMember.GroupOwner).Fight; }
        }

        public FightGroupMember CurrentFighter
        {
            get
            {
                if (!IsInFight)
                    return null;

                return (GroupMember as FightGroupMember);
            }
        }

        /// <summary>
        ///   unused ?
        /// </summary>
        public Location Position
        {
            get;
            set;
        }

        /// <summary>
        ///   The Id of this character.
        /// </summary>
        public virtual long Id
        {
            get;
            set;
        }

        #endregion
    }
}