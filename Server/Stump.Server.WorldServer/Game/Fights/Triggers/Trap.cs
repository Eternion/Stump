using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Spells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class Trap : MarkTrigger
    {
        public Trap(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Spell glyphSpell, Cell centerCell, byte size)
            : base(id, caster, castedSpell, originEffect, centerCell, new MarkShape(caster.Fight, centerCell, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, size, Color.Brown))
        {
            TrapSpell = glyphSpell;
            VisibleState = GameActionFightInvisibilityStateEnum.INVISIBLE;
        }

        public Trap(short id, FightActor caster, Spell spell, EffectDice originEffect, Spell trapSpell, Cell centerCell, GameActionMarkCellsTypeEnum shape, byte size)
            : base(id, caster, spell, originEffect, centerCell, new MarkShape(caster.Fight, centerCell, shape, size, Color.Brown))
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

        public override bool DecrementDuration()
        {
            return false;
        }

        public override void Trigger(FightActor trigger)
        {
            NotifyTriggered(trigger, TrapSpell);

            foreach (var shape in Shapes)
            {
                var handler = SpellManager.Instance.GetSpellCastHandler(Caster, TrapSpell, shape.Cell, false);
                handler.MarkTrigger = this;
                handler.Initialize();

                foreach (var effectHandler in handler.GetEffectHandlers())
                {
                    effectHandler.EffectZone = new Zone(shape.Shape == GameActionMarkCellsTypeEnum.CELLS_CROSS ? SpellShapeEnum.Q : effectHandler.Effect.ZoneShape, shape.Size);
                }

                handler.Execute();
            }
        }

        public override GameActionMark GetHiddenGameActionMark()
        {
            return new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, (short)CastedSpell.Id, (sbyte)Type, CenterCell.Id,
                                      Shapes.Select(entry => entry.GetGameActionMarkedCell()), true);
        }

        public override GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, (short)CastedSpell.Id, (sbyte)Type, CenterCell.Id,
                                      Shapes.Select(entry => entry.GetGameActionMarkedCell()), true);
        }

        public override bool IsAffected(FightActor actor)
        {
            return true;
        }
    }
}