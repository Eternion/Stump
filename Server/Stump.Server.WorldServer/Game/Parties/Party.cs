using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Collections;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps;
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

            var handler = LeaderChanged;
            if (handler != null)
                handler(this, leader);
        }

        public event MemberAddedHandler GuestAdded;

        protected virtual void OnGuestAdded(Character groupGuest)
        {
            PartyHandler.SendPartyNewGuestMessage(Clients, this, groupGuest);

            var handler = GuestAdded;
            if (handler != null)
                handler(this, groupGuest);
        }

        public event MemberRemovedHandler GuestRemoved;

        protected virtual void OnGuestRemoved(Character groupGuest, bool kicked)
        {
            m_clients.Remove(groupGuest.Client);
            var handler = GuestRemoved;
            if (handler != null)
                handler(this, groupGuest, kicked);
        }

        public event Action<Party, Character> GuestPromoted;

        protected virtual void OnGuestPromoted(Character groupMember)
        {
            m_clients.Add(groupMember.Client);

            GroupLevelSum += groupMember.Level;
            GroupLevelAverage = GroupLevelSum/MembersCount;

            PartyHandler.SendPartyJoinMessage(groupMember.Client, this);
            PartyHandler.SendPartyNewMemberMessage(Clients, this, groupMember);

            BindEvents(groupMember);

            var handler = GuestPromoted;
            if (handler != null)
                handler(this, groupMember);
        }

        public event MemberRemovedHandler MemberRemoved;

        protected virtual void OnMemberRemoved(Character groupMember, bool kicked)
        {
            m_clients.Remove(groupMember.Client);

            GroupLevelSum -= groupMember.Level;
            GroupLevelAverage = MembersCount > 0 ? GroupLevelSum / MembersCount : 0;

            if (kicked)
                PartyHandler.SendPartyKickedByMessage(groupMember.Client, this, Leader);
            else
                groupMember.Client.Send(new PartyLeaveMessage(Id));

            PartyHandler.SendPartyMemberRemoveMessage(Clients, this, groupMember);
            var handler = MemberRemoved;

            UnBindEvents(groupMember);

            if (handler != null)
                handler(this, groupMember, kicked);
        }

        public event Action<Party> PartyDeleted;

        protected virtual void OnGroupDisbanded()
        {
            PartyHandler.SendPartyDeletedMessage(Clients, this);

            UnBindEvents();

            var handler = PartyDeleted;
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
            get;
            private set;
        }

        public int GroupLevelAverage
        {
            get;
            private set;
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

        public virtual bool CanInvite(Character character, out PartyJoinErrorEnum error, Character inviter = null, bool send = true)
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

        public virtual bool CanLeaveParty(Character character)
        {
            return IsMember(character);
        }

        public bool AddGuest(Character character)
        {
            PartyJoinErrorEnum error;
            if (!CanInvite(character, out error))
            {
                PartyHandler.SendPartyCannotJoinErrorMessage(character.Client, this, error);
                return false;
            }

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

        public void RemoveAllGuest()
        {
            foreach (var guest in m_guests.ToArray())
            {
                guest.DenyAllInvitations(this);
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

        public virtual bool AddMember(Character member)
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

        public virtual void Kick(Character member)
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
                foreach (var character in Members)
                {
                    action(character);
                }
            }
        }

        public void ForEach(Action<Character> action, Character except)
        {
            lock (m_memberLocker)
            {
                foreach (var character in Members.Where(character => character != except))
                {
                    action(character);
                }
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

        private void OnEnterMap(RolePlayActor character, Map map)
        {
            UpdateMember(character as Character);
        }

        private void OnContextChanged(Character character, bool infight)
        {
            // not rdy yet
            if (!infight)
                return;

            /*if (character.Fight is FightAgression)
            {
                PartyHandler.SendPartyMemberInFightMessage(Clients, this, character,
                    character.Fighter.Team == character.Fight.ChallengersTeam
                    ? PartyFightReasonEnum.ATTACK_PLAYER
                    : PartyFightReasonEnum.PLAYER_ATTACK, character.Fight);
            }
            else if (character.Fight is FightPvM)
                PartyHandler.SendPartyMemberInFightMessage(Clients, this, character, PartyFightReasonEnum.MONSTER_ATTACK, character.Fight);*/
        }

        private void BindEvents(Character member)
        {
            member.LifeRegened += OnLifeUpdated;
            member.LevelChanged += OnLevelChanged;
            member.EnterMap += OnEnterMap;
            member.ContextChanged += OnContextChanged;
        }

        private void UnBindEvents(Character member)
        {
            member.LifeRegened -= OnLifeUpdated;
            member.LevelChanged -= OnLevelChanged;
            member.EnterMap -= OnEnterMap;
            member.ContextChanged -= OnContextChanged;
        }

        private void UnBindEvents()
        {
            foreach (var member in Members)
            {
                UnBindEvents(member);
            }
        }

        public virtual PartyMemberInformations GetPartyMemberInformations(Character character)
        {
            return character.GetPartyMemberInformations();
        }

        public virtual PartyGuestInformations GetPartyGuestInformations(Character character)
        {
            return character.GetPartyGuestInformations(this);
        }
    }
}