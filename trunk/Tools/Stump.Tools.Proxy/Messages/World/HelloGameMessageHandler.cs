using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;
using System.Net;

namespace Stump.Tools.Proxy.Messages
{
    class HelloGameMessageHandler
    {


        [Handler(typeof(HelloGameMessage))]
        public static void HandleHelloGameMessage(HelloGameMessage message, DerivedConnexion sender)
        {

            sender.Server.Send(new AuthenticationTicketMessage("fr",(sender as WorldDerivedConnexion).Ticket));

        }

    }
}
