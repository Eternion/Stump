
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class ChatClientMultiMessageHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (ChatClientMultiMessage))]
        public static void ChatAbstractClientMessage(WorldClient client, ChatClientMultiMessage message)
        {
            // todo : process command

            client.Server.Send(message);
        }
    }
}