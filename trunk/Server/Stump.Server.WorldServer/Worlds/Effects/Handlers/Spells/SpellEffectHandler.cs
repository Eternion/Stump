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

        public void RefreshZone()
        {
            AffectedCells = EffectZone.GetCells(TargetedCell, Map);
        }

        public IEnumerable<FightActor> GetAffectedActors()
        {
            return Fight.GetAllFighters(AffectedCells);
        }

        public IEnumerable<FightActor> GetAffectedActors(Predicate<FightActor> predicate)
        {
            return Fight.GetAllFighters(AffectedCells).Where(entry => predicate(entry));
        }

        public StatBuff AddStatBuff(FightActor target, short value, CaracteristicsEnum caracteritic, bool dispelable)
        {
            int id = target.PopNextBuffId();
            var buff = new StatBuff(id, target, Caster, Effect, Spell, value, caracteritic, Critical, dispelable);

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