using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_AddAgility)]
    [EffectHandler(EffectsEnum.Effect_AddChance)]
    [EffectHandler(EffectsEnum.Effect_AddIntelligence)]
    [EffectHandler(EffectsEnum.Effect_AddStrength)]
    [EffectHandler(EffectsEnum.Effect_AddWisdom)]
    [EffectHandler(EffectsEnum.Effect_AddVitality)]
    [EffectHandler(EffectsEnum.Effect_AddRange)]
    [EffectHandler(EffectsEnum.Effect_AddRange_136)]
    [EffectHandler(EffectsEnum.Effect_AddSummonLimit)]
    [EffectHandler(EffectsEnum.Effect_AddDamageBonus)]
    [EffectHandler(EffectsEnum.Effect_AddDamageBonus_121)]
    [EffectHandler(EffectsEnum.Effect_AddHealBonus)]
    [EffectHandler(EffectsEnum.Effect_IncreaseDamage_138)]
    [EffectHandler(EffectsEnum.Effect_IncreaseDamage_1054)]
    [EffectHandler(EffectsEnum.Effect_AddDamageBonusPercent)]
    [EffectHandler(EffectsEnum.Effect_AddDamageReflection)]
    [EffectHandler(EffectsEnum.Effect_AddPhysicalDamage_137)]
    [EffectHandler(EffectsEnum.Effect_AddPhysicalDamage_142)]
    [EffectHandler(EffectsEnum.Effect_AddPhysicalDamageReduction)]
    [EffectHandler(EffectsEnum.Effect_AddMagicDamageReduction)]
    [EffectHandler(EffectsEnum.Effect_AddLock)]
    [EffectHandler(EffectsEnum.Effect_AddDodge)]
    [EffectHandler(EffectsEnum.Effect_AddDodgeAPProbability)]
    [EffectHandler(EffectsEnum.Effect_AddDodgeMPProbability)]
    [EffectHandler(EffectsEnum.Effect_AddCriticalHit)]
    [EffectHandler(EffectsEnum.Effect_AddCriticalMiss)]
    [EffectHandler(EffectsEnum.Effect_AddMPAttack)]
    [EffectHandler(EffectsEnum.Effect_AddAPAttack)]
    [EffectHandler(EffectsEnum.Effect_AddPushDamageBonus)]
    [EffectHandler(EffectsEnum.Effect_AddShield)]
    [EffectHandler(EffectsEnum.Effect_AddAirResistPercent)]
    [EffectHandler(EffectsEnum.Effect_AddFireResistPercent)]
    [EffectHandler(EffectsEnum.Effect_AddEarthResistPercent)]
    [EffectHandler(EffectsEnum.Effect_AddWaterResistPercent)]
    [EffectHandler(EffectsEnum.Effect_AddNeutralResistPercent)]
    [EffectHandler(EffectsEnum.Effect_IncreaseGlyphDamages)]
    [EffectHandler(EffectsEnum.Effect_AddWeaponDamageBonus)]
    [EffectHandler(EffectsEnum.Effect_AddProspecting)]
    [EffectHandler(EffectsEnum.Effect_IncreaseFinalDamages)]
    public class StatsBuff : SpellEffectHandler
    {
        public StatsBuff(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (var actor in GetAffectedActors())
            {
                AddStatBuff(actor, integerEffect.Value, GetEffectCaracteristic());
            }

            return true;
        }
        
        PlayerFields GetEffectCaracteristic()
        {
            switch (Effect.EffectId)
            {
                case EffectsEnum.Effect_AddVitality:
                    return PlayerFields.Vitality;
                case EffectsEnum.Effect_AddAgility:
                    return PlayerFields.Agility;
                case EffectsEnum.Effect_AddChance:
                    return PlayerFields.Chance;
                case EffectsEnum.Effect_AddIntelligence:
                    return PlayerFields.Intelligence;
                case EffectsEnum.Effect_AddStrength:
                    return PlayerFields.Strength;
                case EffectsEnum.Effect_AddWisdom:
                    return PlayerFields.Wisdom;
                case EffectsEnum.Effect_AddRange:
                case EffectsEnum.Effect_AddRange_136:
                    return PlayerFields.Range;
                case EffectsEnum.Effect_AddSummonLimit:
                    return PlayerFields.SummonLimit;
                case EffectsEnum.Effect_AddDamageBonus:
                case EffectsEnum.Effect_AddDamageBonus_121:
                    return PlayerFields.DamageBonus;
                case EffectsEnum.Effect_IncreaseDamage_138:
                case EffectsEnum.Effect_AddDamageBonusPercent:
                    return PlayerFields.DamageBonusPercent;
                case EffectsEnum.Effect_AddHealBonus:
                    return PlayerFields.HealBonus;
                case EffectsEnum.Effect_AddDamageReflection:
                    return PlayerFields.DamageReflection;
                case EffectsEnum.Effect_AddPhysicalDamage_137:
                case EffectsEnum.Effect_AddPhysicalDamage_142:
                    return PlayerFields.PhysicalDamage;
                case EffectsEnum.Effect_AddPhysicalDamageReduction:
                    return PlayerFields.PhysicalDamageReduction;
                case EffectsEnum.Effect_AddMagicDamageReduction:
                    return PlayerFields.MagicDamageReduction;
                case EffectsEnum.Effect_AddLock:
                    return PlayerFields.TackleBlock;
                case EffectsEnum.Effect_AddDodge:
                    return PlayerFields.TackleEvade;
                case EffectsEnum.Effect_AddDodgeAPProbability:
                    return PlayerFields.DodgeAPProbability;
                case EffectsEnum.Effect_AddDodgeMPProbability:
                    return PlayerFields.DodgeMPProbability;
                case EffectsEnum.Effect_AddCriticalHit:
                    return PlayerFields.CriticalHit;
                case EffectsEnum.Effect_AddCriticalMiss:
                    return PlayerFields.CriticalMiss;
                case EffectsEnum.Effect_AddMPAttack:
                    return PlayerFields.MPAttack;
                case EffectsEnum.Effect_AddAPAttack:
                    return PlayerFields.APAttack;
                case EffectsEnum.Effect_AddPushDamageBonus:
                    return PlayerFields.PushDamageBonus;
                case EffectsEnum.Effect_AddShield:
                    return PlayerFields.Shield;
                case EffectsEnum.Effect_AddAirResistPercent:
                    return PlayerFields.AirResistPercent;
                case EffectsEnum.Effect_AddFireResistPercent:
                    return PlayerFields.FireResistPercent;
                case EffectsEnum.Effect_AddEarthResistPercent:
                    return PlayerFields.EarthResistPercent;
                case EffectsEnum.Effect_AddWaterResistPercent:
                    return PlayerFields.WaterResistPercent;
                case EffectsEnum.Effect_AddNeutralResistPercent:
                    return PlayerFields.NeutralResistPercent;
                case EffectsEnum.Effect_IncreaseGlyphDamages:
                    return PlayerFields.GlyphBonusPercent;
                case EffectsEnum.Effect_AddWeaponDamageBonus:
                    return PlayerFields.WeaponDamageBonus;
                case EffectsEnum.Effect_IncreaseDamage_1054:
                    return PlayerFields.SpellDamageBonus;
                case EffectsEnum.Effect_AddProspecting:
                    return PlayerFields.Prospecting;
                case EffectsEnum.Effect_IncreaseFinalDamages:
                    return PlayerFields.DamageMultiplicator;
                default:
                    throw new Exception($"'{Effect.EffectId}' has no binded caracteristic");
            }
        }
    }
}