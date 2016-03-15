using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;using Stump.Server.WorldServer.Game.Spells.Casts;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States
{
    [EffectHandler(EffectsEnum.Effect_DispelState)]
    [EffectHandler(EffectsEnum.Effect_DisableState)]
    public class DispelState : SpellEffectHandler
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DispelState(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
            DefaultDispellableStatus = FightDispellableEnum.DISPELLABLE_BY_DEATH;
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (Effect.Duration != 0 || Effect.Delay != 0)
                {
                    AddTriggerBuff(actor, BuffTrigger);
                }
                else
                {
                    var state = SpellManager.Instance.GetSpellState((uint)Dice.Value);

                    if (state == null)
                    {
                        logger.Error("Spell state {0} not found", Dice.Value);
                        return false;
                    }

                    if (!actor.HasState(state.Id))
                        return false;

                    if (Effect.EffectId == EffectsEnum.Effect_DisableState)
                    {
                        var actualState = actor.GetBuffs(x => x is StateBuff && ((StateBuff)x).State.Id == state.Id).FirstOrDefault() as StateBuff;
                        if (actualState == null)
                            return false;

                        var id = actor.PopNextBuffId();
                        var stateBuff = new DisableStateBuff(id, actor, Caster, Dice, Spell, DefaultDispellableStatus, actualState)
                        {
                            Duration = 1
                        };

                        actor.AddBuff(stateBuff);
                    }
                    else
                        RemoveStateBuff(actor, (SpellStatesEnum)state.Id);
                }
            }

            return true;
        }

        void BuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var state = SpellManager.Instance.GetSpellState((uint)Dice.Value);

            if (state == null)
            {
                logger.Error("Spell state {0} not found", Dice.Value);
                return;
            }

            if (!buff.Target.HasState(state.Id))
                return;

            if (Effect.EffectId == EffectsEnum.Effect_DisableState)
            {
                var actualState = buff.Target.GetBuffs(x => x is StateBuff && ((StateBuff)x).State.Id == state.Id).FirstOrDefault() as StateBuff;
                if (actualState == null)
                    return;

                var id = buff.Target.PopNextBuffId();
                var stateBuff = new DisableStateBuff(id, buff.Target, Caster, Dice, Spell, FightDispellableEnum.DISPELLABLE_BY_DEATH, actualState)
                {
                    Duration = 1
                };

                buff.Target.AddBuff(stateBuff);
            }
            else
                RemoveStateBuff(buff.Target, (SpellStatesEnum)state.Id);
        }
    }
}
