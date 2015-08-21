using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_SymetricTeleport)]
    public class SymetricTeleport : SpellEffectHandler
    {
        public SymetricTeleport(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var target = GetAffectedActors().FirstOrDefault();

            if (target == null)
                return false;

            var casterPoint = Caster.Position.Point;
            var targetPoint = target.Position.Point;

            var distance = casterPoint.EuclideanDistanceTo(targetPoint);
            var direction = casterPoint.OrientationTo(targetPoint, true);

            var cell = target.Position.Point.GetCellInDirection(direction, (short)distance);

            if (cell == null)
                return false;

            var dstCell = Map.GetCell(cell.CellId);

            if (dstCell == null)
                return false;

            if (!Fight.IsCellFree(dstCell) || !dstCell.Walkable)
                return false;

            Caster.Position.Cell = dstCell;

            Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, Caster, Caster, dstCell), true);

            return true;
        }
    }
}
