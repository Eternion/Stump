using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class TelefragCriterion : TargetCriterion
    {
        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
            => actor.NeedTelefragState;
    }
}
