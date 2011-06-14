
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        public static void SendSequenceStartMessage(WorldClient client, Entity entity, SequenceTypeEnum sequenceType)
        {
            client.Send(new SequenceStartMessage((int) entity.Id, (int) sequenceType));
        }

        public static void SendSequenceEndMessage(WorldClient client, Entity entity, int actionId,
                                                  SequenceTypeEnum sequenceType)
        {
            client.Send(new SequenceEndMessage((uint) actionId, (int) entity.Id, (int) sequenceType));
        }
    }
}