using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;
using System.Net;

namespace Stump.Tools.Proxy.Messages
{
    class CurrentMapMessageHandler
    {


        [Handler(typeof(CurrentMapMessage))]
        static void HandleCurrentMapMessage(CurrentMapMessage message, DerivedConnexion sender)
        {
            (sender as WorldDerivedConnexion).MapId = message.mapId;

             sender.Client.Send(message);
        }

    }
}
