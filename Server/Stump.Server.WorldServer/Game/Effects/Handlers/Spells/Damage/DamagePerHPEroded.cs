using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageAirPerHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageEarthPerHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageFirePerHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterPerHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutralPerHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageAirPerCasterHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageEarthPerCasterHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageFirePerCasterHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterPerCasterHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutralPerCasterHPEroded)]
    public class DamagePerHPEroded : SpellEffectHandler
    {
        public DamagePerHPEroded(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (Effect.Duration != 0 || Effect.Delay != 0)
                {
                    AddTriggerBuff(actor, BuffTriggerType.Instant, OnBuffTriggered);
                }
                else
                {
                    var damages = new Fights.Damage(Dice)
                    {
                        Source = Caster,
                        IgnoreDamageReduction = true,
                        IgnoreDamageBoost = true,
                        School = GetEffectSchool(Dice.EffectId),
                        MarkTrigger = MarkTrigger,
                        IsCritical = Critical,
                        Amount = (int)Math.Floor((actor.Stats.Health.PermanentDamages * Dice.DiceNum) / 100d)
                    };

                    if (Effect.EffectId == EffectsEnum.Effect_DamageNeutralPerCasterHPEroded ||
                        Effect.EffectId == EffectsEnum.Effect_DamageEarthPerCasterHPEroded ||
                        Effect.EffectId == EffectsEnum.Effect_DamageFirePerCasterHPEroded ||
                        Effect.EffectId == EffectsEnum.Effect_DamageWaterPerCasterHPEroded ||
                        Effect.EffectId == EffectsEnum.Effect_DamageAirPerCasterHPEroded)
                    {
                        damages.Amount = (int)Math.Floor((Caster.Stats.Health.PermanentDamages * Dice.DiceNum) / 100d);
                    }

                    actor.InflictDamage(damages);
                }
            }

            return true;
        }

        void OnBuffTriggered(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damages = new Fights.Damage(Dice)
            {
                Source = buff.Caster,
                Buff = buff,
                IgnoreDamageReduction = true,
                IgnoreDamageBoost = true,
                School = GetEffectSchool(buff.Dice.EffectId),
                MarkTrigger = MarkTrigger,
                IsCritical = Critical,
                Amount = (int)Math.Floor((buff.Target.Stats.Health.PermanentDamages * Dice.DiceNum) / 100d)
            };

            if (Effect.EffectId == EffectsEnum.Effect_DamageNeutralPerCasterHPEroded ||
                Effect.EffectId == EffectsEnum.Effect_DamageEarthPerCasterHPEroded ||
                Effect.EffectId == EffectsEnum.Effect_DamageFirePerCasterHPEroded ||
                Effect.EffectId == EffectsEnum.Effect_DamageWaterPerCasterHPEroded ||
                Effect.EffectId == EffectsEnum.Effect_DamageAirPerCasterHPEroded)
            {
                damages.Amount = (int)Math.Floor((buff.Caster.Stats.Health.PermanentDamages * Dice.DiceNum) / 100d);
            }

            buff.Target.InflictDamage(damages);
        }

        static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWaterPerHPEroded:
                case EffectsEnum.Effect_DamageWaterPerCasterHPEroded:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_DamageEarthPerHPEroded:
                case EffectsEnum.Effect_DamageEarthPerCasterHPEroded:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_DamageAirPerHPEroded:
                case EffectsEnum.Effect_DamageAirPerCasterHPEroded:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_DamageFirePerHPEroded:
                case EffectsEnum.Effect_DamageFirePerCasterHPEroded:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_DamageNeutralPerHPEroded:
                case EffectsEnum.Effect_DamageNeutralPerCasterHPEroded:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}