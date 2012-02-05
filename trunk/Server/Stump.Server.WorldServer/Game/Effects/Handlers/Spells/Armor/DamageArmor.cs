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

                AddTriggerBuff(actor, true, BuffTriggerType.BUFF_ADDED, ApplyArmorBuff, RemoveArmorBuff);
            }
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


        public static IEnumerable<PlayerFields> GetAssociatedCaracteristics(int spellId)
        {
            switch (spellId)
            {
                case 18:
                    yield return PlayerFields.WaterDamageArmor;
                    break;
                case 6:
                    yield return PlayerFields.EarthDamageArmor;
                    yield return PlayerFields.NeutralDamageArmor;
                    break;
                case 14:
                    yield return PlayerFields.AirDamageArmor;
                    break;
                case 1:
                    yield return PlayerFields.FireDamageArmor;
                    break;
                default:
                    yield return PlayerFields.GlobalDamageReduction;
                    break;
            }
        }
    }
}