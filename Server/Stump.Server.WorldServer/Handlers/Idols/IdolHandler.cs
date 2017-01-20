using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Handlers.Idols
{
    public class IdolHandler : WorldHandlerContainer
    {
        [WorldHandler(IdolPartyRegisterRequestMessage.Id)]
        public static void HandleIdolPartyRegisterRequestMessage(WorldClient client, IdolPartyRegisterRequestMessage message)
        {
            if (message.register)
                SendIdolListMessage(client);
        }

        [WorldHandler(IdolSelectRequestMessage.Id)]
        public static void HandleIdolSelectRequestMessage(WorldClient client, IdolSelectRequestMessage message)
        {
            if (message.activate)
                client.Character.IdolInventory.Add(message.idolId);
            else
                client.Character.IdolInventory.Remove(message.idolId);
        }

        public static void SendIdolListMessage(WorldClient client)
        {
            client.Send(new IdolListMessage(client.Character.IdolInventory.GetIdols().Select(x => (short)x.Id), new short[0], new PartyIdol[0]));
        }

        public static void SendIdolSelectedMessage(WorldClient client, bool activate, bool party, short idolId)
        {
            client.Send(new IdolSelectedMessage(activate, party, idolId));
        }

        public static void SendIdolSelectErrorMessage(WorldClient client, bool activate, bool party, short idolId, sbyte reason)
        {
            client.Send(new IdolSelectErrorMessage(activate, party, reason, idolId));
        }

        public static void SendIdolFightPreparationUpdate(IPacketReceiver client, IEnumerable<Idol> idols)
        {
            client.Send(new IdolFightPreparationUpdateMessage(0, idols));
        }
    }
}
