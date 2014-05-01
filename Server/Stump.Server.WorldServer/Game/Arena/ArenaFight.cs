using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaFight : Fight<ArenaTeam, ArenaTeam>
    {
        public ArenaFight(int id, Map fightMap, ArenaTeam blueTeam, ArenaTeam redTeam)
            : base(id, fightMap, blueTeam, redTeam)
        {
        }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_PVP_ARENA; }
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            throw new System.NotImplementedException();
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            throw new System.NotImplementedException();
        }

        protected override void SendGameFightJoinMessage(FightSpectator spectator)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CanCancelFight()
        {
            throw new System.NotImplementedException();
        }
    }
}