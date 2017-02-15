using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Fights.Teams
{
    public class FightTaxCollectorAttackersTeam : FightTeamWithLeader<CharacterFighter>
    {
        public FightTaxCollectorAttackersTeam(TeamEnum id, Cell[] placementCells)
            : base(id, placementCells)
        {
        }

        public FightTaxCollectorAttackersTeam(TeamEnum id, Cell[] placementCells, AlignmentSideEnum alignmentSide)
            : base(id, placementCells, alignmentSide)
        {
        }

        public override TeamTypeEnum TeamType
        {
            get { return TeamTypeEnum.TEAM_TYPE_PLAYER; }
        }

        public override FighterRefusedReasonEnum CanJoin(Character character)
        {
            if (Fight is FightPvT fightPvT && character.Guild == fightPvT.TaxCollector.TaxCollectorNpc.Guild)
                return FighterRefusedReasonEnum.WRONG_GUILD;

            if (Fighters.Where(x => x is CharacterFighter).Any(x => (x as CharacterFighter).Character.Client.IP == character.Client.IP))
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;

            if (Fighters.Count >= FightPvT.PvTMaxFightersSlots)
                return FighterRefusedReasonEnum.TEAM_FULL;

            return base.CanJoin(character);
        }

        public override FightOutcomeEnum GetOutcome()
        {
            if ((Fight as FightPvT).TaxCollector.IsDead())
                return FightOutcomeEnum.RESULT_VICTORY;

            return FightOutcomeEnum.RESULT_LOST;
        }
    }
}