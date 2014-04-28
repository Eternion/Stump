using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Collections;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Context.RolePlay.Party;

namespace Stump.Server.WorldServer.Game.Parties
{
    // todo : update members when their stats change
    public class Party
    {
        /// <summary>
        ///     Maximum number of characters that can be in a same group.
        /// </summary>
        public const int MaxMemberCount = 8;

        #region Events

        public delegate void MemberAddedHandler(Party party, Character member);

        public delegate void MemberRemovedHandler(Party party, Character member, bool kicked);

        public event Action<Party, Character> LeaderChanged;

        protected virtual void OnLeaderChanged(Character leader)
        {
            PartyHandler.SendPartyLeaderUpdateMessage(Clients, this, leader);

            Action<Party, Character> handler = LeaderChanged;
            if (handler != null)
                handler(this, leader);
        }

        public event MemberAddedHandler GuestAdded;

        protected virtual void OnGuestAdded(Character groupGuest)
        {
            PartyHandler.SendPartyNewGuestMessage(Clients, this, groupGuest);

            MemberAddedHandler handler = GuestAdded;
            if (handler != null)
                handler(this, groupGuest);
        }

        public event MemberRemovedHandler GuestRemoved;

        protected virtual void OnGuestRemoved(Character groupGuest, bool kicked)
        {
            m_clients.Remove(groupGuest.Client);
            MemberRemovedHandler handler = GuestRemoved;
            if (handler != null)
                handler(this, groupGuest, kicked);
        }

        public event Action<Party, Character> GuestPromoted;

        protected virtual void OnGuestPromoted(Character groupMember)
        {
            m_clients.Add(groupMember.Client);

            m_levelSum += groupMember.Level;
            m_levelAvg = m_levelSum/MembersCount;

            PartyHandler.SendPartyJoinMessage(groupMember.Client, this);

            UpdateMember(groupMember);
            BindEvents(groupMember);

            Action<Party, Character> handler = GuestPromoted;
            if (handler != null)
                handler(this, groupMember);
        }

        public event MemberRemovedHandler MemberRemoved;

        protected virtual void OnMemberRemoved(Character groupMember, bool kicked)
        {
            m_clients.Remove(groupMember.Client);

            m_levelSum -= groupMember.Level;
            m_levelAvg = m_levelSum/MembersCount;

            if (kicked)
                PartyHandler.SendPartyKickedByMessage(groupMember.Client, this, Leader);
            else
                groupMember.Client.Send(new PartyLeaveMessage(Id));

            PartyHandler.SendPartyMemberRemoveMessage(Clients, this, groupMember);
            MemberRemovedHandler handler = MemberRemoved;

            UnBindEvents(groupMember);

            if (handler != null)
                handler(this, groupMember, kicked);
        }

        public event Action<Party> PartyDeleted;

        protected virtual void OnGroupDisbanded()
        {
            PartyHandler.SendPartyDeletedMessage(Clients, this);

            UnBindEvents();

            Action<Party> handler = PartyDeleted;
            if (handler != null)
                handler(this);
        }

        #endregion

        private readonly WorldClientCollection m_clients = new WorldClientCollection();

        private readonly object m_guestLocker = new object();
        private readonly ConcurrentList<Character> m_guests = new ConcurrentList<Character>();
        private readonly object m_memberLocker = new object();
        private readonly ConcurrentList<Character> m_members = new ConcurrentList<Character>();
        private bool m_restricted;

        private int m_levelSum;
        private int m_levelAvg;
        private int m_prospectionSum;

        public Party(int id)
        {
            Id = id;
            Restricted = true;
        }

        public int Id
        {
            get;
            private set;
        }

        public WorldClientCollection Clients
        {
            get { return m_clients; }
        }

        public virtual PartyTypeEnum Type
        {
            get { return PartyTypeEnum.PARTY_TYPE_CLASSICAL; }
        }

        public bool Restricted
        {
            get { return m_restricted; }
            private set
            {
                m_restricted = value;
                PartyHandler.SendPartyRestrictedMessage(Clients, this, m_restricted);
            }
        }

        public virtual int MembersLimit
        {
            get { return MaxMemberCount; }
        }

        public bool IsFull
        {
            get { return m_members.Count >= MembersLimit; }
        }

        public int GroupLevelSum
        {
            get { return m_levelSum; }
        }

        public int GroupLevelAverage
        {
            get { return m_levelAvg; }
        }

        public int GroupProspecting
        {
            get { return m_members.Sum(entry => entry.Stats[PlayerFields.Prospecting].Total); }
        }

        public int MembersCount
        {
            get { return m_members.Count; }
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

        public bool Disbanded
        {
            get;
            private set;
        }

        public bool CanInvite(Character character)
        {
            PartyJoinErrorEnum dummy;
            return CanInvite(character, out dummy);
        }

        public bool CanInvite(Character character, out PartyJoinErrorEnum error)
        {
            if (IsMember(character) || IsGuest(character))
            {
                error = PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_ALREADY_INVITED;
                return false;
            }

            if (IsFull)
            {
                error = PartyJoinErrorEnum.PARTY_JOIN_ERROR_PARTY_FULL;
                return false;
            }

            error = PartyJoinErrorEnum.PARTY_JOIN_ERROR_UNKNOWN;
            return true;
        }

        public bool AddGuest(Character character)
        {
            if (IsFull || !CanInvite(character))
                return false;

            lock (m_guestLocker)
                m_guests.Add(character);

            OnGuestAdded(character);

            return true;
        }

        public void RemoveGuest(Character character)
        {
            lock (m_guestLocker)
            {
                if (!m_guests.Remove(character))
                    return;

                OnGuestRemoved(character, false);

                if (MembersCount <= 1)
                    Disband();
            }
        }

        /// <summary>
        ///     The guest is promote to member in the party. Whenever the player is not a guest, he auto joined the party.
        /// </summary>
        /// <param name="guest"></param>
        public bool PromoteGuestToMember(Character guest)
        {
            if (IsMember(guest))
                return false;

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

            if (Leader == null)
                Leader = guest;

            OnGuestPromoted(guest);

            return true;
        }

        public bool AddMember(Character member)
        {
            return PromoteGuestToMember(member);
        }

        public void RemoveMember(Character character)
        {
            lock (m_memberLocker)
            {
                if (!m_members.Remove(character))
                    return;

                OnMemberRemoved(character, false);

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

                OnMemberRemoved(member, true);

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

            OnLeaderChanged(Leader);
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
            if (Disbanded)
                return;

            Disbanded = true;

            PartyManager.Instance.Remove(this);

            OnGroupDisbanded();
        }

        public Character GetMember(int id)
        {
            return m_members.SingleOrDefault(entry => entry.Id == id);
        }

        public Character GetGuest(int id)
        {
            return m_guests.SingleOrDefault(entry => entry.Id == id);
        }

        public void UpdateMember(Character character)
        {
            if (!IsInGroup(character))
                return;

            PartyHandler.SendPartyUpdateMessage(Clients, this, character);
        }

        public void ForEach(Action<Character> action)
        {
            lock (m_memberLocker)
            {
                foreach (Character character in Members)
                {
                    action(character);
                }
            }
        }

        public void ForEach(Action<Character> action, Character except)
        {
            lock (m_memberLocker)
            {
                foreach (Character character in Members)
                {
                    if (character != except)
                        action(character);
                }
            }
        }

        public void SendToAll(Message message)
        {
            lock (m_memberLocker)
            {
                m_clients.Send(message);
            }
        }

        private void OnLifeUpdated(Character character, int regainedLife)
        {
            UpdateMember(character);
        }

        private void OnLevelChanged(Character character, byte currentLevel, int difference)
        {
            UpdateMember(character);
        }

        private void BindEvents(Character member)
        {
            member.LifeRegened += OnLifeUpdated;
            member.LevelChanged += OnLevelChanged;
        }

        private void UnBindEvents(Character member)
        {
            member.LifeRegened -= OnLifeUpdated;
            member.LevelChanged -= OnLevelChanged;
        }

        private void UnBindEvents()
        {
            foreach (Character member in Members)
            {
                UnBindEvents(member);
            }
        }
    }
}