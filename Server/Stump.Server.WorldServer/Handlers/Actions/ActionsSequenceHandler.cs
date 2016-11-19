using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Sequences;

namespace Stump.Server.WorldServer.Handlers.Actions
{
    public partial class ActionsHandler
    {
        public static void SendSequenceStartMessage(IPacketReceiver client, FightSequence sequence)
        {
            client.Send(new SequenceStartMessage((sbyte) sequence.Type, sequence.Author.Id));
        }

        public static void SendSequenceEndMessage(IPacketReceiver client, FightSequence sequence)
        {
            client.Send(new SequenceEndMessage((short) sequence.Id, sequence.Author.Id, (sbyte)sequence.Type));
        }

        public static void SendSequenceNumberRequestMessage(IPacketReceiver client)
        {
            client.Send(new SequenceNumberRequestMessage());
        }
    }
}