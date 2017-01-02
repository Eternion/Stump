using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class SpellCastInformations
    {
        public SpellCastInformations(FightActor caster, Spell spell, Cell cell, bool critical = false)
        {
            Caster = caster;
            Spell = spell;
            TargetedCell = cell;
            Critical = critical ? FightSpellCastCriticalEnum.CRITICAL_HIT : FightSpellCastCriticalEnum.NORMAL;
            CastCell = caster.Cell;
            Efficiency = 1;
        }

        public FightActor Caster
        {
            get;
        }

        public Cell TargetedCell
        {
            get;
        }

        public Cell CastCell
        {
            get;
            set;
        }

        public Spell Spell
        {
            get;
        }

        public SpellLevelTemplate SpellLevel => Spell.CurrentSpellLevel;

        public FightSpellCastCriticalEnum Critical
        {
            get;
            set;
        }

        public CastSpellEffect TriggerEffect
        {
            get;
            set;
        }

        public bool Silent
        {
            get;
            set;
        }

        public bool ApFree
        {
            get;
            set;
        }

        // if contains SpellCastResult => bypass all conditions
        public SpellCastResult[] BypassedConditions
        {
            get;
            set;
        }

        public bool Triggered => TriggerEffect != null;

        public FightActor Triggerer
        {
            get;
            set;
        }

        public bool Force
        {
            get;
            set;
        }

        public double Efficiency
        {
            get;
            set;
        }

        public bool IsConditionBypassed(SpellCastResult result) => BypassedConditions != null && (BypassedConditions.Contains(result) || BypassedConditions.Contains(SpellCastResult.OK));
    }
}