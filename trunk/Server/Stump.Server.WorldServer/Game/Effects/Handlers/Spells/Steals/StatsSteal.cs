using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Steals
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

                var displayedEffects = GetBuffDisplayedEffect(Effect.EffectId);

                AddStatBuff(actor, (short) (-(integerEffect.Value)), GetEffectCaracteristic(Effect.EffectId), true, (short)displayedEffects[1]);
                AddStatBuff(Caster, integerEffect.Value, GetEffectCaracteristic(Effect.EffectId), true, (short)displayedEffects[0]);
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

        private static EffectsEnum[] GetBuffDisplayedEffect(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_StealChance:
                    return new[] { EffectsEnum.Effect_AddChance, EffectsEnum.Effect_SubChance };
                case EffectsEnum.Effect_StealVitality:
                    return new[] { EffectsEnum.Effect_AddVitality, EffectsEnum.Effect_SubVitality };
                case EffectsEnum.Effect_StealWisdom:
                    return new[] { EffectsEnum.Effect_AddWisdom, EffectsEnum.Effect_SubWisdom };
                case EffectsEnum.Effect_StealIntelligence:
                    return new[] { EffectsEnum.Effect_AddIntelligence, EffectsEnum.Effect_SubIntelligence };
                case EffectsEnum.Effect_StealAgility:
                    return new[] { EffectsEnum.Effect_AddAgility, EffectsEnum.Effect_SubAgility };
                case EffectsEnum.Effect_StealStrength:
                    return new[] { EffectsEnum.Effect_AddStrength, EffectsEnum.Effect_SubStrength };
                case EffectsEnum.Effect_StealRange:
                    return new[] { EffectsEnum.Effect_AddRange, EffectsEnum.Effect_SubRange };
                default:
                    throw new Exception("No associated caracteristic to effect '" + effect + "'");
            }
        }
    }
}