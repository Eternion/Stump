
using System.Collections.Generic;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ChatHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (ChannelEnablingMessage))]
        public static void HandleChannelEnablingMessage(WorldClient client, ChannelEnablingMessage message)
        {
        }

        public static void SendEnabledChannelsMessage(WorldClient client, List<uint> allows, List<uint> disallows)
        {
            client.Send(new EnabledChannelsMessage(allows, disallows));
        }
    }
}