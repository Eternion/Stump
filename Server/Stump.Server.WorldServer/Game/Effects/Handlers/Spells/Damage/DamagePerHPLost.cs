using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageAirPerHPLost)]
    [EffectHandler(EffectsEnum.Effect_DamageEarthPerHPLost)]
    [EffectHandler(EffectsEnum.Effect_DamageFirePerHPLost)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterPerHPLost)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutralPerHPLost)]
    public class DamagePerHPLost : SpellEffectHandler
    {
        public DamagePerHPLost(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var damages = new Fights.Damage(Dice)
                {
                    Source = Caster,
                    IgnoreDamageReduction = true,
                    IgnoreDamageBoost = true,
                    School = GetEffectSchool(Dice.EffectId),
                    MarkTrigger = MarkTrigger,
                    IsCritical = Critical
                };

                var damagesAmount = Math.Round(((Caster.Stats.Health.DamageTaken * Dice.DiceNum) / 100d));

                damages.Amount = (int)damagesAmount;

                actor.InflictDamage(damages);
            }

            return true;
        }

        static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWaterPerHPLost:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_DamageEarthPerHPLost:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_DamageAirPerHPLost:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_DamageFirePerHPLost:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_DamageNeutralPerHPLost:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}
