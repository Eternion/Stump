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
    [EffectHandler(EffectsEnum.Effect_DamageWater)]
    [EffectHandler(EffectsEnum.Effect_DamageEarth)]
    [EffectHandler(EffectsEnum.Effect_DamageAir)]
    [EffectHandler(EffectsEnum.Effect_DamageFire)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutral)]
    public class DirectDamage : SpellEffectHandler
    {
        public DirectDamage(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override void Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

                if (integerEffect == null)
                    return;

                if (Effect.Duration > 0)
                {
                    AddTriggerBuff(actor, true, BuffTriggerType.TURN_BEGIN, DamageBuffTrigger);
                }
                else
                {
                    // spell reflected
                    var buff = actor.GetBestReflectionBuff();
                    if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel)
                    {
                        NotifySpellReflected(actor);
                        Caster.InflictDamage(integerEffect.Value, GetEffectSchool(integerEffect.EffectId), Caster, Caster is CharacterFighter, Spell);

                        actor.RemoveAndDispellBuff(buff);
                    }
                    else
                    {
                        short inflictedDamage = actor.InflictDamage(integerEffect.Value, GetEffectSchool(integerEffect.EffectId), Caster, actor is CharacterFighter, Spell);
                    }
                }
            }
        }

        private void NotifySpellReflected(FightActor source)
        {
            Fight.ForEach(entry => ActionsHandler.SendGameActionFightReflectSpellMessage(entry.Client, source, Caster));
        }

        private static void DamageBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var integerEffect = buff.Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            buff.Target.InflictDamage(integerEffect.Value, GetEffectSchool(integerEffect.EffectId), buff.Caster, buff.Target is CharacterFighter, buff.Spell);
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWater:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_DamageEarth:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_DamageAir:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_DamageFire:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_DamageNeutral:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}