using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightDuel : Fight
    {
        public FightDuel(int id, Map fightMap, FightTeam blueTeam, FightTeam redTeam)
            : base(id, fightMap, blueTeam, redTeam)
        {
        }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_CHALLENGE; }
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            return Fighters.Select(fighter => fighter.GetFightResult());
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, State == FightState.Placement, State == FightState.Placement, false, State == FightState.Fighting, 0, FightType);
        }

        protected override bool CanCancelFight()
        {
            return State == FightState.Placement;
        }
    }
}