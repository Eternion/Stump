using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_ReturnToOriginalPos)]
    [EffectHandler(EffectsEnum.Effect_ReturnToLastPos)]
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
                var lastCell = Effect.EffectId == EffectsEnum.Effect_ReturnToLastPos ?
                    fighter.LastPositions.Reverse().ElementAtOrDefault(1) : fighter.FightStartPosition.Cell;
                var newCell = lastCell == null ? fighter.FightStartPosition.Cell : lastCell;

                var oldFighter = Fight.GetOneFighter(newCell);
                if (oldFighter != null)
                    fighter.Telefrag(Caster, oldFighter, Spell);
                else
                {
                    fighter.Position.Cell = newCell;

                    ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(Fight.Clients, Caster, fighter, newCell);
                }
            }

            return true;
        }
    }
}