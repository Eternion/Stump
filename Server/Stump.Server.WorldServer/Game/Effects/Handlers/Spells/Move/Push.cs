using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_PushBack)]
    [EffectHandler(EffectsEnum.Effect_PushBack_1103)]
    public class Push : SpellEffectHandler
    {
        public Push(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (var actor in GetAffectedActors())
            {
                var referenceCell = TargetedCell.Id == actor.Cell.Id ? new MapPoint(CastCell) : TargetedPoint;

                if (referenceCell.CellId == actor.Position.Cell.Id)
                    continue;

                var pushDirection = referenceCell.OrientationTo(actor.Position.Point, false);
                var startCell = actor.Position.Point;
                var lastCell = startCell;

                for (var i = 0; i < integerEffect.Value; i++)
                {
                    var nextCell = lastCell.GetNearestCellInDirection(pushDirection);

                    if (nextCell == null)
                        break;

                    if (Fight.ShouldTriggerOnMove(Fight.Map.Cells[nextCell.CellId]))
                    {
                        lastCell = nextCell;
                        break;
                    }

                    if (nextCell == null || !Fight.IsCellFree(Map.Cells[nextCell.CellId]))
                    {
                        var pushbackDamages = (8 + new AsyncRandom().Next(1, 8) * (Caster.Level / 50)) * (integerEffect.Value - i);

                        actor.InflictDirectDamage(pushbackDamages, Caster);
                        break;
                    }

                    lastCell = nextCell;
                }

                var endCell = lastCell;
                var actorCopy = actor;
                Fight.ForEach(entry => ActionsHandler.SendGameActionFightSlideMessage(entry.Client, Caster, actorCopy, startCell.CellId, endCell.CellId));

                actor.Position.Cell = Map.Cells[endCell.CellId];
            }

            return true;
        }
    }
}