using System.Collections.Generic;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

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
        private Cell m_customCastCell;

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

        public CastSpell CastedByEffect
        {
            get;
            set;
        }

        public SpellLevelTemplate SpellLevel
        {
            get { return Spell.CurrentSpellLevel; }
        }

        public Cell TargetedCell
        {
            get;
            protected set;
        }

        public FightActor TargetedActor
        {
            get;
            protected set;
        }

        public MapPoint TargetedPoint
        {
            get;
            protected set;
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
            get { return m_customCastCell ?? (MarkTrigger != null ? MarkTrigger.Shape.Cell : Caster.Cell); }
            set { m_customCastCell = value; }

        }

        public MapPoint CastPoint
        {
            get { return m_castPoint ?? (m_castPoint = new MapPoint(CastCell)); }
            set { m_castPoint = value; }
        }

        public IFight Fight
        {
            get { return Caster.Fight; }
        }

        public Map Map
        {
            get { return Fight.Map; }
        }

        public abstract bool Initialize();
        public abstract void Execute();

        public virtual IEnumerable<SpellEffectHandler> GetEffectHandlers()
        {
            return new SpellEffectHandler[0];
        }

        public bool IsCastedBySpell(int spellId)
        {
            if (Spell.Id == spellId)
                return true;

            var effect = CastedByEffect;
            while (effect != null && effect.Spell.Id != spellId)
                effect = effect.CastHandler.CastedByEffect;

            return effect != null;
        }
    }
}