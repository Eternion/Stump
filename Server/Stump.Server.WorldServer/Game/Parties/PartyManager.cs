using Stump.Core.Pool;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Parties
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