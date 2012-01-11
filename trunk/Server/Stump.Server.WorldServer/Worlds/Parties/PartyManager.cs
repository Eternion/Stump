using System.Collections.Generic;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Parties
{
    public class PartyManager : EntityManager<PartyManager, Party>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();

        public Party Create(Character leader)
        {
            var group = new Party(m_idProvider.Pop(), leader);

            AddEntity(group.Id, group);

            return group;
        }

        public void Remove(Party party)
        {
            RemoveEntity(party.Id);

            m_idProvider.Push(party.Id);
        }

        public Party GetGroup(int id)
        {
            return GetEntityOrDefault(id);
        }
    }
}