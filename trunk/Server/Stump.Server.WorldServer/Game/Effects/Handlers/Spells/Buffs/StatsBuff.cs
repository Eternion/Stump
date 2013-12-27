using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

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
    [EffectHandler(EffectsEnum.Effect_AddCriticalHit)]
    [EffectHandler(EffectsEnum.Effect_AddSummonLimit)]
    [EffectHandler(EffectsEnum.Effect_AddDamageBonus)]
    [EffectHandler(EffectsEnum.Effect_AddDamageBonus_121)]
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
    public class StatsBuff : SpellEffectHandler
    {
        public StatsBuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                if (Effect.Duration > 0)
                {
                    AddStatBuff(actor, integerEffect.Value, GetEffectCaracteristic(Effect.EffectId), true);
                }
            }

            return true;
        }

        public static PlayerFields GetEffectCaracteristic(EffectsEnum effect)
        {
            switch (effect)
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
                case EffectsEnum.Effect_AddCriticalHit:
                    return PlayerFields.CriticalHit;
                case EffectsEnum.Effect_AddSummonLimit:
                    return PlayerFields.SummonLimit;
                case EffectsEnum.Effect_AddDamageBonus:
                case EffectsEnum.Effect_AddDamageBonus_121:
                    return PlayerFields.DamageBonus;
                case EffectsEnum.Effect_IncreaseDamage_138:
                case EffectsEnum.Effect_IncreaseDamage_1054:
                case EffectsEnum.Effect_AddDamageBonusPercent:
                    return PlayerFields.DamageBonusPercent;
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
                default:
                    throw new Exception(string.Format("'{0}' has no binded caracteristic", effect));
            }
        }
    }
}