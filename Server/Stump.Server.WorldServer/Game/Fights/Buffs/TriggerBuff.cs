using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public delegate void TriggerBuffApplyHandler(TriggerBuff buff, BuffTriggerType trigger, object token);
    public delegate void TriggerBuffRemoveHandler(TriggerBuff buff);

    public class TriggerBuff : Buff
    {
        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, Spell parentSpell, bool critical, bool dispelable, TriggerBuffApplyHandler applyTrigger)
            : this(id, target, caster, effect, spell, parentSpell, critical, dispelable, applyTrigger, (short?)null)
        {
        }

        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, Spell parentSpell, bool critical, bool dispelable, TriggerBuffApplyHandler applyTrigger, TriggerBuffRemoveHandler removeTrigger)
            : this(id, target, caster, effect, spell, parentSpell, critical, dispelable, applyTrigger, removeTrigger, null)
        {
        }

        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell,  Spell parentSpell, bool critical, bool dispelable, TriggerBuffApplyHandler applyTrigger, short? customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            Dice = effect;
            ParentSpell = parentSpell;
            ApplyTrigger = applyTrigger;
        }

        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, Spell parentSpell, bool critical, bool dispelable, TriggerBuffApplyHandler applyTrigger, TriggerBuffRemoveHandler removeTrigger, short? customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            Dice = effect;
            ParentSpell = parentSpell;
            ApplyTrigger = applyTrigger;
            RemoveTrigger = removeTrigger;
        }

        public object Token
        {
            get;
            set;
        }

        public Spell ParentSpell
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

        public override void Apply(BuffTriggerType type, object token)
        {
            if (ApplyTrigger != null)
                ApplyTrigger(this, type, Token);
        }

        public void Apply(BuffTriggerType trigger)
        {
            if (ApplyTrigger != null)
                ApplyTrigger(this, trigger, Token);
        }
        

        public override void Dispell()
        {
            if (RemoveTrigger != null)
                RemoveTrigger(this);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration, (sbyte)( Dispellable ? 0 : 1 ), (short)ParentSpell.Id, 0, 0, (short)values[0], (short)values[1], (short)values[2], 0);
        }
    }
}