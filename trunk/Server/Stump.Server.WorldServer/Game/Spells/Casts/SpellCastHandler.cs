using System.Collections.Generic;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    public abstract class SpellCastHandler
    {
        protected SpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
        {
            Caster = caster;
            Spell = spell;
            TargetedCell = targetedCell;
            Critical = critical;
        }

        private MapPoint m_castPoint;

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

        public SpellLevelTemplate SpellLevel
        {
            get { return Spell.CurrentSpellLevel; }
        }

        public Cell TargetedCell
        {
            get;
            private set;
        }

        public MapPoint TargetedPoint
        {
            get;
            private set;
        }

        public bool Critical
        {
            get;
            private set;
        }

        public virtual bool SilentCast
        {
            get { return false; }
        }

        public MarkTrigger MarkTrigger
        {
            get;
            set;
        }

        public Cell CastCell
        {
            get { return MarkTrigger != null && MarkTrigger.Shapes.Length > 0 ? MarkTrigger.Shapes[0].Cell : Caster.Cell; }
        }

        public MapPoint CastPoint
        {
            get { return m_castPoint ?? (m_castPoint = new MapPoint(CastCell)); }
            set { m_castPoint = value; }
        }

        public Fight Fight
        {
            get { return Caster.Fight; }
        }

        public Map Map
        {
            get { return Fight.Map; }
        }

        public abstract void Initialize();
        public abstract void Execute();

        public virtual IEnumerable<SpellEffectHandler> GetEffectHandlers()
        {
            return new SpellEffectHandler[0];
        }
    }
}