using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Steals
{
    [EffectHandler(EffectsEnum.Effect_StealHPWater)]
    [EffectHandler(EffectsEnum.Effect_StealHPEarth)]
    [EffectHandler(EffectsEnum.Effect_StealHPAir)]
    [EffectHandler(EffectsEnum.Effect_StealHPFire)]
    [EffectHandler(EffectsEnum.Effect_StealHPNeutral)]
    public class HPSteal : SpellEffectHandler
    {
        public HPSteal(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

                if (integerEffect == null)
                    return false;

                if (Effect.Duration > 0)
                {
                    AddTriggerBuff(actor, true, BuffTriggerType.TURN_BEGIN, StealHpBuffTrigger);
                }
                else
                {
                    var damage = actor.InflictDamage(integerEffect.Value, GetEffectSchool(Effect.EffectId), Caster, actor is CharacterFighter);

                    if (damage / 2 > 0)
                        Caster.HealDirect((short)( damage / 2d ), actor);
                }
            }

            return true;
        }


        private static void StealHpBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var integerEffect = buff.Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            buff.Target.Heal(integerEffect.Value, buff.Caster);
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_StealHPWater:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_StealHPEarth:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_StealHPAir:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_StealHPFire:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_StealHPNeutral:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}