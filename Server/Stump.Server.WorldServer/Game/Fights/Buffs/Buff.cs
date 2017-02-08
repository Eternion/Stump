using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public abstract class Buff
    {
       
        public const int CHARACTERISTIC_STATE = 71;

        protected Buff(int id, FightActor target, FightActor caster, SpellEffectHandler effectHandler, Spell spell, bool critical, FightDispellableEnum dispelable, FightActor triggerer = null)
        {
            Id = id;
            Target = target;
            Caster = caster;
            EffectHandler = effectHandler;
            Spell = spell;
            Critical = critical;

            Duration = triggerer != null && EffectFix?.TriggerBuffDuration != null ? (short)EffectFix.TriggerBuffDuration : (short) (EffectHandler.Duration == -1 ? -1000 : Effect.Duration);
            Dispellable = EffectFix?.Dispellable != null ? (FightDispellableEnum)EffectFix.Dispellable : dispelable;

            Delay = (short) EffectHandler.Delay;
            CustomActionId = (short?) EffectFix?.ActionId;

            Efficiency = 1.0d;

            if (triggerer == null && effectHandler.CastHandler?.Informations.Triggerer != null)
                triggerer = effectHandler.CastHandler.Informations.Triggerer;

            BuffTriggered = triggerer != null;
            DecrementReference = triggerer ?? Caster;
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

        public FightActor DecrementReference
        {
            get;
            set;
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

        public int Priority => EffectFix?.Priority ?? (EffectHandler.Priority);

        public bool Critical
        {
            get;
        }

        public FightDispellableEnum Dispellable
        {
            get;
            set;
        }

        public short? CustomActionId
        {
            get;
            set;
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

        public bool BuffTriggered
        {
            get;
            private set;
        }

        public SpellEffectFix EffectFix => Effect.EffectFix;

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
                        if (this is TriggerBuff buffTrigger && buffTrigger.ShouldTrigger(BuffTriggerType.Instant))
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