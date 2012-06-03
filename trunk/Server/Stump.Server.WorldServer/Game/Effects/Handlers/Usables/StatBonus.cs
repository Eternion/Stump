using System;
using System.Drawing;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_AddPermanentAgility)]
    [EffectHandler(EffectsEnum.Effect_AddPermanentStrength)]
    [EffectHandler(EffectsEnum.Effect_AddPermanentChance)]
    [EffectHandler(EffectsEnum.Effect_AddPermanentIntelligence)]
    [EffectHandler(EffectsEnum.Effect_AddPermanentWisdom)]
    [EffectHandler(EffectsEnum.Effect_AddPermanentVitality)]
    public class StatBonus : UsableEffectHandler
    {
        [Variable]
        public static short StatBonusLimit = 101;

        public StatBonus(EffectBase effect, Character target, PlayerItem item) : base(effect, target, item)
        {
        }

        public override bool Apply()
        {
            var effect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (effect == null)
                return false;

            var bonus = AdjustBonusStat(effect.Value);

            if (bonus == 0)
            {
                Target.SendServerMessage(string.Format("Bonus limit reached : {0}", StatBonusLimit), Color.Red);
                return false;
            }

            Target.Stats[GetEffectCharacteristic(Effect.EffectId)].Base += bonus;
            UpdatePermanentStatField(bonus);
            Target.RefreshStats();

            return true;
        }

        private static PlayerFields GetEffectCharacteristic(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_AddPermanentChance:
                    return PlayerFields.Chance;
                case EffectsEnum.Effect_AddPermanentAgility:
                    return PlayerFields.Agility;
                case EffectsEnum.Effect_AddPermanentIntelligence:
                    return PlayerFields.Intelligence;
                case EffectsEnum.Effect_AddPermanentStrength:
                    return PlayerFields.Strength;
                case EffectsEnum.Effect_AddPermanentWisdom:
                    return PlayerFields.Wisdom;
                case EffectsEnum.Effect_AddPermanentVitality:
                    return PlayerFields.Vitality;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated Characteristic", effect));
            }
        }

        private short AdjustBonusStat(short bonus)
        {
            short actualPts;

            switch (Effect.EffectId)
            {
                case EffectsEnum.Effect_AddPermanentChance:
                    actualPts = Target.PermanentAddedChance;
                    break;
                case EffectsEnum.Effect_AddPermanentAgility:
                    actualPts = Target.PermanentAddedAgility;
                    break;
                case EffectsEnum.Effect_AddPermanentIntelligence:
                    actualPts = Target.PermanentAddedIntelligence;
                    break;
                case EffectsEnum.Effect_AddPermanentStrength:
                    actualPts = Target.PermanentAddedStrength;
                    break;
                case EffectsEnum.Effect_AddPermanentWisdom:
                    actualPts = Target.PermanentAddedWisdom;
                    break;
                case EffectsEnum.Effect_AddPermanentVitality:
                    actualPts =  Target.PermanentAddedVitality;
                    break;
                default:
                    return 0;
            }

            if (actualPts >= StatBonusLimit)
                return 0;

            if (actualPts + bonus > StatBonusLimit)
                return (short) (actualPts - StatBonusLimit);

            return bonus;
        }

        private void UpdatePermanentStatField(short bonus)
        {

            switch (Effect.EffectId)
            {
                case EffectsEnum.Effect_AddPermanentChance:
                    Target.PermanentAddedChance += bonus;
                    break;
                case EffectsEnum.Effect_AddPermanentAgility:
                    Target.PermanentAddedAgility += bonus;
                    break;
                case EffectsEnum.Effect_AddPermanentIntelligence:
                    Target.PermanentAddedIntelligence += bonus;
                    break;
                case EffectsEnum.Effect_AddPermanentStrength:
                    Target.PermanentAddedStrength += bonus;
                    break;
                case EffectsEnum.Effect_AddPermanentWisdom:
                    Target.PermanentAddedWisdom += bonus;
                    break;
                case EffectsEnum.Effect_AddPermanentVitality:
                    Target.PermanentAddedVitality += bonus;
                    break;
            }
        }
    }
}