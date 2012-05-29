using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamagePercentAir)]
    [EffectHandler(EffectsEnum.Effect_DamagePercentEarth)]
    [EffectHandler(EffectsEnum.Effect_DamagePercentFire)]
    [EffectHandler(EffectsEnum.Effect_DamagePercentWater)]
    [EffectHandler(EffectsEnum.Effect_DamagePercentNeutral)]
    public class DamagePercent : SpellEffectHandler
    {
        public DamagePercent(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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
                    AddTriggerBuff(actor, true, BuffTriggerType.TURN_BEGIN, DamageBuffTrigger);
                }
                else
                {
                    var damage = (short)( actor.MaxLifePoints * ( integerEffect.Value / 100d ) );

                    // spell reflected
                    var buff = actor.GetBestReflectionBuff();
                    if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel)
                    {
                        NotifySpellReflected(actor);
                        Caster.InflictNoBoostedDamage(damage, GetEffectSchool(integerEffect.EffectId), Caster, Caster is CharacterFighter, Spell);

                        actor.RemoveAndDispellBuff(buff);
                    }
                    else
                    {
                        short inflictedDamage = actor.InflictNoBoostedDamage(damage, GetEffectSchool(integerEffect.EffectId), Caster, actor is CharacterFighter, Spell);
                    }
                }
            }

            return true;
        }

        private void NotifySpellReflected(FightActor source)
        {
            ActionsHandler.SendGameActionFightReflectSpellMessage(Fight.Clients, source, Caster);
        }

        private static void DamageBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var integerEffect = buff.Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            var damage = (short)( buff.Target.MaxLifePoints * ( integerEffect.Value / 100d ) );

            buff.Target.InflictDamage(damage, GetEffectSchool(integerEffect.EffectId), buff.Caster, buff.Target is CharacterFighter, buff.Spell);
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamagePercentAir:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_DamagePercentEarth:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_DamagePercentFire:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_DamagePercentWater:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_DamagePercentNeutral:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}