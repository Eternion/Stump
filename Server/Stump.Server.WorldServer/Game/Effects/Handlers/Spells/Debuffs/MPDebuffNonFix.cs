using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_LosingMP)]
    [EffectHandler(EffectsEnum.Effect_LostMP)]
    [EffectHandler(EffectsEnum.Effect_SubMP_1080)]
    public class MPDebuffNonFix : SpellEffectHandler
    {
        public MPDebuffNonFix(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (var actor in GetAffectedActors())
            {
                var value = RollMP(actor, integerEffect.Value);

                if (value <= 0)
                    return false;

                if (Effect.Duration != 0 |Effect.Delay != 0)
                {
                    AddTriggerBuff(actor, true, MPBuffTrigger);
                }
                else
                {
                    var dodged = (short)(integerEffect.Value - value);

                    if (dodged > 0)
                    {
                        ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                            ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PM, Caster, actor, dodged);
                    }

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

            if (value <= 0)
                return;

            var dodged = (short)(integerEffect.Value - value);

            if (dodged > 0)
            {
                ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                    ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PM, Caster, buff.Target, dodged);
            }

            AddStatBuff(buff.Target, (short)(-value), PlayerFields.MP, true, (short)EffectsEnum.Effect_SubMP);
            buff.Target.TriggerBuffs(buff.Target, BuffTriggerType.OnMPLost);
        }
    }
}