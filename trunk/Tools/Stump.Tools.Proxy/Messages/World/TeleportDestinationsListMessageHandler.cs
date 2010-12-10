using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;
using System.Net;

namespace Stump.Tools.Proxy.Messages
{
    class TeleportDestinationsListMessageHandler
    {


        [Handler(typeof(TeleportDestinationsListMessage))]
        static void HandleTeleportDestinationsListMessage(TeleportDestinationsListMessage message, DerivedConnexion sender)
        {
            var lMessage = sender.LastClientMessage as TeleportRequestMessage;

            if (lMessage != null)
            {
          

            }

             sender.Client.Send(message);
        }

    }
}
