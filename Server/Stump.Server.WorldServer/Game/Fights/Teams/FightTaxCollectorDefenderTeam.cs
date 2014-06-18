using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Fights.Teams
{
    public class FightTaxCollectorDefenderTeam : FightTeamWithLeader<TaxCollectorFighter>
    {
        public FightTaxCollectorDefenderTeam(TeamEnum id, Cell[] placementCells) : base(id, placementCells)
        {
        }

        public FightTaxCollectorDefenderTeam(TeamEnum id, Cell[] placementCells, AlignmentSideEnum alignmentSide)
            : base(id, placementCells, alignmentSide)
        {
        }

        public override TeamTypeEnum TeamType
        {
            get { return TeamTypeEnum.TEAM_TYPE_TAXCOLLECTOR; }
        }

        public override FighterRefusedReasonEnum CanJoin(Character character)
        {
            return character.Guild != Leader.TaxCollectorNpc.Guild ? FighterRefusedReasonEnum.WRONG_GUILD : base.CanJoin(character);
        }
    }
}