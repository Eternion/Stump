using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Zobal
{
    [SpellCastHandler(SpellIdEnum.APPUI)]
    [SpellCastHandler(SpellIdEnum.APPUI_DU_DOPEUL)]
    public class AppuiCastHandler : DefaultSpellCastHandler
    {
        public AppuiCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            Handlers[0].AddAffectedActor(Caster);

            TargetedPoint = new MapPoint(TargetedCell);
            var direction = Caster.Position.Point.OrientationTo(TargetedPoint);
            var oppositeDirection = TargetedPoint.GetOppositeDirection(direction);

            TargetedPoint = Caster.Position.Point.GetNearestCellInDirection(oppositeDirection);
            TargetedCell = Map.Cells[TargetedPoint.CellId];

            base.Execute();
        }
    }
}
