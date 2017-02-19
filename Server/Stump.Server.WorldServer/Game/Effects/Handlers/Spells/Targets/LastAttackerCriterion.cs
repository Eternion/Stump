using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class LastAttackerCriterion : TargetCriterion
    {
        public LastAttackerCriterion(bool required)
        {
            Required = required;
        }

        public bool Required
        {
            get;
            set;
        }

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            return Required ? handler.Caster.LastAttacker == actor : handler.Caster.LastAttacker != actor;
        }
    }
}
