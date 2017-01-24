using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Idols;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Handlers.Idols
{
    public class IdolHandler : WorldHandlerContainer
    {
        [WorldHandler(IdolPartyRegisterRequestMessage.Id)]
        public static void HandleIdolPartyRegisterRequestMessage(WorldClient client, IdolPartyRegisterRequestMessage message)
        {
            var partyChosenIdols = new PlayerIdol[0];
            var partyIdols = new PartyIdol[0];

            if (client.Character.IsInParty(PartyTypeEnum.PARTY_TYPE_CLASSICAL))
            {
                partyIdols = client.Character.Party.IdolInventory.GetPartyIdols().ToArray();
                partyChosenIdols = client.Character.Party.IdolInventory.GetIdols().ToArray();
            }

            if (message.register)
                SendIdolListMessage(client, client.Character.IdolInventory.GetIdols(), partyChosenIdols, partyIdols);
        }

        [WorldHandler(IdolSelectRequestMessage.Id)]
        public static void HandleIdolSelectRequestMessage(WorldClient client, IdolSelectRequestMessage message)
        {
            if (message.party && client.Character.IsPartyLeader())
            {
                if (message.activate)
                    client.Character.Party.IdolInventory.Add(message.idolId);
                else
                    client.Character.Party.IdolInventory.Remove(message.idolId);
            }
            else
            {
                if (message.activate)
                    client.Character.IdolInventory.Add(message.idolId);
                else
                    client.Character.IdolInventory.Remove(message.idolId);
            }
        }

        public static void SendIdolListMessage(WorldClient client, IEnumerable<PlayerIdol> chosenIdols, IEnumerable<PlayerIdol> partyChosenIdols, IEnumerable<PartyIdol> partyIdols)
        {
            client.Send(new IdolListMessage(chosenIdols.Select(x => (short)x.Id), partyChosenIdols.Select(x => (short)x.Id), partyIdols));
        }

        public static void SendIdolSelectedMessage(IPacketReceiver client, bool activate, bool party, short idolId)
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

        public static void SendIdolPartyLostMessage(IPacketReceiver client, short idolId)
        {
            client.Send(new IdolPartyLostMessage(idolId));
        }

        public static void SendIdolPartyRefreshMessage(WorldClient client, PartyIdol idol)
        {
            client.Send(new IdolPartyRefreshMessage(idol));
        }
    }
}
