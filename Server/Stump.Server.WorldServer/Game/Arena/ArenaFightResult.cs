using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Results;
using FightLoot = Stump.Server.WorldServer.Game.Fights.Results.FightLoot;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaFightResult : FightResult<CharacterFighter>
    {
        public ArenaFightResult(CharacterFighter fighter, FightOutcomeEnum outcome, FightLoot loot, int rank)
            : base(fighter, outcome, loot)
        {
            Rank = rank;
        }

        public int Rank
        {
            get;
            private set;
        }

        public override FightResultListEntry GetFightResultListEntry()
        {
            var amount = Fighter.Character.ComputeWonArenaTokens(Rank);
            return new FightResultPlayerListEntry((short) Outcome, new DofusProtocol.Types.FightLoot(Enumerable.Repeat((short)ItemIdEnum.Kolizeton, amount), 0), Id, Alive, (byte)Level,
                new FightResultAdditionalData[0]);
        }

        public override void Apply()
        {
            Fighter.Character.UpdateArenaProperties(Rank, Outcome == FightOutcomeEnum.RESULT_VICTORY);
        }
    }
}