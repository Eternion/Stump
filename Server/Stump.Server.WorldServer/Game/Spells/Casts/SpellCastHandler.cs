using System.Collections.Generic;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    public abstract class SpellCastHandler
    {
        protected SpellCastHandler(SpellCastInformations informations)
        {
            Informations = informations;
        }

        private MapPoint m_castPoint;
        private Cell m_customCastCell;
        public SpellCastInformations Informations
        {
            get;
        }

        public FightActor Caster => Informations.Caster;

        public Spell Spell => Informations.Spell;

        public CastSpellEffect CastedByEffect => Informations.TriggerEffect;

        public SpellLevelTemplate SpellLevel
        {
            get { return Spell.CurrentSpellLevel; }
        }

        public Cell TargetedCell => Informations.TargetedCell;
        
        public bool Critical => Informations.Critical == FightSpellCastCriticalEnum.CRITICAL_HIT;
        public virtual bool SilentCast => Informations.Silent;

        public MarkTrigger MarkTrigger
        {
            get;
            set;
        }

        public Cell TriggerCell
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

        public virtual bool SeeCast(Character character) => true;
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