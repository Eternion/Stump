using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class Trap : MarkTrigger
    {
        public Trap(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell, Cell centerCell, byte size)
            : base(id, caster, castedSpell, originEffect, new MarkShape(caster.Fight, centerCell, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, size, Color.Brown))
        {
            TrapSpell = glyphSpell;
            VisibleState = GameActionFightInvisibilityStateEnum.INVISIBLE;
        }

        public Trap(short id, FightActor caster, Spell spell, EffectDice originEffect, Spell trapSpell, Cell centerCell, GameActionMarkCellsTypeEnum shape, byte size)
            : base(id, caster, spell, originEffect, new MarkShape(caster.Fight, centerCell, shape, size, Color.Brown))
        {
            TrapSpell = trapSpell;
            VisibleState = GameActionFightInvisibilityStateEnum.INVISIBLE;
        }

        public Trap(short id, FightActor caster, Spell spell, EffectDice originEffect, Spell trapSpell, params MarkShape[] shapes)
            : base(id, caster, spell, originEffect ,shapes)
        {
            TrapSpell = trapSpell;
            VisibleState = GameActionFightInvisibilityStateEnum.INVISIBLE;
        }

        public Spell TrapSpell
        {
            get;
            private set;
        }

        public GameActionFightInvisibilityStateEnum VisibleState
        {
            get;
            set;
        }

        public override GameActionMarkTypeEnum Type
        {
            get { return GameActionMarkTypeEnum.TRAP; }
        }

        public override TriggerType TriggerType
        {
            get { return TriggerType.MOVE; }
        }

        public override bool DoesSeeTrigger(FightActor fighter)
        {
            return VisibleState != GameActionFightInvisibilityStateEnum.INVISIBLE || fighter.IsFriendlyWith(Caster);
        }

        public override void Trigger(FightActor trigger)
        {
            NotifyTriggered(trigger, TrapSpell);

            foreach (var shape in Shapes)
            {
                foreach (var effect in TrapSpell.CurrentSpellLevel.Effects)
                {
                    var handler = EffectManager.Instance.GetSpellEffectHandler(effect, Caster, TrapSpell, shape.Cell, false);
                    handler.EffectZone = new Zone(shape.Shape == GameActionMarkCellsTypeEnum.CELLS_CROSS ? SpellShapeEnum.Q : effect.ZoneShape, shape.Size);
                    handler.MarkTrigger = this;

                    handler.Apply();
                }
            }
        }

        public override GameActionMark GetHiddenGameActionMark()
        {
            return new GameActionMark(Caster.Id, CastedSpell.Id, Id, (sbyte)Type, new GameActionMarkedCell[0]);
        }

        public override GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, CastedSpell.Id, Id, (sbyte) Type, Shapes.Select(entry => entry.GetGameActionMarkedCell()));
        }
    }
}