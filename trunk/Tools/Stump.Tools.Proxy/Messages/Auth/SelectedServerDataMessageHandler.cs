using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Proxy.Messages
{
    class SelectedServerDataMessageHandler
    {

        /* Intercept and modify adress & port of the server */
        [Handler(typeof(SelectedServerDataMessage))]
        public static void HandleSelectedServerDataMessage(SelectedServerDataMessage message, DerivedConnexion sender)
        {
             WorldDerivedConnexion.Tickets.Add(message.ticket, message);

             var mess = new SelectedServerDataMessage();

             mess.canCreateNewCharacter = true;

             mess.serverId = message.serverId;

             mess.ticket = message.ticket;

             mess.address = Proxy.worldClientListener.Host;

             mess.port = (uint)Proxy.worldClientListener.Port;

             sender.Client.Send(mess);

             sender.Client.Disconnect();
        }

    }
}
