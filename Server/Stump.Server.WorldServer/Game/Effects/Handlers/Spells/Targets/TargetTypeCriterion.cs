using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class TargetTypeCriterion : TargetCriterion
    {
        public TargetTypeCriterion(SpellTargetType type, bool caster)
        {
            TargetType = type;
            Caster = caster;
        }

        public SpellTargetType TargetType
        {
            get;
            set;
        }

        public bool Caster
        {
            get;
            set;
        }

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            if (Caster)
                actor = handler.Caster;

            if (TargetType == SpellTargetType.NONE)
                // return false; note : wtf, why is there spells with TargetType = NONE ?
                return true;

            if (handler.Caster == actor && (TargetType.HasFlag(SpellTargetType.SELF)
                || TargetType.HasFlag(SpellTargetType.SELF_ONLY)
                || TargetType.HasFlag(SpellTargetType.ALLY_ALL)))
                return true;

            if (TargetType.HasFlag(SpellTargetType.SELF_ONLY) && actor != handler.Caster)
                return false;

            if (handler.Caster.IsFriendlyWith(actor) && (handler.Caster != actor || Caster))
            {
                if (TargetType == SpellTargetType.ALLY_ALL_EXCEPT_SELF || TargetType == SpellTargetType.ALLY_ALL)
                    return true;

                if ((TargetType.HasFlag(SpellTargetType.ALLY_PLAYER))
                    && (actor is CharacterFighter))
                    return true;

                if ((TargetType.HasFlag(SpellTargetType.ALLY_MONSTER)) && (actor is MonsterFighter))
                    return true;

                if (TargetType.HasFlag(SpellTargetType.ALLY_SUMMON) && (actor is SummonedFighter))
                    return true;

                if (TargetType.HasFlag(SpellTargetType.ALLY_SUMMONER) && (handler.Caster is SummonedFighter) && ((SummonedFighter)handler.Caster).Summoner == actor)
                    return true;

                if ((TargetType.HasFlag(SpellTargetType.ALLY_MONSTER_SUMMON) || TargetType.HasFlag(SpellTargetType.ALLY_NON_MONSTER_SUMMON))
                    && (actor is SummonedMonster))
                    return true;
            }

            if (!handler.Caster.IsEnnemyWith(actor))
                return false;

            if (TargetType == SpellTargetType.ENEMY_ALL)
                return true;

            if ((TargetType.HasFlag(SpellTargetType.ENEMY_PLAYER) || TargetType.HasFlag(SpellTargetType.ENEMY_UNKN_1))
                && (actor is CharacterFighter))
                return true;

            if ((TargetType.HasFlag(SpellTargetType.ENEMY_MONSTER)) && (actor is MonsterFighter))
                return true;

            if (TargetType.HasFlag(SpellTargetType.ENEMY_SUMMON) && (actor is SummonedFighter))
                return true;

            if ((TargetType.HasFlag(SpellTargetType.ENEMY_MONSTER_SUMMON) || TargetType.HasFlag(SpellTargetType.ENEMY_NON_MONSTER_SUMMON))
                && (actor is SummonedMonster))
                return true;

            return false;
        }
    }
}
