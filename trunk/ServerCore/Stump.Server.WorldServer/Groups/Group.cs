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
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Server.BaseServer.Manager;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Groups
{
    public abstract class Group : IInstance
    {
        #region Fields

        /// <summary>
        ///   Maximum number of characters that can be in a same group.
        /// </summary>
        protected const int MaxMemberCount = 8;

        /// <summary>
        ///   Minimum number of characters needed to be/create a group.
        /// </summary>
        protected const int MinGroupMemberCount = 2;

        protected Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Id of the leader of the group
        /// </summary>
        protected int m_leaderId;

        /// <summary>
        ///   Synchronization tool.
        /// </summary>
        protected ReaderWriterLockSlim m_syncLock;

        #endregion

        #region Properties

        public bool IsFull
        {
            get { return (Count >= MaxMemberCount); }
        }

        public int Count
        {
            get { return Members.Count; }
        }

        public int GroupLevel
        {
            get;
            set;
        }

        public List<GroupMember> Members
        {
            get;
            set;
        }

        public GroupMember Leader
        {
            get { return GetMemberById(m_leaderId); }
            protected set
            {
                if (value == null) throw new ArgumentNullException("value");
                GetMemberById(m_leaderId);
            }
        }

        public int Id
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        ///   Constructor
        /// </summary>
        protected Group()
        {
            m_leaderId = -1;
            m_syncLock = new ReaderWriterLockSlim();
            GroupLevel = 0;
            Members = new List<GroupMember>();
        }

        #region Group Member Management

        public Character GetCharacterById(int id)
        {
            return Members.Where(entry => (entry.Entity.Id == id && entry.Entity is Character))
                .Select(entry => entry.Entity as Character)
                .FirstOrDefault();
        }

        public Entity GetEntityById(int id)
        {
            return Members.Where(entry => entry.Entity.Id == id)
                .Select(entry => entry.Entity)
                .FirstOrDefault();
        }

        public GroupMember GetMemberById(int id)
        {
            return Members.Where(entry => entry.Entity.Id == id)
                .FirstOrDefault();
        }

        public void AddMembers(Entity[] ents)
        {
            foreach (Entity ent in ents)
                AddMember(ent);
        }

        /// <summary>
        ///   Add a new member to this group.
        /// </summary>
        /// <param name = "ent"></param>
        public virtual GroupMember AddMember(Entity ent)
        {
            GroupMember newMember = null;

            m_syncLock.EnterWriteLock();

            try
            {
                if (!IsFull)
                {
                    newMember = new GroupMember(ent, this);
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
        public virtual void RemoveMember(GroupMember member)
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
                    Members.Remove(member);
                    OnMemberRemoved(member);
                    if (member.Entity.Id == m_leaderId)
                        m_leaderId = (int) Members.First().Entity.Id;
                }
                finally
                {
                    m_syncLock.ExitWriteLock();
                }
            }

            // send update to all (?)
        }

        #endregion

        #region Group Events

        public virtual void OnAddMember(GroupMember member)
        {
            GroupLevel += member.Entity.Level;
        }

        public virtual void OnMemberRemoved(GroupMember member)
        {
            GroupLevel -= member.Entity.Level;

            member.Entity.GroupMember = null;
        }

        public virtual void Disband()
        {
            m_syncLock.EnterWriteLock();

            try
            {
                // send to everyone group has been destroyed.
                Parallel.ForEach(Members, member => member.Entity.GroupMember = null);

                Members.Clear();
                // Dispose this group.
            }
            finally
            {
                m_syncLock.ExitWriteLock();
            }
        }

        #endregion
    }
}