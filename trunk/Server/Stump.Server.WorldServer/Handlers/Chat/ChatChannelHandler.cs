using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Chat
{
    public partial class ChatHandler : WorldHandlerContainer
    {
        [WorldHandler(ChannelEnablingMessage.Id)]
        public static void HandleChannelEnablingMessage(WorldClient client, ChannelEnablingMessage message)
        {
        }

        public static void SendEnabledChannelsMessage(IPacketReceiver client, sbyte[] allows, sbyte[] disallows)
        {
            client.Send(new EnabledChannelsMessage(allows, disallows));
        }
    }
}