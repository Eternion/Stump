using System.Collections.Generic;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Parties
{
    public class PartyManager : Singleton<PartyManager>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();
        private readonly Dictionary<int, Party> m_parties = new Dictionary<int, Party>();

        public Party Create(Character leader)
        {
            var group = new Party(m_idProvider.Pop(), leader);

            m_parties.Add(group.Id, group);

            return group;
        }

        public void Remove(Party party)
        {
            m_parties.Remove(party.Id);

            m_idProvider.Push(party.Id);
        }

        public Party GetGroup(int id)
        {
            return m_parties.ContainsKey(id) ? m_parties[id] : null;
        }
    }
}