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
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public abstract partial class LivingEntity : Entity, ILivingEntity, IMovable
    {
        public int CurrentHealth
        {
            get { return Stats["Health"].TotalSafe; }
        }

        public int MaxHealth
        {
            get { return ((StatsHealth) Stats["Health"]).TotalMax; }
        }

        #region ILivingEntity Members

        public int Level
        {
            get;
            set;
        }

        public int BaseHealth
        {
            get { return Stats["Health"].Base; }
            set { Stats["Health"].Base = value; }
        }

        public int DamageTaken
        {
            get { return ((StatsHealth) Stats["Health"]).DamageTaken; }
            set { ((StatsHealth) Stats["Health"]).DamageTaken = value; }
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

        public Group Group
        {
            get { return !IsInGroup ? null : GroupMember.GroupOwner; }
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

        #endregion

        public abstract FightTeamMemberInformations ToNetworkTeamMember();

        public abstract GameFightFighterInformations ToNetworkFighter();

        #region Movements

        private bool m_isMoving;
        private VectorIso m_movingPath;

        public bool IsMoving
        {
            get { return m_isMoving && m_movingPath != null; }
            protected set
            {
                m_isMoving = value;
                m_movingPath = null;
            }
        }

        // todo : check another factors
        public virtual bool CanMove()
        {
            return !IsMoving;
        }

        public void Move(List<uint> movementKeys)
        {
            if (!CanMove())
                return;

            NotifyEntityCompressedMovingStart(movementKeys);

            IsMoving = true;
            m_movingPath = new VectorIso((ushort) (movementKeys[movementKeys.Count - 1] & 0x0FFF),
                                         DirectionsEnum.DIRECTION_EAST, Map);
        }

        public virtual void Move(VectorIso to)
        {
            if (!CanMove())
                return;

            NotifyEntityMovingStart(to);

            IsMoving = true;
            m_movingPath = to;
        }

        public virtual void Move(ushort cellId)
        {
            Move(new VectorIso(cellId, Position.Direction));
        }

        public virtual void Move(ushort cellId, DirectionsEnum direction)
        {
            Move(new VectorIso(cellId, direction));
        }

        public virtual void MoveInstant(VectorIso to)
        {
            if (!CanMove())
                return;

            NotifyEntityTeleport(to);
        }

        public virtual void MoveInstant(ushort cellId)
        {
            MoveInstant(new VectorIso(cellId, Position.Direction));
        }

        public virtual void MoveInstant(ushort cellId, DirectionsEnum direction)
        {
            MoveInstant(new VectorIso(cellId, direction));
        }

        public virtual void MovementEnded()
        {
            if (!IsMoving)
                return;

            Position.ChangeLocation(m_movingPath);
            NotifyEntityMovingStop(m_movingPath);

            IsMoving = false;
        }

        public virtual void StopMove(VectorIso currentVectorIso)
        {
            if (!IsMoving)
                return;

            Position.ChangeLocation(currentVectorIso);
            NotifyEntityMovingStop(currentVectorIso);

            IsMoving = false;
        }

        #endregion
    }
}