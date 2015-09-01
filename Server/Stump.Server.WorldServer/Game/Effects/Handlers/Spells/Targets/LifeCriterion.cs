using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class LifeCriterion : TargetCriterion
    {
        public LifeCriterion(int lifePercent, bool mustBeGreater)
        {
            LifePercent = lifePercent;
            MustBeGreater = mustBeGreater;
        }

        public int LifePercent
        {
            get;
            set;
        }

        public bool MustBeGreater
        {
            get;
            set;
        }

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            return MustBeGreater ? actor.LifePoints / (double)actor.MaxLifePoints > LifePercent :
                actor.LifePoints / (double)actor.MaxLifePoints <= LifePercent;
        }
    }
}
