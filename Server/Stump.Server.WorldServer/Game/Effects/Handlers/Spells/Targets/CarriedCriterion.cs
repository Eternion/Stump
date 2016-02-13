using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class CarriedCriterion : TargetCriterion
    {
        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler) => actor.IsCarried();
    }
}
