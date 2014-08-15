using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageAirPerMP)] 
    [EffectHandler(EffectsEnum.Effect_DamageEarthPerMP)]
    [EffectHandler(EffectsEnum.Effect_DamageFirePerMP)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterPerMP)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutralPerMP)]
    public class DamagePerMPUsed : SpellEffectHandler
    {
        public DamagePerMPUsed(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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
            var damages = new Fights.Damage(Dice)
            {
                Source = buff.Caster,
                Buff = buff,
                IgnoreDamageReduction = true,
                School = GetEffectSchool(buff.Dice.EffectId),
                MarkTrigger = MarkTrigger,
                IsCritical = Critical
            };

            damages.BaseMaxDamages = buff.Target.UsedMP * damages.BaseMaxDamages; 
            damages.BaseMinDamages = buff.Target.UsedMP * damages.BaseMinDamages;

            buff.Target.InflictDamage(damages);
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_DamageWaterPerMP:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_DamageEarthPerMP:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_DamageAirPerMP:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_DamageFirePerMP:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_DamageNeutralPerMP:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}