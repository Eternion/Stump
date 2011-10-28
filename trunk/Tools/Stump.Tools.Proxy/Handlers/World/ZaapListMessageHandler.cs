
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class ZaapListMessageHandler : WorldHandlerContainer
    {
        [WorldHandler(ZaapListMessage.Id)]
        public static void HandleZaapListMessage(WorldClient client, ZaapListMessage message)
        {
            client.Send(message);
        }
    }
}