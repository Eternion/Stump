using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Handlers.Context;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_RevealsInvisible)]
    public class RevealsInvisible : SpellEffectHandler
    {
        public RevealsInvisible(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical) : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var containedTraps = Fight.GetTriggers().OfType<Trap>().Where(entry => entry.VisibleState == GameActionFightInvisibilityStateEnum.INVISIBLE && 
                Caster.IsEnnemyWith(entry.Caster) &&
                AffectedCells.Contains(entry.Shape.Cell));

            foreach (var trap in containedTraps)
            {
                trap.VisibleState = GameActionFightInvisibilityStateEnum.VISIBLE;
                ContextHandler.SendGameActionFightMarkCellsMessage(Fight.Clients, trap);
            }

            foreach (var target in GetAffectedActors().Where(target => target.VisibleState == GameActionFightInvisibilityStateEnum.INVISIBLE && target.IsEnnemyWith(Caster)))
            {
                target.SetInvisibilityState(GameActionFightInvisibilityStateEnum.VISIBLE);
            }

            return true;
        }
    }
}