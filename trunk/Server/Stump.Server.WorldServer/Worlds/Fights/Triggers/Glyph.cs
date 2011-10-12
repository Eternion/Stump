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
        public Glyph(short id, FightActor caster, Spell castedSpell, Spell glyphSpell, Cell centerCell, sbyte size)
            : base(id, caster, castedSpell, new MarkShape(caster.Fight, centerCell, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, size, Color.DarkRed))
        {
            GlyphSpell = glyphSpell;
        }

        public Glyph(short id, FightActor caster, Spell castedSpell, Spell glyphSpell, params MarkShape[] shapes)
            : base(id, caster, castedSpell, shapes)
        {
            GlyphSpell = glyphSpell;
        }

        public Spell GlyphSpell
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