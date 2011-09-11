using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

namespace Stump.Server.WorldServer.Handlers.Actions
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        public static void SendSequenceStartMessage(WorldClient client, FightActor entity, SequenceTypeEnum sequenceType)
        {
            client.Send(new SequenceStartMessage((sbyte) sequenceType, entity.Id));
        }

        public static void SendSequenceEndMessage(WorldClient client, FightActor entity, short actionId,
                                                  SequenceTypeEnum sequenceType)
        {
            client.Send(new SequenceEndMessage(actionId, entity.Id, (sbyte) sequenceType));
        }
    }
}