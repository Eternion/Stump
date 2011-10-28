
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class CharacterSelectedSuccessMessageHandler : WorldHandlerContainer
    {
        [WorldHandler(CharacterSelectedSuccessMessage.Id)]
        public static void ChatAbstractClientMessage(WorldClient client, CharacterSelectedSuccessMessage message)
        {
            client.CharacterInformations = message.infos;

            client.Send(message);
        }
    }
}