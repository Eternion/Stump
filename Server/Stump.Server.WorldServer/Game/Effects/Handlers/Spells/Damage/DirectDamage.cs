using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_DamageWater)]
    [EffectHandler(EffectsEnum.Effect_DamageEarth)]
    [EffectHandler(EffectsEnum.Effect_DamageAir)]
    [EffectHandler(EffectsEnum.Effect_DamageFire)]
    [EffectHandler(EffectsEnum.Effect_DamageNeutral)]
    public class DirectDamage : SpellEffectHandler
    {
        public DirectDamage(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
            BuffTriggerType = BuffTriggerType.Unknown;
        }

        public BuffTriggerType BuffTriggerType
        {
            get;
            set;
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (Effect.Duration != 0 || Effect.Delay != 0)
                {
                    if (BuffTriggerType == BuffTriggerType.Unknown)
                        AddTriggerBuff(actor, DamageBuffTrigger);
                    else
                        AddTriggerBuff(actor, BuffTriggerType, DamageBuffTrigger);
                }
                else
                {
                    // spell reflected
                    var buff = actor.GetBestReflectionBuff();
                    if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel
                        && Spell.Template.Id != 0 && !Caster.IsIndirectSpellCast(Spell) && !Caster.IsPoisonSpellCast(Spell))
                    {
                        NotifySpellReflected(actor);
                        var damage = new Fights.Damage(Dice, GetEffectSchool(Dice.EffectId), Caster, Spell, Caster.Cell)
                        {
                            ReflectedDamages = true,
                            MarkTrigger = MarkTrigger,
                            IsCritical = Critical
                        };
                        damage.GenerateDamages();
                        damage.Amount = (short)(damage.Amount * Efficiency);

                        Caster.InflictDamage(damage);

                        if (buff.Duration <= 0)
                            actor.RemoveBuff(buff);
                    }
                    else
                    {
                        var damage = new Fights.Damage(Dice, GetEffectSchool(Dice.EffectId), Caster, Spell, TargetedCell, EffectZone)
                        {
                            MarkTrigger = MarkTrigger,
                            IsCritical = Critical
                        };
                        damage.GenerateDamages();
                        damage.Amount = (short)(damage.Amount * Efficiency);

                        actor.InflictDamage(damage);
                    }
                }
            }

            return true;
        }

        static void DamageBuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damages = token as Fights.Damage;

            if (damages != null && (damages.Spell == null || damages.ReflectedDamages))
                return;

            var damage = new Fights.Damage(buff.Dice, GetEffectSchool(buff.Dice.EffectId), buff.Caster, null, buff.Target.Cell)
            {
                IsCritical = buff.Critical,
                ReflectedDamages = true
            };

            damage.GenerateDamages();
            damage.Amount = (short)(damage.Amount * buff.Efficiency);

            buff.Target.InflictDamage(damage);
        }

        static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
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

        void NotifySpellReflected(FightActor source)
        {
            ActionsHandler.SendGameActionFightReflectSpellMessage(Fight.Clients, Caster, source);
        }
    }
}