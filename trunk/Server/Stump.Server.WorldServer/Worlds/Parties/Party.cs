using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Handlers.Context.RolePlay.Party;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Parties
{
    public class Party 
    {
        /// <summary>
        ///   Maximum number of characters that can be in a same group.
        /// </summary>
        protected const int MaxMemberCount = 8;

        #region Events

        public delegate void MemberAddedHandler(Party party, Character member);
        public delegate void MemberRemovedHandler(Party party, Character member, bool kicked);

        public event Action<Party, Character> LeaderChanged;

        protected void NotifyLeaderChanged(Character leader)
        {
            OnLeaderChanged(leader);

            Action<Party, Character> handler = LeaderChanged;
            if (handler != null)
                handler(this, leader);
        }

        protected virtual void OnLeaderChanged(Character leader)
        {
            DoForAll(entry => PartyHandler.SendPartyLeaderUpdateMessage(entry.Client, leader));
        }

        public event MemberAddedHandler GuestAdded;

        protected void NotifyGuestAdded(Character groupGuest)
        {
            OnGuestAdded(groupGuest);

            var handler = GuestAdded;
            if (handler != null)
                handler(this, groupGuest);
        }

        protected virtual void OnGuestAdded(Character guest)
        {
            DoForAll(entry => PartyHandler.SendPartyNewGuestMessage(entry.Client, this, guest));
        }

        public event MemberRemovedHandler GuestRemoved;

        protected void NotifyGuestRemoved(Character groupGuest, bool kicked)
        {
            OnGuestRemoved(groupGuest, kicked);

            var handler = GuestRemoved;
            if (handler != null)
                handler(this, groupGuest, kicked);
        }

        protected virtual void OnGuestRemoved(Character member, bool kicked)
        {
        }

        public event Action<Party, Character> GuestPromoted;

        protected void NotifyGuestPromoted(Character groupMember)
        {
            OnGuestPromoted(groupMember);

            var handler = GuestPromoted;
            if (handler != null)
                handler(this, groupMember);
        }

        protected virtual void OnGuestPromoted(Character member)
        {
            PartyHandler.SendPartyJoinMessage(member.Client, this);

            UpdateMember(member);
        }

        public event MemberRemovedHandler MemberRemoved;

        protected void NotifyMemberRemoved(Character groupMember, bool kicked)
        {
            OnMemberRemoved(groupMember, kicked);

            MemberRemovedHandler handler = MemberRemoved;
            if (handler != null)
                handler(this, groupMember, kicked);
        }

        protected virtual void OnMemberRemoved(Character member, bool kicked)
        {
            if (kicked)
                PartyHandler.SendPartyKickedByMessage(member.Client, Leader);
            else
                member.Client.Send(new PartyLeaveMessage());

            DoForAll(entry => PartyHandler.SendPartyMemberRemoveMessage(entry.Client, member));
        }

        public event Action<Party> PartyDeleted;

        protected void NotifyGroupDisbanded()
        {
            OnGroupDisbanded();

            Action<Party> handler = PartyDeleted;
            if (handler != null)
                handler(this);
        }

        protected virtual void OnGroupDisbanded()
        {
            DoForAll(entry => PartyHandler.SendPartyDeletedMessage(entry.Client, this));
        }

        #endregion

        private readonly List<Character> m_members = new List<Character>();
        private readonly List<Character> m_guests = new List<Character>();

        private readonly object m_guestLocker = new object();
        private readonly object m_memberLocker = new object();

        internal Party(int id, Character leader)
        {
            Id = id;
            Restricted = true;

            m_members.Add(leader);
            Leader = leader;
            PartyHandler.SendPartyJoinMessage(leader.Client, this);
        }

        public int Id
        {
            get;
            private set;
        }

        private bool m_restricted;

        public bool Restricted
        {
            get { return m_restricted; }
            private set { m_restricted = value;
                DoForAll(entry => PartyHandler.SendPartyRestrictedMessage(entry.Client, m_restricted)); }
        }

        public bool IsFull
        {
            get { return m_members.Count >= MaxMemberCount;}
        }

        public int GroupLevel
        {
            get { return m_members.Sum(entry => entry.Level); }
        }

        public int GroupProspecting
        {
            get { return m_members.Sum(entry => entry.Stats[CaracteristicsEnum.Prospecting].Total); }
        }

        public int MembersCount
        {
            get
            {
                return m_members.Count + m_guests.Count;
            }
        }

        public IEnumerable<Character> Members
        {
            get { return m_members; }
        }

        public IEnumerable<Character> Guests
        {
            get { return m_guests; }
        }

        public Character Leader
        {
            get;
            private set;
        }

        public bool AddGuest(Character character)
        {
            if (IsFull)
                return false;

            lock (m_guestLocker)
                m_guests.Add(character);

            NotifyGuestAdded(character);

            return true;
        }

        public void RemoveGuest(Character character)
        {
            lock (m_guestLocker)
            {
                if (!m_guests.Remove(character))
                    return;

                NotifyGuestRemoved(character, false);

                if (MembersCount <= 1)
                    Disband();
            }
        }

        /// <summary>
        /// The guest is promote to member in the party. Whenever the player is not a guest, he auto joined the party.
        /// </summary>
        /// <param name="guest"></param>
        public bool PromoteGuestToMember(Character guest)
        {
            if (!IsGuest(guest))
            {
                // if the player is not invited we force an invitation
                if (!AddGuest(guest))
                    return false;
            }

            lock (m_guestLocker)
                m_guests.Remove(guest);

            lock (m_memberLocker)
                m_members.Add(guest);

            NotifyGuestPromoted(guest);

            return true;
        }

        public void RemoveMember(Character character)
        {
            lock (m_memberLocker)
            {
                if (!m_members.Remove(character)) 
                    return;

                NotifyMemberRemoved(character, false);

                if (MembersCount <= 1)
                    Disband();
                else if (Leader == character)
                {
                    ChangeLeader(m_members.First());
                }
            }
        }

        public void Kick(Character member)
        {
            lock (m_memberLocker)
            {
                if (!m_members.Remove(member))
                    return;

                NotifyMemberRemoved(member, true);

                if (MembersCount <= 1)
                    Disband();
                else if (Leader == member)
                {
                    ChangeLeader(m_members.First());
                }
            }
        }

        public void ChangeLeader(Character leader)
        {
            if (!IsInGroup(leader))
                return;
            
            if (Leader == leader)
                return;

            Leader = leader;

            NotifyLeaderChanged(Leader);
        }

        public bool IsInGroup(Character character)
        {
            return IsMember(character) || IsGuest(character);
        }

        public bool IsMember(Character character)
        {
            return m_members.Contains(character);
        }

        public bool IsGuest(Character character)
        {
            return m_guests.Contains(character);
        }

        public void Disband()
        {
            PartyManager.Instance.Remove(this);

            NotifyGroupDisbanded();
        }

        public Character GetMember(int id)
        {
            return m_members.Find(entry => entry.Id == id);
        }

        public Character GetGuest(int id)
        {
            return m_guests.Find(entry => entry.Id == id);
        }

        public void UpdateMember(Character character)
        {
            if (!IsInGroup(character))
                return;

            DoForAll(entry => PartyHandler.SendPartyUpdateMessage(entry.Client, character), character);
        }

        public void DoForAll(Action<Character> action)
        {
            foreach (var character in Members)
            {
                action(character);
            }
        }

        public void DoForAll(Action<Character> action, Character except)
        {
            foreach (var character in Members)
            {
                if (character != except) 
                    action(character);
            }
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