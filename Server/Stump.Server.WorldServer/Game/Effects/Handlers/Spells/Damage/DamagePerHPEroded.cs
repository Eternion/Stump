﻿using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageAirPerHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageEarthPerHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageFirePerHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterPerHPEroded)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutralPerHPEroded)]
    public class DamagePerHPEroded : SpellEffectHandler
    {
        public DamagePerHPEroded(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            GetAffectedActors().Select(actor => AddTriggerBuff(actor, true, BuffTriggerType.BUFF_ADDED, OnBuffTriggered));

            return true;
        }

        private void OnBuffTriggered(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damages = new Fights.Damage(Dice)
            {
                Source = buff.Caster,
                Buff = buff,
                IgnoreDamageReduction = true,
                School = GetEffectSchool(buff.Dice.EffectId),
                MarkTrigger = MarkTrigger,
                IsCritical = Critical
            };

            var damagesAmount = (buff.Target.Stats.Health.PermanentDamages*Dice.DiceNum)/100;

            damages.BaseMaxDamages = damagesAmount;
            damages.BaseMinDamages = damagesAmount;

            buff.Target.InflictDamage(damages);
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWaterPerHPEroded:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_DamageEarthPerHPEroded:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_DamageAirPerHPEroded:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_DamageFirePerHPEroded:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_DamageNeutralPerHPEroded:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}