using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_Rollback)]
    public class Rollback : SpellEffectHandler
    {
        public Rollback(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var fighters = Fight.GetAllFighters(x => x.IsAlive() && !(x is SummonedFighter) && !(x is SummonedBomb));
            foreach (var fighter in fighters)
            {
                var newCell = fighter.FightStartPosition.Cell;

                var oldFighter = Fight.GetOneFighter(newCell);
                if (oldFighter != null)
                    fighter.ExchangePositions(oldFighter);
                else
                {
                    fighter.Position.Cell = newCell;

                    ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(Fight.Clients, Caster, fighter, newCell);               
                }
            }

            return true;
        }

        private void MoveOldFighter(FightActor oldFighter)
        {
            var adjacentCell = oldFighter.Position.Point
                .GetAdjacentCells(c => Fight.IsCellFree(Map.Cells[c]))
                .FirstOrDefault();
            if (adjacentCell != null)
                oldFighter.Position.Cell = Map.Cells[adjacentCell.CellId];
            else
                oldFighter.Die();
        }
    }
}