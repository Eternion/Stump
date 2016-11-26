using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Idols
{
    public class IdolHandler : WorldHandlerContainer
    {
        [WorldHandler(IdolPartyRegisterRequestMessage.Id)]
        public static void HandleIdolPartyRegisterRequestMessage(WorldClient client, IdolPartyRegisterRequestMessage message)
        {
            SendIdolListMessage(client);
        }

        public static void SendIdolListMessage(WorldClient client)
        {
            client.Send(new IdolListMessage(new short[0], new short[0], new PartyIdol[0]));
        }
    }
}
