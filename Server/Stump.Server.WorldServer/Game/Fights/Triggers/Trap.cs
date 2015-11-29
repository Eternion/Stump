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
        public Trap(short id, FightActor caster, Spell spell, EffectDice originEffect, Spell trapSpell, Cell centerCell, SpellShapeEnum shape, byte size)
            : base(id, caster, spell, originEffect, centerCell, new MarkShape(caster.Fight, centerCell, shape, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, size, GetTrapColorBySpell(spell)))
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
            return new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, Id, (sbyte)Type, -1,
                                      new GameActionMarkedCell[0], true);
        }

        public override GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, Id, (sbyte)Type, CenterCell.Id,
                                      Shapes.Select(entry => entry.GetGameActionMarkedCell()), true);
        }

        public override bool CanTrigger(FightActor actor)
        {
            return true;
        }

        private static Color GetTrapColorBySpell(Spell spell)
        {
            switch (spell.Id)
            {
                case (int)SpellIdEnum.PIÈGE_MORTEL:
                    return Color.Black;
                case (int)SpellIdEnum.PIÈGE_RÉPULSIF:
                    return Color.LightBlue;
                case (int)SpellIdEnum.PIÈGE_EMPOISONNÉ:
                    return Color.Violet;
                case (int)SpellIdEnum.PIÈGE_DE_SILENCE:
                    return Color.Blue;
                case (int)SpellIdEnum.PIÈGE_D_IMMOBILISATION:
                    return Color.Green;
                default:
                    return Color.Brown;
            }
        }
    }
}