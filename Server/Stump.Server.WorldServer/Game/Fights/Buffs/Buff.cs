using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public abstract class Buff
    {
        public static readonly Dictionary<string, BuffTriggerType> m_triggersMapping = new Dictionary<string, BuffTriggerType>()
        {
            {"I", BuffTriggerType.Instant},
            {"D", BuffTriggerType.OnDamaged},
            {"DA", BuffTriggerType.OnDamagedAir},
            {"DE", BuffTriggerType.OnDamagedEarth},
            {"DF", BuffTriggerType.OnDamagedFire},
            {"DW", BuffTriggerType.OnDamagedWater},
            {"DN", BuffTriggerType.OnDamagedNeutral},
            {"DBA", BuffTriggerType.OnDamagedByAlly},
            {"DBE", BuffTriggerType.OnDamagedByEnemy},
            {"DC", BuffTriggerType.OnDamagedByWeapon},
            {"DS", BuffTriggerType.OnDamagedBySpell},
            {"DG", BuffTriggerType.OnDamagedByGlyph},
            {"DP", BuffTriggerType.OnDamagedByTrap},
            {"DM", BuffTriggerType.OnDamagedInCloseRange},
            {"DR", BuffTriggerType.OnDamagedInLongRange},
            {"MD", BuffTriggerType.OnDamagedByPush},
            {"DI", BuffTriggerType.OnDamagedUnknown_1},
            {"Dr", BuffTriggerType.OnDamagedUnknown_2},
            {"DTB", BuffTriggerType.OnDamagedUnknown_3},
            {"DTE", BuffTriggerType.OnDamagedUnknown_4},
            {"MDM", BuffTriggerType.OnDamagedUnknown_5},
            {"MDP", BuffTriggerType.OnDamagedUnknown_6},
            {"TB", BuffTriggerType.OnTurnBegin},
            {"TE", BuffTriggerType.OnTurnEnd},
            {"m", BuffTriggerType.OnMPLost},
            {"A", BuffTriggerType.OnAPLost},
            {"H", BuffTriggerType.OnHealed},
            {"EO", BuffTriggerType.OnStateAdded},
            {"Eo", BuffTriggerType.OnStateRemoved},
            {"CC", BuffTriggerType.OnCriticalHit},
            {"d", BuffTriggerType.Unknown_1},
            {"M", BuffTriggerType.OnPush},
            {"mA", BuffTriggerType.Unknown_3},
            {"ML", BuffTriggerType.Unknown_4},
            {"MP", BuffTriggerType.Unknown_5},
            {"MS", BuffTriggerType.Unknown_6},
            {"P", BuffTriggerType.Unknown_7},
            {"R", BuffTriggerType.Unknown_8},
            {"tF", BuffTriggerType.Unknown_9},
            {"tS", BuffTriggerType.OnTackle},
            {"X", BuffTriggerType.Unknown_11},
        };

        public const int CHARACTERISTIC_STATE = 71;

        protected Buff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable)
            : this(id, target,caster,effect,spell,critical, dispelable, 0)
        {
        }

        protected Buff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, short customActionId)
        {
            Id = id;
            Target = target;
            Caster = caster;
            Effect = effect;
            Spell = spell;
            Critical = critical;
            Dispellable = dispelable;
            CustomActionId = customActionId;

            Duration = (short)Effect.Duration;
            Efficiency = 1.0d;

            var triggerStr = Effect.Triggers.Split('|');
            Triggers = triggerStr.Select(GetTriggerFromString).ToList();
        }

        public int Id
        {
            get;
            private set;
        }

        public FightActor Target
        {
            get;
            private set;
        }

        public FightActor Caster
        {
            get;
            private set;
        }


        public EffectBase Effect
        {
            get;
            private set;
        }

        public Spell Spell
        {
            get;
            private set;
        }

        public short Duration
        {
            get;
            set;
        }

        public bool Critical
        {
            get;
            private set;
        }

        public bool Dispellable
        {
            get;
            private set;
        }

        public short? CustomActionId
        {
            get;
            private set;
        }

        /// <summary>
        /// Efficiency factor, increase or decrease effect's efficiency. Default is 1.0
        /// </summary>
        public double Efficiency
        {
            get;
            set;
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

        public List<BuffTrigger> Triggers
        {
            get;
            protected set;
        }

        public bool ShouldTrigger(BuffTriggerType type, object token = null)
        {
            return Triggers.Any(x => x.Type == type && (x.Parameter == null || x.Parameter == token));
        }

        /// <summary>
        /// Decrement Duration and return true whenever the buff is over
        /// </summary>
        /// <returns></returns>
        public bool DecrementDuration()
        {
            return --Duration == 0;
        }

        public virtual void Apply(BuffTriggerType type) => Apply(type, null);
        public abstract void Apply(BuffTriggerType type, object token);
        public abstract void Dispell();

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
        {
            return new FightDispellableEffectExtendedInformations(GetActionId(), Caster.Id, GetAbstractFightDispellableEffect());
        }

        public abstract AbstractFightDispellableEffect GetAbstractFightDispellableEffect();

        public static BuffTrigger GetTriggerFromString(string str)
        {
            if (m_triggersMapping.ContainsKey(str))
                return new BuffTrigger(m_triggersMapping[str]);

            if (str.StartsWith("EO"))
                return new BuffTrigger(BuffTriggerType.OnSpecificStateAdded, int.Parse(str.Remove(0, "EO".Length)));

            if (str.StartsWith("Eo"))
                return new BuffTrigger(BuffTriggerType.OnSpecificStateRemoved, int.Parse(str.Remove(0, "Eo".Length)));

            return new BuffTrigger(BuffTriggerType.Unknown);
        }
    }
}