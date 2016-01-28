using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class UnknownCriterion : TargetCriterion
    {
        public UnknownCriterion(string criterion)
        {
            Criterion = criterion;
        }

        public string Criterion
        {
            get;
            set;
        }

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            return true;
        }
    }
}
