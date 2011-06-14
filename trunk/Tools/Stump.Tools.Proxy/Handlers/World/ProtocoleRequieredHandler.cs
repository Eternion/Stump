
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class ProtocolRequiredMessageHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(ProtocolRequired))]
        public static void ProtocolRequiredMessage(ProxyClient client, ProtocolRequired message)
        {
            if (!(client is WorldClient))
                client.Send(message);
        }
    }
}