using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;

namespace Stump.Server.WorldServer.Game.Actors.Interfaces
{
    public interface ISummoned
    {
        FightActor Summoner
        {
            get;
        }


        SpellEffectHandler SummoningEffect
        {
            get;
            set;
        }

    }
}