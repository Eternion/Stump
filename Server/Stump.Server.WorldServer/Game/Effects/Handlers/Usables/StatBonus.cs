using System;
using System.Drawing;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
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

            var characteristic = GetEffectCharacteristic(Effect.EffectId);
            var bonus = AdjustBonusStat((short) (effect.Value * NumberOfUses), characteristic);

            if (bonus == 0 || bonus + Target.Stats[characteristic].Additional > StatBonusLimit)
            {
                Target.SendServerMessage(string.Format("Bonus limit reached : {0}", StatBonusLimit), Color.Red);
                return false;
            }

            Target.Stats[characteristic].Additional += bonus;
            Target.RefreshStats();

            UsedItems = (uint) Math.Ceiling((double) bonus/effect.Value);

            return true;
        }

        static PlayerFields GetEffectCharacteristic(EffectsEnum effect)
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

        short AdjustBonusStat(short bonus, PlayerFields characteristic)
        {
            if (Target.Stats[characteristic].Additional >= StatBonusLimit)
                return 0;

            return Target.Stats[characteristic].Additional + bonus > StatBonusLimit ? StatBonusLimit : bonus;
        }
    }
}