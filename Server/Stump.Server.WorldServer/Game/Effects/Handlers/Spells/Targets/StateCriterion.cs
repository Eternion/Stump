using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class StateCriterion : TargetCriterion
    {
        public StateCriterion(int state, bool caster, bool required)
        {
            State = state;
            Caster = caster;
            Required = required;
        }

        public int State
        {
            get;
            set;
        }

        public bool Caster
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public override bool IsDisjonction => false;

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            if (Caster)
                return Required ? handler.Caster.HasState(State) : !handler.Caster.HasState(State);

            return Required ? actor.HasState(State) : !actor.HasState(State);
        }
    }
}
