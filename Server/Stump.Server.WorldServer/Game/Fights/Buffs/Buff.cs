using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public abstract class Buff
    {
       
        public const int CHARACTERISTIC_STATE = 71;

        protected Buff(int id, FightActor target, FightActor caster, SpellEffectHandler effectHandler, Spell spell, bool critical, FightDispellableEnum dispelable, int priority = 0, short? customActionId = null)
        {
            Id = id;
            Target = target;
            Caster = caster;
            EffectHandler = effectHandler;
            Spell = spell;
            Critical = critical;
            Dispellable = dispelable;
            CustomActionId = customActionId;

            Duration = (short) (EffectHandler.Duration == -1 ? -1000 : Effect.Duration);
            Delay = (short) EffectHandler.Delay;

            Efficiency = 1.0d;
            Priority = priority;
        }

        public SpellEffectHandler EffectHandler
        {
            get;
        }

        public int Id
        {
            get;
        }

        public FightActor Target
        {
            get;
        }

        public FightActor Caster
        {
            get;
        }

        public EffectDice Dice => EffectHandler.Dice;

        public EffectBase Effect => EffectHandler.Effect;

        public Spell Spell
        {
            get;
        }

        public short Duration
        {
            get;
            set;
        }

        public short Delay
        {
            get;
            set;
        }

        public int Priority
        {
            get;
            set;
        }

        public bool Critical
        {
            get;
        }

        public FightDispellableEnum Dispellable
        {
            get;
        }

        public short? CustomActionId
        {
            get;
        }

        /// <summary>
        /// Efficiency factor, increase or decrease effect's efficiency. Default is 1.0
        /// </summary>
        public double Efficiency
        {
            get;
            set;
        }

        public bool Applied
        {
            get;
            private set;
        }

        public virtual BuffType Type
        {
            get
            {
                if (Effect.Template.Characteristic == CHARACTERISTIC_STATE)
                    return BuffType.CATEGORY_STATE;

                if (Effect.Template.Operator == "-")
                    return Effect.Template.Active ? BuffType.CATEGORY_ACTIVE_MALUS : BuffType.CATEGORY_PASSIVE_MALUS;

                if (Effect.Template.Operator == "+")
                    return  Effect.Template.Active ? BuffType.CATEGORY_ACTIVE_BONUS : BuffType.CATEGORY_PASSIVE_BONUS;

                return BuffType.CATEGORY_OTHER;
            }
        }

        /// <summary>
        /// Decrement Duration and return true whenever the buff is over
        /// </summary>
        /// <returns></returns>
        public bool DecrementDuration()
        {
            if (Delay > 0)
            {
                if (--Delay == 0)
                {
                    var fight = Caster.Fight;

                    using (fight.StartSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED))
                    {
                        var buffTrigger = this as TriggerBuff;
                        if (buffTrigger != null && buffTrigger.ShouldTrigger(BuffTriggerType.Instant))
                            buffTrigger.Apply(Target, BuffTriggerType.Instant);
                        else
                            Apply();

                        fight.UpdateBuff(this, false);
                    }
                }

                return false;
            }

            if (Duration <= -1) // Duration = -1000 => unlimited buff
                return false;

            return --Duration <= 0;
        }

        public virtual void Apply() => Applied = true;
        public virtual void Dispell() => Applied = false;

        public virtual short GetActionId()
        {
            if (CustomActionId.HasValue)
                return CustomActionId.Value;

            return (short) Effect.EffectId;
        }

        public EffectInteger GenerateEffect()
        {
            var effect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (effect != null)
                effect.Value = (short)( effect.Value * Efficiency );

            return effect;
        }

        public FightDispellableEffectExtendedInformations GetFightDispellableEffectExtendedInformations()
            => new FightDispellableEffectExtendedInformations(GetActionId(), Caster.Id, GetAbstractFightDispellableEffect());

        public abstract AbstractFightDispellableEffect GetAbstractFightDispellableEffect();
    }
}