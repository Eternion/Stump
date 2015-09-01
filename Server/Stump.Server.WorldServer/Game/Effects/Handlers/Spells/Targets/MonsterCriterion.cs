using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class MonsterCriterion : TargetCriterion
    {
        public MonsterCriterion(int monsterId, bool required)
        {
            MonsterId = monsterId;
            Required = required;
        }

        public int MonsterId
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            return Required ? (actor is MonsterFighter) && (actor as MonsterFighter).Monster.Template.Id == MonsterId :
                !(actor is MonsterFighter) || (actor as MonsterFighter).Monster.Template.Id != MonsterId;
        }
    }
}
