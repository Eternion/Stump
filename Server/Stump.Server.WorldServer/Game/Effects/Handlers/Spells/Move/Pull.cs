using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_PullForward)]
    public class Pull : SpellEffectHandler
    {
        public Pull(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        public uint Distance
        {
            get;
            set;
        }

        protected override bool InternalApply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            if (Distance == 0)
                Distance = (uint)integerEffect.Value;

            foreach (var actor in GetAffectedActors().OrderBy(entry => entry.Position.Point.ManhattanDistanceTo(TargetedPoint)))
            {
                if (!actor.CanBePushed() || actor.HasState((int)SpellStatesEnum.INEBRANLABLE_157))
                    continue;

                var referenceCell = TargetedCell.Id == actor.Cell.Id ? CastPoint : TargetedPoint;

                if (referenceCell.CellId == actor.Position.Cell.Id)
                    continue;

                var pushDirection = actor.Position.Point.OrientationTo(referenceCell);
                var startCell = actor.Position.Point;
                var lastCell = startCell;

                for (var i = 0; i < Distance; i++)
                {
                    var nextCell = lastCell.GetNearestCellInDirection(pushDirection);

                    if (nextCell == null)
                        break;

                    if (!Fight.IsCellFree(Map.Cells[nextCell.CellId]))
                    {
                        break;
                    }

                    if (Fight.ShouldTriggerOnMove(Fight.Map.Cells[nextCell.CellId], actor))
                    {
                        lastCell = nextCell;
                        break;
                    } 

                    lastCell = nextCell;
                }

                var endCell = lastCell;
                var actorCopy = actor;

                if (actor.IsCarrying())
                    actor.ThrowActor(Map.Cells[startCell.CellId], true);

                foreach (var character in Fight.GetCharactersAndSpectators().Where(actorCopy.IsVisibleFor))
                    ActionsHandler.SendGameActionFightSlideMessage(character.Client, Caster, actorCopy, startCell.CellId, endCell.CellId);

                actor.Position.Cell = Map.Cells[endCell.CellId];
                actor.OnActorMoved(Caster, false);

                Caster.TriggerBuffs(Caster, BuffTriggerType.OnPush);
            }

            return true;
        }
    }
}