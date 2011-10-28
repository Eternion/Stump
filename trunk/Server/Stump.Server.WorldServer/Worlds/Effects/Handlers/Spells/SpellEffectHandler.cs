using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Fights.Buffs;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells
{
    public abstract class SpellEffectHandler : EffectHandler
    {
        private Zone m_effectZone;

        protected SpellEffectHandler(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect)
        {
            Dice = effect;
            Caster = caster;
            Spell = spell;
            TargetedCell = targetedCell;
            Critical = critical;
        }

        public EffectDice Dice
        {
            get;
            private set;
        }

        public FightActor Caster
        {
            get;
            private set;
        }

        public Spell Spell
        {
            get;
            private set;
        }

        public Cell TargetedCell
        {
            get;
            private set;
        }

        public bool Critical
        {
            get;
            private set;
        }

        public Zone EffectZone
        {
            get { return m_effectZone ?? (m_effectZone = new Zone((SpellShapeEnum) Effect.ZoneShape, Effect.ZoneSize)); }
            set
            {
                m_effectZone = value;

                RefreshZone();
            }
        }

        private Cell[] m_affectedCells;

        public Cell[] AffectedCells
        {
            get { return m_affectedCells ?? (m_affectedCells = EffectZone.GetCells(TargetedCell, Map)); }
            private set { m_affectedCells = value; }
        }

        public Fight Fight
        {
            get { return Caster.Fight; }
        }

        public Map Map
        {
            get { return Fight.Map; }
        }

        public bool IsValidTarget(FightActor actor)
        {
            if (Effect.Targets == SpellTargetType.NONE || Effect.Targets == SpellTargetType.ALL)
                return true;

            if (Caster == actor &&
                !Effect.Targets.HasFlag(SpellTargetType.SELF) &&
                !Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF))
                return false;

            // todo : summons

            if (Caster.IsFriendlyWith(actor) && 
                Effect.Targets.HasFlag(SpellTargetType.ALLY_1) ||
                Effect.Targets.HasFlag(SpellTargetType.ALLY_2) ||
                Effect.Targets.HasFlag(SpellTargetType.ALLY_3) ||
                Effect.Targets.HasFlag(SpellTargetType.ALLY_4) ||
                Effect.Targets.HasFlag(SpellTargetType.ALLY_5) ||
                Effect.Targets.HasFlag(SpellTargetType.ALLY_SUMMONS) ||
                Effect.Targets.HasFlag(SpellTargetType.ALLY_STATIC_SUMMONS))
                return true;

            if (Caster.IsEnnemyWith(actor) &&
                Effect.Targets.HasFlag(SpellTargetType.ENNEMY_1) ||
                Effect.Targets.HasFlag(SpellTargetType.ENNEMY_2) ||
                Effect.Targets.HasFlag(SpellTargetType.ENNEMY_3) ||
                Effect.Targets.HasFlag(SpellTargetType.ENNEMY_4) ||
                Effect.Targets.HasFlag(SpellTargetType.ENNEMY_5) ||
                Effect.Targets.HasFlag(SpellTargetType.ENNEMY_SUMMONS) ||
                Effect.Targets.HasFlag(SpellTargetType.ENNEMY_STATIC_SUMMONS))
                return true;

            return false;
        }

        public void RefreshZone()
        {
            AffectedCells = EffectZone.GetCells(TargetedCell, Map);
        }

        public IEnumerable<FightActor> GetAffectedActors()
        {
            if (Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF))
                return new [] { Caster };

            return Fight.GetAllFighters(AffectedCells).Where(IsValidTarget);
        }

        public IEnumerable<FightActor> GetAffectedActors(Predicate<FightActor> predicate)
        {
            if (Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF) && predicate(Caster))
                return new[] { Caster };
            
            if (Effect.Targets.HasFlag(SpellTargetType.ONLY_SELF))
                return new FightActor[0];


            return GetAffectedActors().Where(entry => predicate(entry));
        }

        public StatBuff AddStatBuff(FightActor target, short value, CaracteristicsEnum caracteritic, bool dispelable)
        {
            int id = target.PopNextBuffId();
            var buff = new StatBuff(id, target, Caster, Effect, Spell, value, caracteritic, Critical, dispelable);

            target.AddAndApplyBuff(buff);

            return buff;
        }

        public StatBuff AddStatBuff(FightActor target, short value, CaracteristicsEnum caracteritic, bool dispelable, short customActionId)
        {
            int id = target.PopNextBuffId();
            var buff = new StatBuff(id, target, Caster, Effect, Spell, value, caracteritic, Critical, dispelable, customActionId);

            target.AddAndApplyBuff(buff);

            return buff;
        }

        public TriggerBuff AddTriggerBuff(FightActor target, bool dispelable, TriggerType trigger, TriggerBuffApplyHandler applyTrigger)
        {
            int id = target.PopNextBuffId();
            var buff = new TriggerBuff(id, target, Caster, Dice, Spell, Critical, dispelable, trigger, applyTrigger);

            target.AddAndApplyBuff(buff);

            return buff;
        }
        public TriggerBuff AddTriggerBuff(FightActor target, bool dispelable, TriggerType trigger, TriggerBuffApplyHandler applyTrigger, TriggerBuffRemoveHandler removeTrigger)
        {
            int id = target.PopNextBuffId();
            var buff = new TriggerBuff(id, target, Caster, Dice, Spell, Critical, dispelable, trigger, applyTrigger, removeTrigger);

            target.AddAndApplyBuff(buff);

            return buff;
        }
    }
}