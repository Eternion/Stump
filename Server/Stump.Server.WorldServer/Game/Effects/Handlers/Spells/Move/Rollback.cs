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
            foreach (var actor in GetAffectedActors(x => !(x is SummonedFighter) && !(x is SummonedBomb)))
            {
                var lastCell = Effect.EffectId == EffectsEnum.Effect_ReturnToLastPos ?
                    actor.LastPositions.Reverse().ElementAtOrDefault(1) : actor.FightStartPosition.Cell;
                var newCell = lastCell == null ? actor.FightStartPosition.Cell : lastCell;

                var fighter = Fight.GetOneFighter(newCell);
                if (fighter != null)
                    actor.Telefrag(Caster, fighter, Spell);
                else
                {
                    actor.Position.Cell = newCell;

                    ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(Fight.Clients, Caster, actor, newCell);
                }
            }

            return true;
        }
    }
}