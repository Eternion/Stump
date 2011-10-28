using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Fights.Buffs
{
    public delegate void TriggerBuffApplyHandler(TriggerBuff buff, TriggerType trigger);
    public delegate void TriggerBuffRemoveHandler(TriggerBuff buff);

    public class TriggerBuff : Buff
    {
        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, bool critical, bool dispelable, TriggerType trigger, TriggerBuffApplyHandler applyTrigger)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Trigger = trigger;
            Dice = effect;
            ApplyTrigger = applyTrigger;
        }

        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, bool critical, bool dispelable, TriggerType trigger, TriggerBuffApplyHandler applyTrigger, TriggerBuffRemoveHandler removeTrigger)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Trigger = trigger;
            Dice = effect;
            ApplyTrigger = applyTrigger;
            RemoveTrigger = removeTrigger;
        }

        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, bool critical, bool dispelable, TriggerType trigger, TriggerBuffApplyHandler applyTrigger, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            Trigger = trigger;
            Dice = effect;
            ApplyTrigger = applyTrigger;
        }

        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, bool critical, bool dispelable, TriggerType trigger, TriggerBuffApplyHandler applyTrigger, TriggerBuffRemoveHandler removeTrigger, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            Trigger = trigger;
            Dice = effect;
            ApplyTrigger = applyTrigger;
            RemoveTrigger = removeTrigger;
        }

        public TriggerType Trigger
        {
            get;
            private set;
        }

        public EffectDice Dice
        {
            get;
            private set;
        }

        public TriggerBuffApplyHandler ApplyTrigger
        {
            get;
            private set;
        }

        public TriggerBuffRemoveHandler RemoveTrigger
        {
            get;
            private set;
        }

        public override void Apply()
        {
            if (ApplyTrigger != null)
                ApplyTrigger(this, TriggerType.UNKNOWN);
        }

        public void Apply(TriggerType trigger)
        {
            if (ApplyTrigger != null)
                ApplyTrigger(this, trigger);
        }

        public override void Remove()
        {
            if (RemoveTrigger != null)
                RemoveTrigger(this);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration, (sbyte)( Dispelable ? 0 : 1 ), (short)Spell.Id, 0, (short)values[0], (short)values[1], (short)values[2], 0);
        }
    }
}