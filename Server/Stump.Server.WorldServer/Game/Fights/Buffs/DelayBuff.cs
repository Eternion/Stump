using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public delegate void DelayBuffApplyHandler(DelayBuff buff, object token);
    public delegate void DelayBuffRemoveHandler(DelayBuff buff);

    public class DelayBuff : Buff
    {
        public DelayBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, bool critical, bool dispelable, DelayBuffApplyHandler applyDelay)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Dice = effect;
            ApplyDelayedHandler = applyDelay;
        }

        public DelayBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, bool critical, bool dispelable, DelayBuffApplyHandler applyDelay, DelayBuffRemoveHandler removeDelay)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Dice = effect;
            ApplyDelayedHandler = applyDelay;
            RemoveDelayedHandler = removeDelay;
        }

        public DelayBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, bool critical, bool dispelable, DelayBuffApplyHandler applyDelay, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            Dice = effect;
            ApplyDelayedHandler = applyDelay;
        }

        public DelayBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, bool critical, bool dispelable, DelayBuffApplyHandler applyDelay, DelayBuffRemoveHandler removeDelay, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            Dice = effect;
            ApplyDelayedHandler = applyDelay;
            RemoveDelayedHandler = removeDelay;
        }

        public object Token
        {
            get;
            set;
        }

        public EffectDice Dice
        {
            get;
            private set;
        }

        public DelayBuffApplyHandler ApplyDelayedHandler
        {
            get;
            private set;
        }

        public DelayBuffRemoveHandler RemoveDelayedHandler
        {
            get;
            private set;
        }

        public override void Apply()
        {
            if (ApplyDelayedHandler != null)
                ApplyDelayedHandler(this, Token);
        }

        public override void Dispell()
        {
            if (ApplyDelayedHandler != null)
                ApplyDelayedHandler(this, Token);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration, (sbyte)( Dispellable ? 0 : 1 ), (short)Spell.Id, 0, (short)values[0], (short)values[1], (short)values[2], 0);
        }
    }
}
