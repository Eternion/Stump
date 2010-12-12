using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;
using System.Net;

namespace Stump.Tools.Proxy.Messages
{
    class ZaapListMessageHandler
    {


        [Handler(typeof(ZaapListMessage))]
        public static void HandleZaapListMessage(ZaapListMessage message, DerivedConnexion sender)
        {
         


             sender.Client.Send(message);
        }

    }
}
