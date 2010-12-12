using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Proxy.Messages
{
    static class MapComplementaryInformationsDataMessageHandler
    {

        [Handler(typeof(MapComplementaryInformationsDataMessage))]
        public static void HandleMapComplementaryInformationsDataMessage(MapComplementaryInformationsDataMessage message, DerivedConnexion sender)
        {
          


            sender.Client.Send(message);
        }

    }
}
