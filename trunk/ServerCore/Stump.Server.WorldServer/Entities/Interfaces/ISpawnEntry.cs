
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities
{
    public interface ISpawnEntry
    {
        int ContextualId
        {
            get;
        }

        ExtendedLook Look
        {
            get;
            set;
        }

        ObjectPosition Position
        {
            get;
        }

        GameRolePlayActorInformations ToNetworkActor(WorldClient client);
    }
}