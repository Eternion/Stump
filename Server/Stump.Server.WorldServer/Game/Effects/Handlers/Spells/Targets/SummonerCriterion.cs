using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class SummonerCriterion : TargetCriterion
    {
        public SummonerCriterion(bool required)
        {
            Required = required;
        }

        public bool Required
        {
            get;
            set;
        }

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            return Required == (actor == handler.Caster || 
                (actor.Summoner != null && (actor.Summoner == handler.Caster || actor.Summoner == handler.Caster.Summoner)) ||
                handler.Caster.Summoner != null && handler.Caster.Summoner == actor);
        }
    }
}