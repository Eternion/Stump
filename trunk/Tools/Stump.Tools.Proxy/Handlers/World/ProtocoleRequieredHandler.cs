
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class ProtocolRequiredMessageHandler : ProxyHandlerContainer
    {
        [WorldHandler(ProtocolRequired.Id)]
        public static void ProtocolRequiredMessage(ProxyClient client, ProtocolRequired message)
        {
            if (!(client is WorldClient))
                client.Send(message);
        }
    }
}