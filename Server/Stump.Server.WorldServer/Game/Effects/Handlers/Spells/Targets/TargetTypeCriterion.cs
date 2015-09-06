using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class TargetTypeCriterion : TargetCriterion
    {
        public TargetTypeCriterion(SpellTargetType type)
        {
            TargetType = type;
        }

        public SpellTargetType TargetType
        {
            get;
            set;
        }

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            if (TargetType == SpellTargetType.NONE)
                // return false; note : wtf, why is there spells with TargetType = NONE ?
                return true;

            if (handler.Caster == actor && (TargetType.HasFlag(SpellTargetType.SELF) || TargetType.HasFlag(SpellTargetType.SELF_ONLY)))
                return true;

            if (TargetType.HasFlag(SpellTargetType.SELF_ONLY) && actor != handler.Caster)
                return false;

            if (handler.Caster.IsFriendlyWith(actor) && handler.Caster != actor)
            {
                if (TargetType == SpellTargetType.ALLY_ALL || TargetType.HasFlag(SpellTargetType.ALLY_PLAYER)
                    || TargetType.HasFlag(SpellTargetType.ALLY_UNKN_1) || TargetType.HasFlag(SpellTargetType.ALLY_UNKN_2)) // not sure about that
                    return true;

                if ((TargetType.HasFlag(SpellTargetType.ALLY_MONSTER)) && (actor is MonsterFighter))
                    return true;

                if ((TargetType.HasFlag(SpellTargetType.ALLY_MONSTER_SUMMON) || TargetType.HasFlag(SpellTargetType.ALLY_NON_MONSTER_SUMMON))
                    && actor is SummonedFighter)
                    return true;

                if (TargetType.HasFlag(SpellTargetType.ALLY_BOMB)
                    && actor is SummonedBomb)
                    return true;
            }

            if (!handler.Caster.IsEnnemyWith(actor))
                return false;


            if (TargetType == SpellTargetType.ENEMY_ALL || TargetType.HasFlag(SpellTargetType.ENEMY_PLAYER)
                || TargetType.HasFlag(SpellTargetType.ENEMY_UNKN_1) || TargetType.HasFlag(SpellTargetType.ENEMY_UNKN_2)) // not sure about that
                return true;

            if ((TargetType.HasFlag(SpellTargetType.ENEMY_MONSTER)) && (actor is MonsterFighter))
                return true;

            if ((TargetType.HasFlag(SpellTargetType.ENEMY_MONSTER_SUMMON) || TargetType.HasFlag(SpellTargetType.ENEMY_NON_MONSTER_SUMMON))
                && actor is SummonedFighter)
                return true;

            if (TargetType.HasFlag(SpellTargetType.ENEMY_BOMB)
                && actor is SummonedBomb)
                return true;

            return false;
        }
    }
}
