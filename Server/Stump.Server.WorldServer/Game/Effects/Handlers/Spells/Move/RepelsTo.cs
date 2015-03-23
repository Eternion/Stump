using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_RepelsTo)]
    public class RepelsTo : SpellEffectHandler
    {
        public RepelsTo(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var orientation = CastPoint.OrientationTo(TargetedPoint);
            var target = Fight.GetFirstFighter<FightActor>(entry => entry.Position.Cell.Id == CastPoint.GetCellInDirection(orientation, 1).CellId);
            
            if (target == null)
                return false;

            if (target.HasState((int)SpellStatesEnum.Unmovable) || target.HasState((int)SpellStatesEnum.Rooted))
                return false;

            var startCell = target.Cell;
            var endCell = TargetedCell;
            var cells = new MapPoint(startCell).GetCellsOnLineBetween(TargetedPoint);

            for (var index = 0; index < cells.Length; index++)
            {
                var cell = cells[index];
                if (!Fight.IsCellFree(Fight.Map.Cells[cell.CellId]))
                {
                    endCell = index > 0 ? Fight.Map.Cells[cells[index - 1].CellId] : startCell;
                    break;
                }

                if (!Fight.ShouldTriggerOnMove(Fight.Map.Cells[cell.CellId]))
                    continue;

                endCell = Fight.Map.Cells[cell.CellId];
                break;
            }

            target.Cell = endCell;
            target.OnActorMoved(Caster, false);

            if (target.IsCarrying())
                target.ThrowActor(Map.Cells[startCell.Id], true);

            Fight.ForEach(entry => ActionsHandler.SendGameActionFightSlideMessage(entry.Client, Caster, target, startCell.Id, target.Cell.Id));

            return true;
        }
    }
}