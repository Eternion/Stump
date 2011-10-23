using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects;
using Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Fights.Triggers
{
    public class Glyph : MarkTrigger
    {
        public Glyph(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell, Cell centerCell, sbyte size, Color color)
            : base(id, caster, castedSpell, originEffect, new MarkShape(caster.Fight, centerCell, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, size, color))
        {
            GlyphSpell = glyphSpell;
            Duration = originEffect.Duration;
        }

        public Glyph(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell, Cell centerCell, GameActionMarkCellsTypeEnum type, sbyte size, Color color)
            : base(id, caster, castedSpell, originEffect, new MarkShape(caster.Fight, centerCell, type, size, color))
        {
            GlyphSpell = glyphSpell;
            Duration = originEffect.Duration;
        }


        public Glyph(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell, params MarkShape[] shapes)
            : base(id, caster, castedSpell, originEffect, shapes)
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

        public bool DecrementDuration()
        {
            return (Duration--) <= 0;
        }

        public override GameActionMarkTypeEnum Type
        {
            get { return GameActionMarkTypeEnum.GLYPH; }
        }

        public override TriggerType TriggerType
        {
            get { return TriggerType.TURN_BEGIN; }
        }

        public override void Trigger(FightActor trigger)
        {
            NotifyTriggered(trigger, GlyphSpell);

            foreach (EffectDice effect in GlyphSpell.CurrentSpellLevel.Effects)
            {
                SpellEffectHandler handler = EffectManager.Instance.GetSpellEffectHandler(effect, Caster, GlyphSpell, trigger.Cell, false);

                handler.Apply();
            }
        }

        public override GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, CastedSpell.Id, Id, (sbyte) Type, Shapes.Select(entry => entry.GetGameActionMarkedCell()));
        }
    }
}