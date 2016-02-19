using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageNeutralRemainingMP)]
    [EffectHandler(EffectsEnum.Effect_DamageAirRemainingMP)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterRemainingMP)]
    [EffectHandler(EffectsEnum.Effect_DamageFireRemainingMP)]
    [EffectHandler(EffectsEnum.Effect_DamageEarthRemainingMP)]
    public class DamagePerRemainingMP : SpellEffectHandler
    {
        public DamagePerRemainingMP(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
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
                    School = GetEffectSchool(Dice.EffectId),
                    MarkTrigger = MarkTrigger,
                    IsCritical = Critical
                };

                var percent = ((double)Caster.Stats.MP.Total / Caster.Stats.MP.TotalMax) * 100.0;
                damages.BaseMaxDamages = (int)Math.Floor(damages.BaseMaxDamages * (percent / 100.0));
                damages.BaseMinDamages = (int)Math.Floor(damages.BaseMinDamages * (percent / 100.0));

                actor.InflictDamage(damages);
            }

            return true;
        }

        static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWaterRemainingMP:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_DamageEarthRemainingMP:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_DamageAirRemainingMP:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_DamageFireRemainingMP:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_DamageNeutralRemainingMP:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}
