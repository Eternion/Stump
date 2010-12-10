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
using System.Linq;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Fights;

namespace Stump.Server.WorldServer.Groups
{
    public sealed class FightGroup : Group
    {
        public int TeamId
        {
            get;
            set;
        }

        public short[] Positions
        {
            get;
            set;
        }

        public Fight Fight
        {
            get;
            set;
        }

        public bool AllAreReady()
        {
            return Members.Cast<FightGroupMember>().All(member => member.IsReady);
        }

        public int[] GetAlivesIds()
        {
            return
                Members.Where(
                    entry =>
                    (entry is FightGroupMember &&
                     !(((entry as FightGroupMember).IsDead) || (entry as FightGroupMember).HasLeft)))
                    .Select(entry => (int) entry.Entity.Id).ToArray();
        }

        public int[] GetDeadsIds()
        {
            return
                Members.Where(
                    entry =>
                    (entry is FightGroupMember &&
                     (((entry as FightGroupMember).IsDead) || (entry as FightGroupMember).HasLeft)))
                    .Select(entry => (int) entry.Entity.Id).ToArray();
        }

        public int[] GetLeftIds()
        {
            return Members.Where(entry => (entry is FightGroupMember && (entry as FightGroupMember).HasLeft))
                .Select(entry => (int) entry.Entity.Id).ToArray();
        }

        public FightGroupMember GetMemberByCharacter(int chrId)
        {
            return Members.Where(entry => (entry is FightGroupMember && entry.Entity.Id == chrId))
                       .FirstOrDefault() as FightGroupMember;
        }

        /// <summary>
        ///   Add a new member to this group.
        /// </summary>
        /// <param name = "ent"></param>
        public override GroupMember AddMember(Entity ent)
        {
            GroupMember newMember = null;

            m_syncLock.EnterWriteLock();

            try
            {
                if (!IsFull)
                {
                    newMember = new FightGroupMember(ent, this);
                    Members.Add(newMember);
                    OnAddMember(newMember);
                    if (m_leaderId < 0)
                        m_leaderId = (int) ent.Id;
                }
            }
            catch (Exception e)
            {
                logger.ErrorException(string.Format("Could not add member {0} to group {1}", ent, this), e);
            }
            finally
            {
                m_syncLock.ExitWriteLock();
            }

            // Send Update to all (todo)

            return newMember;
        }

        /// <summary>
        ///   Remove member from this Group.
        /// </summary>
        public override void RemoveMember(GroupMember group)
        {
            if (Count <= MinGroupMemberCount)
            {
                Disband();
            }
            else
            {
                m_syncLock.EnterWriteLock();

                try
                {
                    // ToDo : Some logic (change leaders, update stuff like group level etc.
                    Members.Remove(group);
                    OnMemberRemoved(group);
                    if (group.Entity.Id == m_leaderId)
                        m_leaderId = (int) Members.First().Entity.Id;
                }
                finally
                {
                    m_syncLock.ExitWriteLock();
                }
            }

            // send update to all (?)
        }
    }
}