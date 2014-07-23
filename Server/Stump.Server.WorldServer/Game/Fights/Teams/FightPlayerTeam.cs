using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Fights.Teams
{
    public class FightPlayerTeam : FightTeamWithLeader<CharacterFighter>
    {
        public FightPlayerTeam(TeamEnum id, Cell[] placementCells) : base(id, placementCells)
        {
        }

        public FightPlayerTeam(TeamEnum id, Cell[] placementCells, AlignmentSideEnum alignmentSide)
            : base(id, placementCells, alignmentSide)
        {
        }

        public override TeamTypeEnum TeamType
        {
            get { return TeamTypeEnum.TEAM_TYPE_PLAYER; }
        }

        public override FighterRefusedReasonEnum CanJoin(Character character)
        {
            if (IsRestrictedToParty)
            {
                if (!Leader.Character.IsInParty(PartyTypeEnum.PARTY_TYPE_CLASSICAL) || !Leader.Character.Party.IsInGroup(character))
                    return FighterRefusedReasonEnum.TEAM_LIMITED_BY_MAINCHARACTER;
            }

            return base.CanJoin(character);
        }
    }
}