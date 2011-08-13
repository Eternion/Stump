using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Groups
{
    public class Group 
    {
        /// <summary>
        ///   Maximum number of characters that can be in a same group.
        /// </summary>
        protected const int MaxMemberCount = 8;

        public event Action<Group, Character> LeaderChanged;

        protected void NotifyLeaderChanged(Character leader)
        {
            OnMemberAdded(leader);

            Action<Group, Character> handler = LeaderChanged;
            if (handler != null)
                handler(this, leader);
        }

        protected virtual void OnLeaderChanged(Character leader)
        {
        }

        public event Action<Group, Character> MemberAdded;

        protected void NotifyMemberAdded(Character groupMember)
        {
            OnMemberAdded(groupMember);

            Action<Group, Character> handler = MemberAdded;
            if (handler != null)
                handler(this, groupMember);
        }

        protected virtual void OnMemberAdded(Character member)
        {
        }

        public event Action<Group, Character> MemberRemoved;

        protected void NotifyMemberRemoved(Character groupMember)
        {
            OnMemberRemoved(groupMember);

            Action<Group, Character> handler = MemberRemoved;
            if (handler != null)
                handler(this, groupMember);
        }

        protected virtual void OnMemberRemoved(Character member)
        {
        }

        public event Action<Group> GroupDisbanded;

        protected void NotifyGroupDisbanded()
        {
            OnGroupDisbanded();

            Action<Group> handler = GroupDisbanded;
            if (handler != null)
                handler(this);
        }

        protected virtual void OnGroupDisbanded()
        {
        }

        private readonly List<Character> m_members = new List<Character>();
        private readonly object m_locker = new object();

        internal Group(int id)
        {
            Id = id;
        }

        public int Id
        {
            get;
            private set;
        }

        public bool IsFull
        {
            get { return m_members.Count >= MaxMemberCount;}
        }

        public int GroupLevel
        {
            get { return m_members.Sum(entry => entry.Level); }
        }

        public int MembersCount
        {
            get
            {
                return m_members.Count;
            }
        }

        public IEnumerable<Character> Characters
        {
            get { return m_members; }
        }

        private Character m_leader;

        public Character Leader
        {
            get { return m_leader; }
            private set
            {
                m_leader = value;
                NotifyLeaderChanged(m_leader);
            }
        }

        public bool AddMember(Character character)
        {
            if (IsFull)
                return false;

            if (Leader == null)
                Leader = character;

            lock(m_locker)
                m_members.Add(character);

            NotifyMemberAdded(character);

            return true;
        }

        public void RemoveMember(Character character)
        {
            if (m_members.Remove(character))
            {
                if (MembersCount <= 1)
                    Disband();
                else
                {
                    if (Leader == character)
                        Leader = m_members.First();

                    NotifyMemberRemoved(character);
                }
            }
        }

        public bool IsInGroup(Character character)
        {
            return m_members.Contains(character);
        }

        public void Disband()
        {
            m_members.Clear();

            GroupManager.Instance.Remove(this);

            NotifyGroupDisbanded();
        }

        public Character GetMember(int id)
        {
            return m_members.Find(entry => entry.Id == id);
        }

        public void SendToAll(Message message)
        {
            foreach (var character in m_members)
            {
                character.Client.Send(message);
            }
        }
    }
}