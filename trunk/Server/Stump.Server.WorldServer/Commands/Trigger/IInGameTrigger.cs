using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Trigger
{
    public interface IInGameTrigger
    {
        Character Character
        {
            get;
        }
    }
}