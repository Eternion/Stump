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
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public abstract partial class LivingEntity : Entity, ILivingEntity, IMovable
    {
        protected LivingEntity(int id) : base(id)
        {
        }

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
                       (((FightGroup) (GroupMember as FightGroupMember).GroupOwner).Fight.State ==
                        FightState.Fighting ||
                        ((GroupMember as FightGroupMember).GroupOwner as FightGroup).Fight.State ==
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
        protected MovementPath m_movingPath;

        public bool IsMoving
        {
            get { return m_isMoving && m_movingPath != null; }
            protected set { m_isMoving = value; }
        }

        // todo : canMove()
        public virtual bool CanMove()
        {
            return true;
        }

        public void Move(MovementPath movementPath)
        {
            if (!CanMove())
                return;

            NotifyEntityMovingStart(movementPath);

            IsMoving = true;
            m_movingPath = movementPath;
        }

        public virtual void MoveInstant(VectorIsometric to)
        {
            if (!CanMove())
                return;

            NotifyEntityTeleport(to);
        }

        public virtual void MovementEnded()
        {
            if (!IsMoving)
                return;

            Position.ChangeLocation(m_movingPath.End);
            NotifyEntityMovingEnd(m_movingPath);

            IsMoving = false;
            m_movingPath = null;
        }

        public virtual void StopMove(VectorIsometric currentVectorIsometric)
        {
            if (!IsMoving)
                return;

            Position.ChangeLocation(currentVectorIsometric);
            NotifyEntityMovingCancel(currentVectorIsometric);

            IsMoving = false;
            m_movingPath = null;
        }

        #endregion
    }
}