
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
    public interface IGroup : IInstance
    {
        bool IsFull
        {
            get;
        }

        int Count
        {
            get;
        }

        int GroupLevel
        {
            get;
        }

        Character GetCharacterById(int id);

        LivingEntity GetEntityById(int id);
    }

    public abstract class Group<T> : IGroup
        where T : class, IGroupMember 
    {
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
            protected set;
        }

        public List<T> Members
        {
            get;
            protected set;
        }

        public T Leader
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

        /// <summary>
        ///   Constructor
        /// </summary>
        protected Group()
        {
            m_leaderId = -1;
            m_syncLock = new ReaderWriterLockSlim();
            GroupLevel = 0;
            Members = new List<T>();

            MemberAdded += OnAddMember;
            MemberRemoved += OnMemberRemoved;
        }

        #region Group Member Management

        public Character GetCharacterById(int id)
        {
            return Members.Where(entry => (entry.Entity.Id == id && entry.Entity is Character))
                .Select(entry => entry.Entity as Character)
                .FirstOrDefault();
        }

        public LivingEntity GetEntityById(int id)
        {
            return Members.Where(entry => entry.Entity.Id == id)
                .Select(entry => entry.Entity)
                .FirstOrDefault();
        }

        public T GetMemberById(int id)
        {
            return Members.Where(entry => entry.Entity.Id == id)
                .FirstOrDefault();
        }

        public void AddMembers(T[] members)
        {
            foreach (var member in members)
                AddMember(member);
        }

        /// <summary>
        ///   Add a new member to this group.
        /// </summary>
        public virtual T AddMember(T member)
        {
            m_syncLock.EnterWriteLock();

            try
            {
                if (!IsFull)
                {
                    Members.Add(member);

                    if (m_leaderId < 0)
                        m_leaderId = (int) member.Entity.Id;

                    NotifyMemberAdded(member);
                }
            }
            catch (Exception e)
            {
                logger.Error(string.Format("Could not add member {0} to group {1} : {2}", member.Entity, this, e));
            }
            finally
            {
                m_syncLock.ExitWriteLock();
            }

            // Send Update to all (todo)

            return member;
        }

        /// <summary>
        ///   Remove member from this Group.
        /// </summary>
        public virtual void RemoveMember(T member)
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
                    
                    if (member.Entity.Id == m_leaderId)
                        m_leaderId = (int) Members.First().Entity.Id;

                    NotifyMemberRemoved(member);
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

        public event Action<Group<T>, T> MemberAdded;

        protected void NotifyMemberAdded(T groupMember)
        {
            Action<Group<T>, T> handler = MemberAdded;
            if (handler != null)
                handler(this, groupMember);
        }

        public event Action<Group<T>, T> MemberRemoved;

        protected void NotifyMemberRemoved(T groupMember)
        {
            Action<Group<T>, T> handler = MemberRemoved;
            if (handler != null)
                handler(this, groupMember);
        }


        protected virtual void OnAddMember(Group<T> group, T member)
        {
            GroupLevel += member.Entity.Level;
        }

        protected virtual void OnMemberRemoved(Group<T> group, T member)
        {
            GroupLevel -= member.Entity.Level;
        }

        public virtual void Disband()
        {
            m_syncLock.EnterWriteLock();

            try
            {
                // send to everyone group has been destroyed.
                Parallel.ForEach(Members, RemoveMember);

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