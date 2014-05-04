using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaFight : Fight<ArenaTeam, ArenaTeam>
    {
        public event Action<ArenaFight, Character> FightDenied;

        protected virtual void OnFightDenied(Character character)
        {
            Action<ArenaFight, Character> handler = FightDenied;
            if (handler != null) handler(this, character);
        }

        public ArenaFight(int id, Map fightMap, ArenaTeam blueTeam, ArenaTeam redTeam)
            : base(id, fightMap, blueTeam, redTeam)
        {
        }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_PVP_ARENA; }
        }

        public void DenyFight(Character character)
        {
            if (State != FightState.NotStarted)
                throw new Exception("DenyFight() : State != FightState.NotStarted");

            OnFightDenied(character);
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            yield break;
        }

        protected override bool CanCancelFight()
        {
            return false;
        }
    }
}