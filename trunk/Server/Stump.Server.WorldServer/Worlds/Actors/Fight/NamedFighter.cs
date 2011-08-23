namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public abstract class NamedFighter : FightActor
    {
        public abstract string Name
        {
            get;
        }
    }
}