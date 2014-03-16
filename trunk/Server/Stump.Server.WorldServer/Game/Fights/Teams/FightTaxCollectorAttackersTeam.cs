using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Fights.Teams
{
    public class FightTaxCollectorAttackersTeam : FightTeamWithLeader<CharacterFighter>
    {
        public FightTaxCollectorAttackersTeam(sbyte id, Cell[] placementCells) : base(id, placementCells)
        {
        }

        public FightTaxCollectorAttackersTeam(sbyte id, Cell[] placementCells, AlignmentSideEnum alignmentSide)
            : base(id, placementCells, alignmentSide)
        {
        }

        public override TeamTypeEnum TeamType
        {
            get { return TeamTypeEnum.TEAM_TYPE_PLAYER; }
        }

        public override FighterRefusedReasonEnum CanJoin(Character character)
        {
            if (Fight is FightPvT && character.Guild == (Fight as FightPvT).TaxCollector.TaxCollectorNpc.Guild)
                return FighterRefusedReasonEnum.WRONG_GUILD;

            return base.CanJoin(character);
        }
    }
}