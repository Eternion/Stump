using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageAirPerAP)] 
    [EffectHandler(EffectsEnum.Effect_DamageEarthPerAP)]
    [EffectHandler(EffectsEnum.Effect_DamageFirePerAP)]
    [EffectHandler(EffectsEnum.Effect_DamageWaterPerAP)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutralPerAP)]
    public class DamagePerAPUsed : SpellEffectHandler
    {
        public DamagePerAPUsed(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, BuffTriggerType.OnTurnEnd, OnBuffTriggered);
            }

            return true;
        }

        void OnBuffTriggered(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var usedAP = buff.Target.UsedAP;
            if (usedAP <= 0)
                return;

            var damages = new Fights.Damage(Dice)
            {
                Source = buff.Caster,
                Buff = buff,
                School = GetEffectSchool(buff.Dice.EffectId),
                MarkTrigger = MarkTrigger,
                IsCritical = Critical,
                Spell = buff.Spell
            };

            damages.GenerateDamages();
            damages.Amount = usedAP * damages.BaseMaxDamages;

            buff.Target.InflictDamage(damages);
        }

        static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
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