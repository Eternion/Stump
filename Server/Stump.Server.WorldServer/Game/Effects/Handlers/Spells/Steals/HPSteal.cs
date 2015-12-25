using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Steals
{
    [EffectHandler(EffectsEnum.Effect_StealHPWater)]
    [EffectHandler(EffectsEnum.Effect_StealHPEarth)]
    [EffectHandler(EffectsEnum.Effect_StealHPAir)]
    [EffectHandler(EffectsEnum.Effect_StealHPFire)]
    [EffectHandler(EffectsEnum.Effect_StealHPNeutral)]
    public class HPSteal : SpellEffectHandler
    {
        public HPSteal(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (Effect.Duration != 0 && Spell.Id != (int)SpellIdEnum.MARTEAU_D_OKIM)
                {
                    AddTriggerBuff(actor, true, BuffTriggerType.OnTurnBegin, StealHpBuffTrigger);
                }
                else
                {
                    var damage = new Fights.Damage(Dice, GetEffectSchool(Effect.EffectId), Caster, Spell, TargetedCell, EffectZone) {IsCritical = Critical};

                    // spell reflected
                    var buff = actor.GetBestReflectionBuff();
                    if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel && Spell.Template.Id != 0)
                    {
                        NotifySpellReflected(actor);
                        damage.Source = Caster;
                        damage.ReflectedDamages = true;
                        Caster.InflictDamage(damage);

                        if (buff.Duration <= 0)
                            actor.RemoveBuff(buff);
                    }
                    else
                    {
                        actor.InflictDamage(damage);

                        var amount = (short)Math.Round(damage.Amount/2.0);
                        if (amount > 0)
                            Caster.HealDirect(amount, actor);
                    }
                }
            }

            return true;
        }

        private void NotifySpellReflected(FightActor source)
        {
            ActionsHandler.SendGameActionFightReflectSpellMessage(Fight.Clients, Caster, source);
        }

        private static void StealHpBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var integerEffect = buff.GenerateEffect();

            if (integerEffect == null)
                return;

            buff.Target.Heal(integerEffect.Value, buff.Caster);
        }

        private static EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
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