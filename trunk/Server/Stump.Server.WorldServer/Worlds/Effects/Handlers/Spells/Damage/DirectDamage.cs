using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageWater)]
    [EffectHandler(EffectsEnum.Effect_DamageEarth)]
    [EffectHandler(EffectsEnum.Effect_DamageAir)]
    [EffectHandler(EffectsEnum.Effect_DamageFire)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutral)]
    public class DirectDamage : SpellEffectHandler
    {
        public DirectDamage(EffectBase effect, FightActor caster, Spell spell, Cell targetedCell)
            : base(effect, caster, spell, targetedCell)
        {
        }

        public override void Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            if (Effect.Duration > 0)
            {
                // todo : add buff
                return;
            }

            foreach (FightActor actor in GetAffectedActors())
            {
                var inflictedDamage = actor.InflictDamage(integerEffect.Value, GetEffectSchool(integerEffect.EffectId), Caster, actor is CharacterFighter);

                // todo : reflected damage ?
            }
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWater:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_DamageEarth:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_DamageAir:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_DamageFire:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_DamageNeutral:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}