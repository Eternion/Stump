using System.Linq;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class JustSummonedCriterion : TargetCriterion
    {
        public override bool CheckWhenExecute => true;

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            return actor.IsSummoned() && handler.CastHandler.GetEffectHandlers()
                .Any(x => actor.SummoningEffect == x);
        }
    }
}