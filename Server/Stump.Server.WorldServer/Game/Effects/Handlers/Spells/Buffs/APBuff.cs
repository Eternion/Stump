using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_RegainAP)]
    [EffectHandler(EffectsEnum.Effect_AddAP_111)]
    public class APBuff : SpellEffectHandler
    {
        public APBuff(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {

                if (IsTriggerBuff())
                {
                    AddTriggerBuff(actor, TriggerBuff);
                }
                else
                {
                    var integerEffect = GenerateEffect();

                    if (integerEffect == null)
                        return false;

                    if (Effect.Duration != 0 || Effect.Delay != 0)
                    {
                        AddStatBuff(actor, integerEffect.Value, PlayerFields.AP);
                    }
                    else
                    {
                        actor.RegainAP(integerEffect.Value);
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

            if (Effect.Duration != 0 || Effect.Delay != 0)
            {
                var newBuff = AddStatBuffDirectly(buff.Target, integerEffect.Value, PlayerFields.AP, triggerrer: triggerrer);
                if (TriggeredBuffDuration > 0)
                    newBuff.Duration = (short) TriggeredBuffDuration;
            }
            else
            {
                buff.Target.RegainAP(integerEffect.Value);
            }
        }
    }
}