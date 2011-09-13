using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells
{
    public abstract class SpellEffectHandler : EffectHandler
    {
        private Zone m_effectZone;

        protected SpellEffectHandler(EffectBase effect, FightActor caster, Spell spell, Cell targetedCell)
            : base(effect)
        {
            Caster = caster;
            Spell = spell;
            TargetedCell = targetedCell;
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

        public Zone EffectZone
        {
            get { return m_effectZone ?? (m_effectZone = new Zone((SpellShapeEnum) Effect.ZoneShape, Effect.ZoneSize)); }
            private set
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
    }
}