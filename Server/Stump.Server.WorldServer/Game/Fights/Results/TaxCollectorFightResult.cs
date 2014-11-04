using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class TaxCollectorFightResult : FightResult<TaxCollectorFighter>
    {
        public TaxCollectorFightResult(TaxCollectorFighter fighter, FightOutcomeEnum outcome, FightLoot loot)
            : base(fighter, outcome, loot)
        {
        }

        public override FightResultListEntry GetFightResultListEntry()
        {
            return new FightResultTaxCollectorListEntry((short) Outcome, 0, Loot.GetFightLoot(), Id, Alive, (byte)Level,
                Fighter.TaxCollectorNpc.Guild.GetBasicGuildInformations(), Fighter.TaxCollectorNpc.GatheredExperience);
        }
    }
}