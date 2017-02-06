using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells.Casts;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubAP)]
    [EffectHandler(EffectsEnum.Effect_LostAP)]
    [EffectHandler(EffectsEnum.Effect_SubAP_Roll)]
    public class APDebuff : SpellEffectHandler
    {
        public APDebuff(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {

            foreach (var actor in GetAffectedActors())
            {
                var target = actor;
                
                var buff = actor.GetBestReflectionBuff();
                if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel && buff.RollReflection() && Spell.Template.Id != 0)
                {
                    NotifySpellReflected(actor);
                    target = Caster;

                    if (buff.Duration <= 0)
                        actor.RemoveBuff(buff);
                }

                if (IsTriggerBuff())
                {
                    AddTriggerBuff(target, TriggerBuff);
                }
                else
                {
                    var integerEffect = GenerateEffect();

                    if (integerEffect == null)
                        return false;

                    var value = Effect.EffectId == EffectsEnum.Effect_SubAP ? integerEffect.Value : RollAP(actor, integerEffect.Value);

                    var dodged = (short) (integerEffect.Value - value);

                    if (dodged > 0)
                    {
                        ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                            ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PA, Caster, target, dodged);
                    }

                    if (value <= 0)
                        continue;

                    target.TriggerBuffs(target, BuffTriggerType.OnAPLost);

                    if (Effect.Duration != 0 || Effect.Delay != 0 && Effect.EffectId != EffectsEnum.Effect_LostAP)
                    {
                        AddStatBuff(actor, (short) -value, PlayerFields.AP, (short) EffectsEnum.Effect_SubAP);
                    }
                    else
                    {
                        target.LostAP(value, Caster);
                    }
                }
            }

            return true;
        }

        private void TriggerBuff(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return;

            var value = Effect.EffectId == EffectsEnum.Effect_SubAP ? integerEffect.Value : RollAP(buff.Target, integerEffect.Value);

            var dodged = (short)(integerEffect.Value - value);

            if (dodged > 0)
            {
                ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                    ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PA, Caster, buff.Target, dodged);
            }

            if (value <= 0)
                return;

            buff.Target.TriggerBuffs(buff.Target, BuffTriggerType.OnAPLost);

            if (Effect.Duration != 0 || Effect.Delay != 0 && Effect.EffectId != EffectsEnum.Effect_LostAP)
            {
                var newBuff = AddStatBuffDirectly(buff.Target, (short)-value, PlayerFields.AP, (short)EffectsEnum.Effect_SubAP, triggerrer: triggerrer);
                if (TriggeredBuffDuration > 0)
                    newBuff.Duration = (short) TriggeredBuffDuration;
            }
            else
            {
                buff.Target.LostAP(value, Caster);
            }
        }

        short RollAP(FightActor actor, int maxValue)
        {
            short value = 0;

            for (var i = 0; i < maxValue && value < actor.AP; i++)
            {
                if (actor.RollAPLose(Caster, value))
                {
                    value++;
                }
            }

            return value;
        }


        void NotifySpellReflected(FightActor source)
        {
            ActionsHandler.SendGameActionFightReflectSpellMessage(Fight.Clients, Caster, source);
        }
    }
}