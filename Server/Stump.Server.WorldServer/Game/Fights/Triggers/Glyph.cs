using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class Glyph : MarkTrigger
    {
        private static readonly int[] SPELLS_GLYPH_END_TURN = {13, 2035}; // glyphe de répulsion

        public Glyph(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell,
                     Cell centerCell, byte size, Color color)
            : base(
                id, caster, castedSpell, originEffect, centerCell,
                new MarkShape(caster.Fight, centerCell, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, size, color))
        {
            GlyphSpell = glyphSpell;
            Duration = originEffect.Duration;
        }

        public Glyph(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell,
                     Cell centerCell, GameActionMarkCellsTypeEnum type, byte size, Color color)
            : base(id, caster, castedSpell, originEffect, centerCell, new MarkShape(caster.Fight, centerCell, type, size, color))
        {
            GlyphSpell = glyphSpell;
            Duration = originEffect.Duration;
        }

        public Spell GlyphSpell
        {
            get;
            private set;
        }

        public int Duration
        {
            get;
            private set;
        }

        public override GameActionMarkTypeEnum Type
        {
            get { return GameActionMarkTypeEnum.GLYPH; }
        }

        public override TriggerType TriggerType
        {
            get { return SPELLS_GLYPH_END_TURN.Contains(CastedSpell.Id) ? TriggerType.TURN_END : TriggerType.TURN_BEGIN; }
        }

        public override bool DecrementDuration()
        {
            if (Duration == -1)
                return false;

            return (--Duration) <= 0;
        }

        public override void Trigger(FightActor trigger)
        {
            NotifyTriggered(trigger, GlyphSpell);

            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, GlyphSpell, trigger.Cell, false);
            handler.MarkTrigger = this;
            handler.Initialize();
            handler.Execute();
        }

        public override GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, CastedSpell.Id, Id, (sbyte) Type,
                                      Shapes.Select(entry => entry.GetGameActionMarkedCell()));
        }

        public override GameActionMark GetHiddenGameActionMark()
        {
            return GetGameActionMark();
        }

        public override bool DoesSeeTrigger(FightActor fighter)
        {
            return true;
        }

        public override bool IsAffected(FightActor actor)
        {
            return true;
        }
    }
}