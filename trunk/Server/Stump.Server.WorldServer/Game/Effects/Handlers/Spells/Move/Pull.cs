using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_PullForward)]
    public class Pull : SpellEffectHandler
    {
        public Pull(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return false;

            foreach (FightActor actor in GetAffectedActors())
            {
                MapPoint referenceCell;
                if (TargetedCell.Id == actor.Cell.Id)
                    referenceCell = new MapPoint(CastCell);
                else
                    referenceCell = TargetedPoint;

                if (referenceCell.CellId == actor.Position.Cell.Id)
                    continue;

                var pushDirection = actor.Position.Point.OrientationTo(referenceCell, false);
                var startCell = actor.Position.Point;
                var lastCell = startCell;

                for (int i = 0; i < integerEffect.Value; i++)
                {
                    var nextCell = lastCell.GetNearestCellInDirection(pushDirection);

                    if (nextCell == null)
                        break;

                    if (Fight.ShouldTriggerOnMove(Fight.Map.Cells[nextCell.CellId]))
                    {
                        lastCell = nextCell;
                        break;
                    } 
                    
                    if (!Fight.IsCellFree(Map.Cells[nextCell.CellId]))
                    {
                        break;
                    }

                    lastCell = nextCell;
                }

                var endCell = lastCell;

                actor.Position.Cell = Map.Cells[endCell.CellId];

                var actorCopy = actor;
                ActionsHandler.SendGameActionFightSlideMessage(Fight.Clients, Caster, actorCopy, startCell.CellId, endCell.CellId);
            }

            return true;
        }
    }
}