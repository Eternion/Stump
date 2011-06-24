
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Commands
{
    public interface IInGameTrigger
    {
        Character Character
        {
            get;
        }
    }
}