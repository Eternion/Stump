using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells.Steals
{
    [EffectHandler(EffectsEnum.Effect_StealChance)]
    [EffectHandler(EffectsEnum.Effect_StealVitality)]
    [EffectHandler(EffectsEnum.Effect_StealWisdom)]
    [EffectHandler(EffectsEnum.Effect_StealIntelligence)]
    [EffectHandler(EffectsEnum.Effect_StealAgility)]
    [EffectHandler(EffectsEnum.Effect_StealStrength)]
    [EffectHandler(EffectsEnum.Effect_StealRange)]
    public class StatsSteal : SpellEffectHandler
    {
        public StatsSteal(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override void Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

                if (integerEffect == null)
                    return;

                AddStatBuff(actor, (short)( -integerEffect.Value ), GetEffectCaracteristic(Effect.EffectId), true);
                AddStatBuff(Caster, integerEffect.Value, GetEffectCaracteristic(Effect.EffectId), true);
            }
        }

        private static CaracteristicsEnum GetEffectCaracteristic(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_StealChance:
                    return CaracteristicsEnum.Chance;
                case EffectsEnum.Effect_StealVitality:
                    return CaracteristicsEnum.Vitality;
                case EffectsEnum.Effect_StealWisdom:
                    return CaracteristicsEnum.Wisdom;
                case EffectsEnum.Effect_StealIntelligence:
                    return CaracteristicsEnum.Intelligence;
                case EffectsEnum.Effect_StealAgility:
                    return CaracteristicsEnum.Agility;
                case EffectsEnum.Effect_StealStrength:
                    return CaracteristicsEnum.Strength;
                case EffectsEnum.Effect_StealRange:
                    return CaracteristicsEnum.Range;
                default:
                    throw new Exception("No associated caracteristic to effect '" + effect + "'");
            }
        }
    }
}