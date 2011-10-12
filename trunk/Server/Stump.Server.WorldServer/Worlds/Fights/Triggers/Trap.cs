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
        public Trap(short id, FightActor caster, Spell spell, Spell trapSpell, Cell centerCell, sbyte size, GameActionMarkCellsTypeEnum shape)
            : base(id, caster, spell, new MarkShape(caster.Fight, centerCell, shape, size, Color.Brown))
        {
            TrapSpell = trapSpell;
        }

        public Trap(short id, FightActor caster, Spell spell, Spell trapSpell, params MarkShape[] shapes)
            : base(id, caster, spell, shapes)
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
                    handler.EffectZone = new Zone(shape.Shape == GameActionMarkCellsTypeEnum.CELLS_CROSS ? SpellShapeEnum.X : SpellShapeEnum.C, (uint)shape.Size);

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