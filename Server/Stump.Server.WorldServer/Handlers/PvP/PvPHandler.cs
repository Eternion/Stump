using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.PvP
{
    public class PvPHandler : WorldHandlerContainer
    {
        [WorldHandler(SetEnablePVPRequestMessage.Id)]
        public static void HandleSetEnablePVPRequestMessage(WorldClient client, SetEnablePVPRequestMessage message)
        {
            client.Character.TogglePvPMode(message.enable);
        }

        public static void SendAlignmentRankUpdateMessage(IPacketReceiver client)
        {
            client.Send(new AlignmentRankUpdateMessage(1, false));
        }

        public static void SendAlignmentSubAreasListMessage(IPacketReceiver client)
        {
            client.Send(new AlignmentSubAreasListMessage(new short[0], new short[0]));
        }
    }
}