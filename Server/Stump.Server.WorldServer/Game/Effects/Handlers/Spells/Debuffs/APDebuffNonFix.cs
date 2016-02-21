using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_RemoveAP)]
    [EffectHandler(EffectsEnum.Effect_LosingAP)]
    [EffectHandler(EffectsEnum.Effect_SubAP_1079)]
    public class APDebuffNonFix : SpellEffectHandler
    {
        public APDebuffNonFix(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (var actor in GetAffectedActors())
            {
                var target = actor;

                if (Effect.Duration != 0 || Effect.Delay != 0)
                {
                    AddTriggerBuff(actor, FightDispellableEnum.DISPELLABLE, APBuffTrigger);
                }
                else
                {
                    var value = RollAP(actor, integerEffect.Value);

                    var dodged = (short)(integerEffect.Value - value);

                    if (dodged > 0)
                    {
                        ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                            ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PA, Caster, target, dodged);
                    }

                    if (value <= 0)
                        continue;

                    target.LostAP(value, Caster);
                    target.TriggerBuffs(target, BuffTriggerType.OnAPLost);
                }
            }

            return true;
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

        void APBuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return;

            var value = RollAP(buff.Target, integerEffect.Value);

            var dodged = (short)(integerEffect.Value - value);

            if (dodged > 0)
            {
                ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                    ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PA, Caster, buff.Target, dodged);
            }

            if (value <= 0)
                return;

            buff.Target.LostAP(value, buff.Caster);

            buff.Target.TriggerBuffs(buff.Target, BuffTriggerType.OnAPLost);
        }
    }
}