using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Fights.Triggers
{
    public class Trap : MarkTrigger
    {
        public Trap(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell, Cell centerCell, sbyte size)
            : base(id, caster, castedSpell, originEffect, new MarkShape(caster.Fight, centerCell, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, size, Color.Brown))
        {
            TrapSpell = glyphSpell;
        }

        public Trap(short id, FightActor caster, Spell spell, EffectDice originEffect, Spell trapSpell, Cell centerCell, GameActionMarkCellsTypeEnum shape, sbyte size)
            : base(id, caster, spell, originEffect, new MarkShape(caster.Fight, centerCell, shape, size, Color.Brown))
        {
            TrapSpell = trapSpell;
        }

        public Trap(short id, FightActor caster, Spell spell, EffectDice originEffect, Spell trapSpell, params MarkShape[] shapes)
            : base(id, caster, spell, originEffect ,shapes)
        {
            TrapSpell = trapSpell;
        }

        public Spell TrapSpell
        {
            get;
            private set;
        }

        public override GameActionMarkTypeEnum Type
        {
            get { return GameActionMarkTypeEnum.TRAP; }
        }

        public override TriggerType TriggerType
        {
            get { return TriggerType.MOVE; }
        }

        public override void Trigger(FightActor trigger)
        {
            NotifyTriggered(trigger, TrapSpell);

            foreach (var shape in Shapes)
            {
                foreach (var effect in TrapSpell.CurrentSpellLevel.Effects)
                {
                    var handler = EffectManager.Instance.GetSpellEffectHandler(effect, Caster, TrapSpell, shape.Cell, false);
                    handler.EffectZone = new Zone(shape.Shape == GameActionMarkCellsTypeEnum.CELLS_CROSS ? SpellShapeEnum.Q : SpellShapeEnum.C, (uint)shape.Size);

                    handler.Apply();
                }
            }
        }

        public override GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, CastedSpell.Id, Id, (sbyte) Type, Shapes.Select(entry => entry.GetGameActionMarkedCell()));
        }
    }
}