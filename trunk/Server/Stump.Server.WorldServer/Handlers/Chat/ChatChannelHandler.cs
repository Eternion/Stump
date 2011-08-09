using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Chat
{
    public partial class ChatHandler : WorldHandlerContainer
    {
        [WorldHandler(ChannelEnablingMessage.Id)]
        public static void HandleChannelEnablingMessage(WorldClient client, ChannelEnablingMessage message)
        {
        }

        public static void SendEnabledChannelsMessage(WorldClient client, List<byte> allows, List<byte> disallows)
        {
            client.Send(new EnabledChannelsMessage(allows, disallows));
        }
    }
}