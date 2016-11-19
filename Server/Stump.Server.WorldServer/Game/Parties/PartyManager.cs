using System;
using System.Linq;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Arena;

namespace Stump.Server.WorldServer.Game.Parties
{
    public class PartyManager : EntityManager<PartyManager, Party>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();

        public Party CreateClassical()
        {
            var group = new Party(m_idProvider.Pop());

            AddEntity(group.Id, group);

            return group;
        }

        public ArenaParty CreateArenaParty()
        {
            var group = new ArenaParty(m_idProvider.Pop());

            AddEntity(group.Id, group);

            return group;
        }

        public Party Create(PartyTypeEnum type)
        {
            switch(type)
            {
                case PartyTypeEnum.PARTY_TYPE_CLASSICAL:
                    return CreateClassical();
                case PartyTypeEnum.PARTY_TYPE_ARENA:
                    return CreateArenaParty();
                default:
                    throw new NotImplementedException(string.Format("Party of type {0} not supported", type));
            }
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

        public Party GetGroup(string name)
        {
            return Entities.Values.FirstOrDefault(x => string.Compare(name, x.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}