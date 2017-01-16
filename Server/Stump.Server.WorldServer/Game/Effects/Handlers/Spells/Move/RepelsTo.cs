using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_RepelsTo)]
    public class RepelsTo : SpellEffectHandler
    {
        public RepelsTo(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical) : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var orientation = CastPoint.OrientationTo(TargetedPoint);
            var target = Fight.GetFirstFighter<FightActor>(entry => entry.Position.Cell.Id == CastPoint.GetCellInDirection(orientation, 1).CellId);
            
            if (target == null)
                return false;

            if (!target.CanBePushed())
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

                if (!Fight.ShouldTriggerOnMove(Fight.Map.Cells[cell.CellId], target))
                    continue;

                endCell = Fight.Map.Cells[cell.CellId];
                break;
            }

            if (target.IsCarrying())
                target.ThrowActor(Map.Cells[startCell.Id], true);

            if (target.IsAlive())
            {
                foreach (var character in Fight.GetCharactersAndSpectators().Where(target.IsVisibleFor))
                    ActionsHandler.SendGameActionFightSlideMessage(character.Client, Caster, target, startCell.Id, endCell.Id);
            }
            
            target.Cell = endCell;
            target.TriggerBuffs(Caster, BuffTriggerType.OnMoved);

            return true;
        }
    }
}