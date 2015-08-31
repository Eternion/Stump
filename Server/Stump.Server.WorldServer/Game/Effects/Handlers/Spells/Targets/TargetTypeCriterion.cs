using Stump.DofusProtocol.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            if (handler.Caster == actor && TargetType.HasFlag(SpellTargetType.SELF))
                return true;

            if (TargetType.HasFlag(SpellTargetType.SELF_ONLY) && actor != handler.Caster)
                return false;

            if (handler.Caster.IsFriendlyWith(actor) && handler.Caster != actor)
            {
                if (TargetType == SpellTargetType.ALLY_ALL)
                    return true;

                /*if ((TargetType.HasFlag(SpellTargetType.ALLY_PLAYER) || TargetType.HasFlag(SpellTargetType.ALLY_2))
                    && !(actor is SummonedFighter)
                    && !(actor is SummonedBomb))
                    return true;

                if (TargetType.HasFlag(SpellTargetType.ALLY_SUMMONER)   
                    && handler.Caster is SummonedFighter
                    && ((SummonedFighter)handler.Caster).Summoner == actor
                    return true;

                if ((TargetType.HasFlag(SpellTargetType.ALLY_SUMMONS) || TargetType.HasFlag(SpellTargetType.ALLY_STATIC_SUMMONS))
                    && actor is SummonedFighter
                    && !(actor is SummonedTurret))
                    return true;

                if (TargetType.HasFlag(SpellTargetType.ALLY_BOMBS)
                    && actor is SummonedBomb)
                    return true;

                if (TargetType.HasFlag(SpellTargetType.ALLY_TURRETS)
                    && actor is SummonedTurret)
                    return true;*/
            }

            if (!handler.Caster.IsEnnemyWith(actor))
                return false;


            if (TargetType == SpellTargetType.ENEMY_ALL)
                return true;

            /*if ((TargetType.HasFlag(SpellTargetType.ENEMY_1) || TargetType.HasFlag(SpellTargetType.ENEMY_2))
                && !(actor is SummonedFighter)
                && !(actor is SummonedBomb)
                && !(actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_251) || actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_244)))
                return true;

            if (TargetType.HasFlag(SpellTargetType.ENEMY_SUMMONER)
                && handler.Caster is SummonedFighter && ((SummonedFighter)handler.Caster).Summoner == actor
                && !(actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_251) || actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_244)))
                return true;

            if ((TargetType.HasFlag(SpellTargetType.ENEMY_SUMMONS) || TargetType.HasFlag(SpellTargetType.ENEMY_STATIC_SUMMONS))
                && actor is SummonedFighter
                && !(actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_251) || actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_244)))
                return true;

            /*if (TargetType.HasFlag(SpellTargetType.ENEMY_BOMBS)
                && actor is SummonedBomb
                && !(actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_251) || actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_244)))
                return true;

            if (TargetType.HasFlag(SpellTargetType.ENEMY_TURRETS)
                && actor is SummonedTurret
                && !(actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_251) || actor.HasState((int)SpellStatesEnum.TÉLÉFRAG_244)))
                return true;*/
                return true;

            return false;
        }
    }
}
