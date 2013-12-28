using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageAirPerAP)] 
    [EffectHandler(EffectsEnum.Effect_DamageEarthPerAP)]
    [EffectHandler(EffectsEnum.Effect_DamageFirePerAP)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterPerAP)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutralPerAP)]
    public class DamagePerAPUsed : SpellEffectHandler
    {
        public DamagePerAPUsed(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var buff = AddTriggerBuff(actor, true, BuffTriggerType.TURN_END, OnBuffTriggered);
            }

            return true;
        }

        private void OnBuffTriggered(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var effect = buff.Dice.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (effect == null)
                return;

            buff.Target.InflictDamage(effect.Value*buff.Target.UsedAP, GetEffectSchool(Effect.EffectId), Caster,
                Caster is CharacterFighter, Spell, true, false);
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWaterPerAP:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_DamageEarthPerAP:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_DamageAirPerAP:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_DamageFirePerAP:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_DamageNeutralPerAP:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}