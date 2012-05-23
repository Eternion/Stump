using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Armor
{
    [EffectHandler(EffectsEnum.Effect_AddArmorDamageReduction)]
    [EffectHandler(EffectsEnum.Effect_AddGlobalDamageReduction_105)]
    public class DamageArmor : SpellEffectHandler
    {
        public DamageArmor(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return false;

            foreach (FightActor actor in GetAffectedActors())
            {
                if (Effect.Duration <= 0)
                    return false;

                AddTriggerBuff(actor, true, BuffTriggerType.BUFF_ADDED, ApplyArmorBuff, RemoveArmorBuff);
            }

            return true;
        }

        public static void ApplyArmorBuff(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var integerEffect = buff.Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            foreach (var caracteristic in GetAssociatedCaracteristics(buff.Spell.Id))
            {
                buff.Target.Stats[caracteristic].Context += buff.Target.CalculateArmorValue(integerEffect.Value, GetEffectSchool(buff.Spell.Id));
            }
        }

        public static void RemoveArmorBuff(TriggerBuff buff)
        {
            var integerEffect = buff.Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            foreach (var caracteristic in GetAssociatedCaracteristics(buff.Spell.Id))
            {
                buff.Target.Stats[caracteristic].Context -= buff.Target.CalculateArmorValue(integerEffect.Value, GetEffectSchool(buff.Spell.Id));
            }
        }

        public static EffectSchoolEnum GetEffectSchool(int spellId)
        {
            // ugly ...
            switch ((SpellIdEnum)spellId)
            {
                case SpellIdEnum.AqueousArmour:
                case SpellIdEnum.AqueousArmour_50:
                    return EffectSchoolEnum.Water;
                case SpellIdEnum.EarthArmour:
                case SpellIdEnum.EarthArmour_52:
                    return EffectSchoolEnum.Earth; // and neutral
                case SpellIdEnum.WindArmour:
                case SpellIdEnum.WindArmour_53:
                    return EffectSchoolEnum.Air;
                case SpellIdEnum.GlowingArmour:
                case SpellIdEnum.GlowingArmour_51:
                case SpellIdEnum.Mummification:
                    return EffectSchoolEnum.Fire;
                default:
                    return EffectSchoolEnum.Fire;
            }
        }


        public static IEnumerable<PlayerFields> GetAssociatedCaracteristics(int spellId)
        {
            switch ((SpellIdEnum)spellId)
            {
                case SpellIdEnum.AqueousArmour:
                case SpellIdEnum.AqueousArmour_50:
                    yield return PlayerFields.WaterDamageArmor;
                    break;
                case SpellIdEnum.EarthArmour:
                case SpellIdEnum.EarthArmour_52:
                    yield return PlayerFields.EarthDamageArmor;
                    yield return PlayerFields.NeutralDamageArmor;
                    break;
                case SpellIdEnum.WindArmour:
                case SpellIdEnum.WindArmour_53:
                    yield return PlayerFields.AirDamageArmor;
                    break;
                case SpellIdEnum.GlowingArmour:
                case SpellIdEnum.GlowingArmour_51:
                    yield return PlayerFields.FireDamageArmor;
                    break;
                default:
                    yield return PlayerFields.GlobalDamageReduction;
                    break;
            }
        }
    }
}