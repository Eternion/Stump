
using System.Collections.Generic;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class PvpHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (SetEnablePVPRequestMessage))]
        public static void HandleSetEnablePvpRequestMessage(WorldClient client, SetEnablePVPRequestMessage message)
        {
        }

        [WorldHandler(typeof (GetPVPActivationCostMessage))]
        public static void HandleGetPvpActivationCostMessage(WorldClient client, GetPVPActivationCostMessage message)
        {

        }

        public static void SendAlignmentSubAreasListMessage(WorldClient client)
        {
            client.Send(new AlignmentSubAreasListMessage(new List<int>(), new List<int>()));
        }

        public static void SendAlignmentAreaUpdateMessage(WorldClient client)
        {
            client.Send(new AlignmentAreaUpdateMessage());
        }

        public static void SendAlignmentSubAreaUpdateMessage(WorldClient client)
        {
            client.Send(new AlignmentSubAreaUpdateMessage());
        }

        public static void SendAlignmentRankUpdateMessage(WorldClient client)
        {
            client.Send(new AlignmentRankUpdateMessage(0, false));
        }
    }
}