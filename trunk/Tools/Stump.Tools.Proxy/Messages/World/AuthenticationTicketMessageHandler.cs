using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;
using System.Net;

namespace Stump.Tools.Proxy.Messages
{
    class AuthenticationTicketMessageHandler
    {


        [Handler(typeof(AuthenticationTicketMessage))]
        public static void HandleAuthenticationTicketMessage(AuthenticationTicketMessage message, DerivedConnexion sender)
        {
             if (WorldDerivedConnexion.Tickets.ContainsKey(message.ticket))
             {
                 SelectedServerDataMessage mess = WorldDerivedConnexion.Tickets[message.ticket];
                 WorldDerivedConnexion.Tickets.Remove(message.ticket);
                 (sender as WorldDerivedConnexion).Ticket = mess.ticket;
                 sender.BindToServer(new IPEndPoint(IPAddress.Parse(mess.address), (int)mess.port));
             }
             else
                 sender.Client.Disconnect();
        }

    }
}
