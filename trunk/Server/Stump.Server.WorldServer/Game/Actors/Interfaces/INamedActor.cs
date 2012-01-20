namespace Stump.Server.WorldServer.Game.Actors.Interfaces
{
    public interface INamedActor
    {
        int Id
        {
            get;
        }

        string Name
        {
            get;
        }
    }
}