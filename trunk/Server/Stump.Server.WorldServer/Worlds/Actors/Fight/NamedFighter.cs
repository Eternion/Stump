using Stump.Server.WorldServer.Worlds.Fights;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public abstract class NamedFighter : FightActor
    {
        protected NamedFighter(FightTeam team)
            : base(team)
        {
        }

        public abstract string Name
        {
            get;
        }
    }
}