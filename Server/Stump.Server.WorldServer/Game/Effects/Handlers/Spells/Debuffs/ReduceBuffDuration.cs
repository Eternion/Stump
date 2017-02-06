using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_ReduceEffectsDuration)]
    public class ReduceBuffDuration : SpellEffectHandler
    {
        public ReduceBuffDuration(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                if (Effect.Duration != 0 | Effect.Delay != 0)
                {
                    AddTriggerBuff(actor, TriggerBuff);
                }
                else
                    ReduceBuffsDuration(actor, integerEffect.Value);
            }

            return true;
        }

        void ReduceBuffsDuration(FightActor actor, short duration)
        {
            foreach (var buff in actor.GetBuffs().Where(buff => buff.Dispellable == FightDispellableEnum.DISPELLABLE).Where(buff => buff.Duration > 0 && buff.Delay == 0).ToArray())
            {
                buff.Duration -= duration;

                if (buff.Duration <= 0)
                    actor.RemoveBuff(buff);
            }

            actor.TriggerBuffs(actor, BuffTriggerType.OnDispelled);
            ContextHandler.SendGameActionFightModifyEffectsDurationMessage(Fight.Clients, Effect.Id, Caster, actor, (short)-duration);
        }

        void TriggerBuff(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return;

            ReduceBuffsDuration(buff.Target, integerEffect.Value);
        }
    }
}