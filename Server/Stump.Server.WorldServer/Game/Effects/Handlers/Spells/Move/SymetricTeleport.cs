using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_SymetricTargetTeleport)]
    [EffectHandler(EffectsEnum.Effect_SymetricCasterTeleport)]
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

            if (Effect.EffectId == EffectsEnum.Effect_SymetricCasterTeleport)
            {
                casterPoint = target.Position.Point;
                targetPoint = Caster.Position.Point;
            }

            var cell = new MapPoint((2 * targetPoint.X - casterPoint.X), (2 * targetPoint.Y - casterPoint.Y));

            if (cell == null)
                return false;

            var dstCell = Map.GetCell(cell.CellId);

            if (dstCell == null)
                return false;

            if (!dstCell.Walkable && !dstCell.NonWalkableDuringFight)
                return false;

            var fighter = Fight.GetOneFighter(dstCell);
            if (fighter != null)
            {
                var caster = Caster;

                if (Effect.EffectId == EffectsEnum.Effect_SymetricCasterTeleport)
                    caster = target;

                if (!caster.Telefrag(Caster, fighter, Spell))
                    return false;
            }
            else
            {
                var caster = Caster;

                if (Effect.EffectId == EffectsEnum.Effect_SymetricCasterTeleport)
                    caster = target;

                caster.Position.Cell = dstCell;
                Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, caster, caster, dstCell), true);
            }

            return true;
        }
    }
}
