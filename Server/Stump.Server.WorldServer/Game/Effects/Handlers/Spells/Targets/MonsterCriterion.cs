using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Spells.Casts;
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
            return Required ? (actor is ICreature) && (actor as ICreature).MonsterGrade.MonsterId == MonsterId :
                !(actor is ICreature) || (actor as ICreature).MonsterGrade.MonsterId != MonsterId;
        }
    }
}
