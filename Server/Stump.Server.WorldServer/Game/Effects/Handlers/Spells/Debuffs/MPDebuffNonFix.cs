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
    [EffectHandler(EffectsEnum.Effect_LosingMP)]
    [EffectHandler(EffectsEnum.Effect_LostMP)]
    [EffectHandler(EffectsEnum.Effect_SubMP_1080)]
    public class MPDebuffNonFix : SpellEffectHandler
    {
        public MPDebuffNonFix(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
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
                if (Effect.Duration != 0 | Effect.Delay != 0)
                {
                    AddTriggerBuff(actor, FightDispellableEnum.DISPELLABLE, MPBuffTrigger);
                }
                else
                {
                    var value = RollMP(actor, integerEffect.Value);

                    var dodged = (short)(integerEffect.Value - value);

                    if (dodged > 0)
                    {
                        ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                            ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PM, Caster, actor, dodged);
                    }

                    if (value <= 0)
                        return false;

                    actor.LostMP(value, Caster);
                    actor.TriggerBuffs(actor, BuffTriggerType.OnMPLost);
                }
            }

            return true;
        }

        short RollMP(FightActor actor, int maxValue)
        {
            short value = 0;

            for (var i = 0; i < maxValue && value < actor.MP; i++)
            {
                if (actor.RollMPLose(Caster, value))
                {
                    value++;
                }
            }

            return value;
        }

        void MPBuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return;

            var value = RollMP(buff.Target, integerEffect.Value);

            var dodged = (short)(integerEffect.Value - value);

            if (dodged > 0)
            {
                ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                    ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PM, buff.Caster, buff.Target, dodged);
            }

            if (value <= 0)
                return;

            buff.Target.LostMP(value, buff.Caster);

            buff.Target.TriggerBuffs(buff.Target, BuffTriggerType.OnMPLost);
        }
    }
}