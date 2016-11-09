using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells.Casts;
using System;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_TransmitCharacteristic)]
    public class TransmitCharacteristic : SpellEffectHandler
    {
        public TransmitCharacteristic(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                PlayerFields field;

                switch (Dice.DiceNum)
                {
                    case (short)CharacteristicEnum.AGILITY:
                        field = PlayerFields.Agility;
                        break;
                    case (short)CharacteristicEnum.AIR_DAMAGE_FIXED:
                        field = PlayerFields.AirDamageBonus;
                        break;
                    case (short)CharacteristicEnum.EARTH_DAMAGE_FIXED:
                        field = PlayerFields.EarthDamageBonus;
                        break;
                    case (short)CharacteristicEnum.NEUTRAL_DAMAGE_FIXED:
                        field = PlayerFields.NeutralDamageBonus;
                        break;
                    case (short)CharacteristicEnum.FIRE_DAMAGE_FIXED:
                        field = PlayerFields.FireDamageBonus;
                        break;
                    case (short)CharacteristicEnum.INTELLIGENCE:
                        field = PlayerFields.Intelligence;
                        break;
                    case (short)CharacteristicEnum.HEALS:
                        field = PlayerFields.HealBonus;
                        break;
                    default:
                        return false;
                }

                var amount = (short)Math.Floor(Caster.Stats[field].TotalSafe * (integerEffect.Value / 100d));
                AddStatBuff(actor, amount, field);
            }

            return true;
        }
    }
}
