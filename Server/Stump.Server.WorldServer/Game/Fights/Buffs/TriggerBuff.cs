using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public delegate void TriggerBuffApplyHandler(TriggerBuff buff, FightActor trigerrer, BuffTriggerType trigger, object token);
    public delegate void TriggerBuffRemoveHandler(TriggerBuff buff);

    public class TriggerBuff : Buff
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
            {"X", BuffTriggerType.OnDeath},
        };


        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell,  Spell parentSpell, bool critical, FightDispellableEnum dispelable, int priority, TriggerBuffApplyHandler applyTrigger, short? customActionId = null)
            : this(id, target, caster, effect, spell, parentSpell, critical, dispelable, priority, applyTrigger, null, customActionId)
        {
        }

        public TriggerBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, Spell parentSpell, bool critical, FightDispellableEnum dispelable, int priority, TriggerBuffApplyHandler applyTrigger, TriggerBuffRemoveHandler removeTrigger, short? customActionId = null)
            : base(id, target, caster, effect, spell, critical, dispelable, priority, customActionId)
        {
            Dice = effect;
            ParentSpell = parentSpell;
            ApplyTrigger = applyTrigger;
            RemoveTrigger = removeTrigger;

            var triggerStr = Effect.Triggers.Split('|');
            Triggers = triggerStr.Select(GetTriggerFromString).ToList();
        }

        public object Token
        {
            get;
            set;
        }

        public Spell ParentSpell
        {
            get;
        }

        public EffectDice Dice
        {
            get;
        }

        public TriggerBuffApplyHandler ApplyTrigger
        {
            get;
        }

        public TriggerBuffRemoveHandler RemoveTrigger
        {
            get;
        }


        public List<BuffTrigger> Triggers
        {
            get;
            protected set;
        }

        public void SetTrigger(BuffTriggerType trigger)
        {
            Triggers = new List<BuffTrigger> { new BuffTrigger(trigger) };
        }

        public bool ShouldTrigger(BuffTriggerType type, object token = null)
            => Delay == 0 && Triggers.Any(x => x.Type == type && (x.Parameter == null || x.Parameter.Equals(token)));

        public override void Apply()
        {
            base.Apply();
            if (ShouldTrigger(BuffTriggerType.Instant))
                Apply(Caster, BuffTriggerType.Instant);
        }

        public void Apply(FightActor fighterTrigger, BuffTriggerType trigger, object token)
        {
            base.Apply();
            if (ApplyTrigger != null)
                ApplyTrigger(this, fighterTrigger, trigger, token);
        }


        public void Apply(FightActor fighterTrigger, BuffTriggerType trigger)
        {
            base.Apply();
            if (ApplyTrigger != null)
                ApplyTrigger(this, fighterTrigger, trigger, Token);
        }
        

        public override void Dispell()
        {
            base.Dispell();
            if (RemoveTrigger != null)
                RemoveTrigger(this);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Delay,
                (sbyte)Dispellable,
                (short)Spell.Id, Effect.Id, 0,
                (values.Length > 0 ? Convert.ToInt32(values[0]) : 0),
                (values.Length > 1 ? Convert.ToInt32(values[1]) : 0),
                (values.Length > 2 ? Convert.ToInt32(values[2]) : 0),
                Delay);
        }

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