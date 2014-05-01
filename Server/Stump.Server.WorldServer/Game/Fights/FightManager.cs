using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Arena;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightManager : EntityManager<FightManager, IFight>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();

        public FightDuel CreateDuel(Map map)
        {
            var redTeam = new FightPlayerTeam(0, map.GetRedFightPlacement());
            var blueTeam = new FightPlayerTeam(1, map.GetBlueFightPlacement());

            var fight = new FightDuel(m_idProvider.Pop(), map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public FightPvM CreatePvMFight(Map map)
        {
            var blueTeam = new FightPlayerTeam(0, map.GetRedFightPlacement());
            var redTeam = new FightMonsterTeam(1, map.GetBlueFightPlacement());

            var fight = new FightPvM(m_idProvider.Pop(), map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public FightAgression CreateAgressionFight(Map map, AlignmentSideEnum redAlignment, AlignmentSideEnum blueAlignment)
        {
            var redTeam = new FightPlayerTeam(0, map.GetRedFightPlacement(), redAlignment);
            var blueTeam = new FightPlayerTeam(1, map.GetBlueFightPlacement(), blueAlignment);

            var fight = new FightAgression(m_idProvider.Pop(), map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public FightPvT CreatePvTFight(Map map)
        {
            var redTeam = new FightTaxCollectorAttackersTeam(0, map.GetRedFightPlacement());
            var blueTeam = new FightTaxCollectorDefenderTeam(1, map.GetBlueFightPlacement());

            var fight = new FightPvT(m_idProvider.Pop(), map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public ArenaFight CreateArenaFight(Map map)
        {
            var redTeam = new ArenaTeam(0, map.GetRedFightPlacement());
            var blueTeam = new ArenaTeam(1, map.GetBlueFightPlacement());

            var fight = new ArenaFight(m_idProvider.Pop(), map, blueTeam, redTeam);

            AddEntity(fight.Id, fight);

            return fight;
        }

        public void Remove(IFight fight)
        {
            RemoveEntity(fight.Id);

            m_idProvider.Push(fight.Id);
        }

        public IFight GetFight(int id)
        {
            return GetEntityOrDefault(id);
        }
    }
}