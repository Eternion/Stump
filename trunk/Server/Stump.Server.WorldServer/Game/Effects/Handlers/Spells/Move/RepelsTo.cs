using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

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

            var startCell = target.Cell;
            target.Cell = TargetedCell;

            Fight.ForEach(entry => ActionsHandler.SendGameActionFightSlideMessage(entry.Client, Caster, target, startCell.Id, target.Cell.Id));

            return true;
        }
    }
}