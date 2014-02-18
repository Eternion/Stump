using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightManager : EntityManager<FightManager, Fight>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();

        public Fight CreateDuel(Map map)
        {
            var redTeam = new FightPlayerTeam(0, map.GetRedFightPlacement());
            var blueTeam = new FightPlayerTeam(1, map.GetBlueFightPlacement());

            var fight = new FightDuel(m_idProvider.Pop(), map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public Fight CreatePvMFight(Map map)
        {
            var redTeam = new FightPlayerTeam(0, map.GetRedFightPlacement());
            var blueTeam = new FightMonsterTeam(1, map.GetBlueFightPlacement());

            var fight = new FightPvM(m_idProvider.Pop(), map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public Fight CreateAgressionFight(Map map, AlignmentSideEnum redAlignment, AlignmentSideEnum blueAlignment)
        {
            var redTeam = new FightPlayerTeam(0, map.GetRedFightPlacement(), redAlignment);
            var blueTeam = new FightPlayerTeam(1, map.GetBlueFightPlacement(), blueAlignment);

            var fight = new FightAgression(m_idProvider.Pop(), map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public FightPvT CreatePvTFight(Map map)
        {
            var redTeam = new FightPlayerTeam(0, map.GetRedFightPlacement());
            var blueTeam = new FightTaxCollectorTeam(1, map.GetBlueFightPlacement());

            var fight = new FightPvT(m_idProvider.Pop(), map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public void Remove(Fight fight)
        {
            RemoveEntity(fight.Id);

            m_idProvider.Push(fight.Id);
        }

        public Fight GetFight(int id)
        {
            return GetEntityOrDefault(id);
        }
    }
}