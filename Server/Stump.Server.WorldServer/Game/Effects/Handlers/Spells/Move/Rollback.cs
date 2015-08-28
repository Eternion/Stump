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
                var lastPos = actor.LastPositions.Reverse().Where(x => x.First != actor.Cell
                                && x.Second >= (Fight.TimeLine.RoundNumber - 2)).FirstOrDefault();

                var newCell = actor.FightStartPosition.Cell;

                if (Effect.EffectId == EffectsEnum.Effect_ReturnToLastPos && lastPos == null)
                    continue;
                else  if (Effect.EffectId == EffectsEnum.Effect_ReturnToLastPos)
                    newCell = lastPos.First;

                var fighter = Fight.GetOneFighter(newCell);
                if (fighter != null && fighter != actor)
                    actor.Telefrag(Caster, fighter, Spell);
                else
                {
                    actor.Position.Cell = newCell;

                    ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(Fight.Clients, Caster, actor, newCell);
                }

                if (Effect.EffectId == EffectsEnum.Effect_ReturnToLastPos)
                {
                    actor.LastPositions.RemoveLast();
                    actor.LastPositions.RemoveLast();
                }
                    
            }

            return true;
        }
    }
}