using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities
{
    public interface ISpawnEntry
    {
        int ContextualId
        {
            get;
        }

        EntityLook Look
        {
            get;
            set;
        }

        VectorIsometric Location
        {
            get;
        }

        GameRolePlayActorInformations ToNetworkActor(WorldClient client);
    }
}