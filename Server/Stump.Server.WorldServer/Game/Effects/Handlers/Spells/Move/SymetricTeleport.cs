using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_SymetricTargetTeleport)]
    [EffectHandler(EffectsEnum.Effect_SymetricCasterTeleport)]
    [EffectHandler(EffectsEnum.Effect_SymetricPointTeleport)]
    public class SymetricTeleport : SpellEffectHandler
    {
        public SymetricTeleport(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var target in GetAffectedActors())
            {
                var casterPoint = Caster.Position.Point;
                var targetPoint = target.Position.Point;

                switch (Effect.EffectId)
                {
                    case EffectsEnum.Effect_SymetricCasterTeleport:
                        casterPoint = target.Position.Point;
                        targetPoint = Caster.Position.Point;
                        break;
                    case EffectsEnum.Effect_SymetricPointTeleport:
                        casterPoint = target.Position.Point;
                        targetPoint = TargetedPoint;
                        break;
                }

                var cell = new MapPoint((2 * targetPoint.X - casterPoint.X), (2 * targetPoint.Y - casterPoint.Y));

                var dstCell = Map.GetCell(cell.CellId);

                if (dstCell == null)
                    continue;

                if (!dstCell.Walkable && !dstCell.NonWalkableDuringFight)
                    continue;

                var fighter = Fight.GetOneFighter(dstCell);
                if (fighter != null && fighter != target)
                {
                    var caster = Caster;

                    if (Effect.EffectId == EffectsEnum.Effect_SymetricCasterTeleport || Effect.EffectId == EffectsEnum.Effect_SymetricPointTeleport)
                        caster = target;

                    caster.Telefrag(Caster, fighter);
                }
                else
                {
                    var caster = Caster;

                    if (Effect.EffectId == EffectsEnum.Effect_SymetricCasterTeleport || Effect.EffectId == EffectsEnum.Effect_SymetricPointTeleport)
                        caster = target;

                    caster.Position.Cell = dstCell;
                    Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, caster, caster, dstCell), true);
                }
            }

            return true;
        }
    }
}
