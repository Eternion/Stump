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
        static void HandleSelectedServerDataMessage(SelectedServerDataMessage message, DerivedConnexion sender)
        {
             WorldDerivedConnexion.Tickets.Add(message.ticket, message);

             message.address = ClientListener.Host;

             message.port = (uint)ClientListener.Port;

             sender.Client.Send(message);
        }

    }
}
