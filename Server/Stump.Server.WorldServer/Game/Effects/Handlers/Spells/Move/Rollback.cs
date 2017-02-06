using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_ReturnToOriginalPos)]
    [EffectHandler(EffectsEnum.Effect_ReturnToLastPos)]
    public class Rollback : SpellEffectHandler
    {
        public Rollback(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var newCell = Effect.EffectId == EffectsEnum.Effect_ReturnToLastPos ? actor.MovementHistory.PopWhile(x=> x.Cell == actor.Cell, 2)?.Cell : actor.FightStartPosition?.Cell;

                if (newCell == null)
                    continue;

                var fighter = Fight.GetOneFighter(newCell);
                if (fighter != null && fighter != actor)
                    actor.Telefrag(Caster, fighter);
                else
                {
                    actor.Position.Cell = newCell;
                    actor.MovementHistory.PopPreviousPosition(); // we don't want a new entry caused by the rollback
                    ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(Fight.Clients, Caster, actor, newCell);
                }
            }

            return true;
        }
    }
}