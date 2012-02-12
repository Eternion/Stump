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

        public override void Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            foreach (FightActor actor in GetAffectedActors())
            {
                if (CastCell.Id == actor.Cell.Id)
                    continue; // in case of traps it is possible

                var castPoint = new MapPoint(CastCell);
                var pushDirection = actor.Position.Point.OrientationTo(castPoint, false);
                var startCell = actor.Position.Point;
                var lastCell = startCell;

                for (int i = 0; i < integerEffect.Value; i++)
                {
                    var nextCell = lastCell.GetNearestCellInDirection(pushDirection);

                    if (!Fight.IsCellFree(Map.Cells[nextCell.CellId]))
                    {
                        var pullDamages = ( 8 + new AsyncRandom().Next(1, 8) * ( Caster.Level / 50 ) ) * ( integerEffect.Value - i );

                        actor.InflictDamage((short) pullDamages, EffectSchoolEnum.Unknown, Caster);
                        break;
                    }

                    lastCell = nextCell;
                }

                var endCell = lastCell;

                actor.Position.Cell = Map.Cells[endCell.CellId];

                var actorCopy = actor;
                ActionsHandler.SendGameActionFightSlideMessage(Fight.Clients, Caster, actorCopy, startCell.CellId, endCell.CellId);
            }
        }
    }
}