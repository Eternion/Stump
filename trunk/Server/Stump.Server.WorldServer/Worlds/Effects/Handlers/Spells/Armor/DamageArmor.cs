using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Fights.Buffs;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells.Armor
{
    [EffectHandler(EffectsEnum.Effect_AddArmorDamageReduction)]
    public class DamageArmor : SpellEffectHandler
    {
        public DamageArmor(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override void Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            foreach (FightActor actor in GetAffectedActors())
            {
                if (Effect.Duration <= 0)
                    return;

                AddTriggerBuff(actor, true, TriggerType.BUFF_ADDED, ApplyArmorBuff, RemoveArmorBuff);
            }
        }

        public static void ApplyArmorBuff(TriggerBuff buff, TriggerType trigger)
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
            switch (spellId)
            {
                case 18:
                    return EffectSchoolEnum.Water;
                case 6:
                    return EffectSchoolEnum.Earth; // and neutral
                case 14:
                    return EffectSchoolEnum.Air;
                case 1:
                    return EffectSchoolEnum.Fire;
                default:
                    return EffectSchoolEnum.Unknown;
            }
        }


        public static IEnumerable<CaracteristicsEnum> GetAssociatedCaracteristics(int spellId)
        {
            switch (spellId)
            {
                case 18:
                    yield return CaracteristicsEnum.WaterDamageArmor;
                    break;
                case 6:
                    yield return CaracteristicsEnum.EarthDamageArmor;
                    yield return CaracteristicsEnum.NeutralDamageArmor;
                    break;
                case 14:
                    yield return CaracteristicsEnum.AirDamageArmor;
                    break;
                case 1:
                    yield return CaracteristicsEnum.FireDamageArmor;
                    break;
                default:
                    yield return CaracteristicsEnum.GlobalDamageReduction;
                    break;
            }
        }
    }
}