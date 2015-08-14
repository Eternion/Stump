using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

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
    [EffectHandler(EffectsEnum.Effect_SubDodge)]
    [EffectHandler(EffectsEnum.Effect_SubLock)]
    [EffectHandler(EffectsEnum.Effect_SubDodgeAPProbability)]
    [EffectHandler(EffectsEnum.Effect_SubDodgeMPProbability)]
    [EffectHandler(EffectsEnum.Effect_SubAPAttack)]
    [EffectHandler(EffectsEnum.Effect_SubMPAttack)]
    public class StatsDebuff : SpellEffectHandler
    {
        public StatsDebuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

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
                case EffectsEnum.Effect_SubRange_135:
                    return PlayerFields.Range;
                case EffectsEnum.Effect_SubCriticalHit:
                    return PlayerFields.CriticalHit;
                case EffectsEnum.Effect_SubDamageBonus:
                    return PlayerFields.DamageBonus;
                case EffectsEnum.Effect_SubDamageBonusPercent:
                    return PlayerFields.DamageBonusPercent;
                case EffectsEnum.Effect_SubDodge:
                    return PlayerFields.TackleEvade;
                case EffectsEnum.Effect_SubLock:
                    return PlayerFields.TackleBlock;
                case EffectsEnum.Effect_SubDodgeAPProbability:
                    return PlayerFields.DodgeAPProbability;
                case EffectsEnum.Effect_SubDodgeMPProbability:
                    return PlayerFields.DodgeMPProbability;
                case EffectsEnum.Effect_SubVitality:
                    return PlayerFields.Vitality;
                case EffectsEnum.Effect_SubAPAttack:
                    return PlayerFields.APAttack;
                case EffectsEnum.Effect_SubMPAttack:
                    return PlayerFields.MPAttack;

                default:
                    throw new Exception(string.Format("'{0}' has no binded caracteristic", effect));
            }
        }
    }
}