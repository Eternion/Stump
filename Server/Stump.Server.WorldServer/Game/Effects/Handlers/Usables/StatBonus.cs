using System;
using System.Drawing;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Spells.Casts;
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

        public StatBonus(EffectBase effect, Character target, BasePlayerItem item) : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            var effect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (effect == null)
                return false;

            var bonus = AdjustBonusStat((short) (effect.Value * NumberOfUses));

            if (bonus == 0 || bonus + Target.Stats[GetEffectCharacteristic(Effect.EffectId)].Additional > StatBonusLimit)
            {
                Target.SendServerMessage(string.Format("Bonus limit reached : {0}", StatBonusLimit), Color.Red);
                return false;
            }

            Target.Stats[GetEffectCharacteristic(Effect.EffectId)].Additional += bonus;
            UsedItems = (uint) Math.Ceiling((double) bonus/effect.Value);
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
                    actualPts = (short)Target.Stats[PlayerFields.Chance].Additional;
                    break;
                case EffectsEnum.Effect_AddPermanentAgility:
                    actualPts = (short)Target.Stats[PlayerFields.Agility].Additional;
                    break;
                case EffectsEnum.Effect_AddPermanentIntelligence:
                    actualPts = (short)Target.Stats[PlayerFields.Intelligence].Additional;
                    break;
                case EffectsEnum.Effect_AddPermanentStrength:
                    actualPts = (short)Target.Stats[PlayerFields.Strength].Additional;
                    break;
                case EffectsEnum.Effect_AddPermanentWisdom:
                    actualPts = (short)Target.Stats[PlayerFields.Wisdom].Additional;
                    break;
                case EffectsEnum.Effect_AddPermanentVitality:
                    actualPts = (short)Target.Stats[PlayerFields.Vitality].Additional;
                    break;
                default:
                    return 0;
            }

            if (actualPts >= StatBonusLimit)
                return 0;

            return actualPts + bonus > StatBonusLimit ? StatBonusLimit : bonus;
        }
    }
}