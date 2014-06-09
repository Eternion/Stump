using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaPreFightTeam
    {
        private readonly List<ArenaWaitingCharacter> m_members = new List<ArenaWaitingCharacter>();

        public event Action<ArenaPreFightTeam, ArenaWaitingCharacter> MemberAdded;

        protected virtual void OnMemberAdded(ArenaWaitingCharacter arg2)
        {
            var handler = MemberAdded;
            if (handler != null) handler(this, arg2);
        }

        public event Action<ArenaPreFightTeam, ArenaWaitingCharacter> MemberRemoved;

        protected virtual void OnMemberRemoved(ArenaWaitingCharacter arg2)
        {
            var handler = MemberRemoved;
            if (handler != null) handler(this, arg2);
        }


        public ArenaPreFightTeam(TeamEnum team, ArenaPreFight fight)
        {
            Team = team;
            Fight = fight;
        }

        public TeamEnum Team
        {
            get;
            private set;
        }

        public ArenaPreFight Fight
        {
            get;
            private set;
        }

        public int MissingMembers
        {
            get { return ArenaManager.MaxPlayersPerFights - m_members.Count; }
        }

        public ReadOnlyCollection<ArenaWaitingCharacter> Members
        {
            get { return m_members.AsReadOnly(); }
        }

        public ArenaWaitingCharacter AddCharacter(Character character)
        {
            var obj = new ArenaWaitingCharacter(character, this);

            m_members.Add(obj);
            OnMemberAdded(obj);
            return obj;
        }

        public bool RemoveCharacter(Character character)
        {
            var member = m_members.FirstOrDefault(x => x.Character == character);

            return member != null && RemoveCharacter(member);
        }

        public bool RemoveCharacter(ArenaWaitingCharacter character)
        {
            var success = m_members.Remove(character);

            if (success)
                OnMemberRemoved(character);

            return success;
        }
    }
}