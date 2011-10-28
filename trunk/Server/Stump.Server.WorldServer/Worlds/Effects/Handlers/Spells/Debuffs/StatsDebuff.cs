using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubAgility)]
    [EffectHandler(EffectsEnum.Effect_SubChance)]
    [EffectHandler(EffectsEnum.Effect_SubIntelligence)]
    [EffectHandler(EffectsEnum.Effect_SubStrength)]
    [EffectHandler(EffectsEnum.Effect_SubWisdom)]
    [EffectHandler(EffectsEnum.Effect_SubVitality)]
    [EffectHandler(EffectsEnum.Effect_SubRange)]
    [EffectHandler(EffectsEnum.Effect_SubRange_135)]
    [EffectHandler(EffectsEnum.Effect_SubCriticalHit)]
    [EffectHandler(EffectsEnum.Effect_SubDamageBonus)]
    [EffectHandler(EffectsEnum.Effect_SubDamageBonusPercent)]
    public class StatsDebuff : SpellEffectHandler
    {
        public StatsDebuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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
                    AddStatBuff(actor, (short) (-integerEffect.Value), GetEffectCaracteristic(Effect.EffectId), true);
                }
            }
        }

        public static CaracteristicsEnum GetEffectCaracteristic(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_SubAgility:
                    return CaracteristicsEnum.Agility;
                case EffectsEnum.Effect_SubChance:
                    return CaracteristicsEnum.Chance;
                case EffectsEnum.Effect_SubIntelligence:
                    return CaracteristicsEnum.Intelligence;
                case EffectsEnum.Effect_SubStrength:
                    return CaracteristicsEnum.Strength;
                case EffectsEnum.Effect_SubWisdom:
                    return CaracteristicsEnum.Wisdom;
                case EffectsEnum.Effect_SubRange:
                    return CaracteristicsEnum.Range;
                case EffectsEnum.Effect_SubCriticalHit:
                    return CaracteristicsEnum.CriticalHit;
                case EffectsEnum.Effect_SubDamageBonus:
                    return CaracteristicsEnum.DamageBonus;
                case EffectsEnum.Effect_IncreaseDamage_138:
                case EffectsEnum.Effect_SubDamageBonusPercent:
                    return CaracteristicsEnum.DamageBonusPercent;

                default:
                    throw new Exception(string.Format("'{0}' has no binded caracteristic", effect));
            }
        }
    }
}