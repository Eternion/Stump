using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Handlers.Actions
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        public static void SendSequenceStartMessage(IPacketReceiver client, FightActor entity, SequenceTypeEnum sequenceType)
        {
            client.Send(new SequenceStartMessage((sbyte) sequenceType, entity.Id));
        }

        public static void SendSequenceEndMessage(IPacketReceiver client, FightActor entity, SequenceTypeEnum sequenceType,
                                                  SequenceTypeEnum lastSequenceType)
        {
            client.Send(new SequenceEndMessage((short) lastSequenceType, entity.Id, (sbyte)sequenceType));
        }
    }
}