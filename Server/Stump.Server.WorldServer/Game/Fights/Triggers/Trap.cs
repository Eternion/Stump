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
        public Trap(short id, FightActor caster, Spell spell, EffectDice originEffect, Spell trapSpell, Cell centerCell, SpellShapeEnum shape, byte minSize, byte size)
            : base(id, caster, spell, originEffect, centerCell, new MarkShape(caster.Fight, centerCell, shape, GetMarkShape(shape), minSize, size, GetTrapColorBySpell(spell)))
        {
            TrapSpell = trapSpell;
            VisibleState = GameActionFightInvisibilityStateEnum.INVISIBLE;
        }

        public bool HasBeenTriggered
        {
            get;
            private set;
        }

        public bool WillBeTriggered
        {
            get;
            set;
        }


        public Spell TrapSpell
        {
            get;
        }

        public GameActionFightInvisibilityStateEnum VisibleState
        {
            get;
            set;
        }

        public override bool StopMovement => !WillBeTriggered;

        public override GameActionMarkTypeEnum Type => GameActionMarkTypeEnum.TRAP;

        public override TriggerType TriggerType => TriggerType.MOVE;

        public override bool DoesSeeTrigger(FightActor fighter) => VisibleState != GameActionFightInvisibilityStateEnum.INVISIBLE || fighter.IsFriendlyWith(Caster);

        public override bool DecrementDuration() => false;

        public override void Trigger(FightActor trigger, Cell triggerCell)
        {
            if (HasBeenTriggered)
                return;

            HasBeenTriggered = true;
            NotifyTriggered(trigger, TrapSpell);
            var handler = SpellManager.Instance.GetSpellCastHandler(Caster, TrapSpell, Shape.Cell, false);
            handler.MarkTrigger = this;
            handler.TriggerCell = triggerCell;
            handler.Initialize();

            foreach (var effectHandler in handler.GetEffectHandlers())
            {
                effectHandler.EffectZone = new Zone(effectHandler.Effect.ZoneShape, Shape.Size);

                if (!effectHandler.GetAffectedActors().Any() && effectHandler.IsValidTarget(trigger))
                    effectHandler.SetAffectedActors(new[] {trigger});
            }

            Remove();
            handler.Execute();
        }

        public override GameActionMark GetHiddenGameActionMark() => new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, Id, (sbyte)Type, -1,
                                      new GameActionMarkedCell[0], true);

        public override GameActionMark GetGameActionMark() => new GameActionMark(Caster.Id, (sbyte)Caster.Team.Id, CastedSpell.Template.Id, (sbyte)CastedSpell.CurrentLevel, Id, (sbyte)Type, CenterCell.Id,
                                      Shape.GetGameActionMarkedCells(), true);

        public override bool CanTrigger(FightActor actor) => !HasBeenTriggered && !WillBeTriggered;

        private static Color GetTrapColorBySpell(Spell spell)
        {
            switch (spell.Id)
            {
                case (int)SpellIdEnum.PIÈGE_MORTEL:
                    return Color.FromArgb(0, 0, 0, 0);
                case (int)SpellIdEnum.PIÈGE_RÉPULSIF:
                    return Color.FromArgb(0, 155, 240, 237);
                case (int)SpellIdEnum.PIÈGE_EMPOISONNÉ:
                    return Color.FromArgb(0, 105, 28, 117);
                case (int)SpellIdEnum.PIÈGE_DE_SILENCE:
                    return Color.FromArgb(0, 49, 45, 134);
                case (int)SpellIdEnum.PIÈGE_D_IMMOBILISATION:
                    return Color.FromArgb(0, 34, 117, 28);
                case (int)SpellIdEnum.PIÈGE_SOURNOIS:
                case (int)SpellIdEnum.PIÈGE_DE_MASSE:
                    return Color.FromArgb(0, 90, 52, 28);
                default:
                    return Color.Brown;
            }
        }
    }
}