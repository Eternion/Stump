using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
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

        public override bool Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

                if (integerEffect == null)
                    return false;

                if (Effect.Duration > 0)
                {
                    AddStatBuff(actor, (short) (-integerEffect.Value), GetEffectCaracteristic(Effect.EffectId), true);
                }
            }

            return true;
        }

        public static PlayerFields GetEffectCaracteristic(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_SubAgility:
                    return PlayerFields.Agility;
                case EffectsEnum.Effect_SubChance:
                    return PlayerFields.Chance;
                case EffectsEnum.Effect_SubIntelligence:
                    return PlayerFields.Intelligence;
                case EffectsEnum.Effect_SubStrength:
                    return PlayerFields.Strength;
                case EffectsEnum.Effect_SubWisdom:
                    return PlayerFields.Wisdom;
                case EffectsEnum.Effect_SubRange:
                    return PlayerFields.Range;
                case EffectsEnum.Effect_SubCriticalHit:
                    return PlayerFields.CriticalHit;
                case EffectsEnum.Effect_SubDamageBonus:
                    return PlayerFields.DamageBonus;
                case EffectsEnum.Effect_IncreaseDamage_138:
                case EffectsEnum.Effect_SubDamageBonusPercent:
                    return PlayerFields.DamageBonusPercent;

                default:
                    throw new Exception(string.Format("'{0}' has no binded caracteristic", effect));
            }
        }
    }
}