using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Steals
{
    [EffectHandler(EffectsEnum.Effect_StealHPWater)]
    [EffectHandler(EffectsEnum.Effect_StealHPEarth)]
    [EffectHandler(EffectsEnum.Effect_StealHPAir)]
    [EffectHandler(EffectsEnum.Effect_StealHPFire)]
    [EffectHandler(EffectsEnum.Effect_StealHPNeutral)]
    public class HPSteal : SpellEffectHandler
    {
        public HPSteal(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (IsBuff())
                    AddTriggerBuff(actor, StealHpBuffTrigger);
                else
                    StealHp(actor);
            }

            return true;
        }

        private void NotifySpellReflected(FightActor source)
        {
            ActionsHandler.SendGameActionFightReflectSpellMessage(Fight.Clients, Caster, source);
        }

        void StealHp(FightActor target)
        {
            var damage = new Fights.Damage(Dice, GetEffectSchool(Effect.EffectId), Caster, Spell, TargetedCell, EffectZone) { IsCritical = Critical };

            // spell reflected
            var buff = target.GetBestReflectionBuff();
            if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel && Spell.Template.Id != 0)
            {
                NotifySpellReflected(target);
                damage.Source = Caster;
                damage.ReflectedDamages = true;
                Caster.InflictDamage(damage);

                if (buff.Duration <= 0)
                    target.RemoveBuff(buff);
            }
            else
            {
                target.InflictDamage(damage);

                var amount = (short)Math.Floor(damage.Amount / 2.0);
                if (amount > 0)
                    Caster.Heal(amount, target, true);
            }
        }

        void StealHpBuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            StealHp(buff.Target);
        }

        static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_StealHPWater:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_StealHPEarth:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_StealHPAir:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_StealHPFire:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_StealHPNeutral:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}