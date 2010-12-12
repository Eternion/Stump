using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;
using System.Net;

namespace Stump.Tools.Proxy.Messages
{
    class ProtocolRequiredMessageHandler
    {


        [Handler(typeof(ProtocolRequired))]
        public static void ProtocolRequiredMessage(ProtocolRequired message, DerivedConnexion sender)
        {
            if (sender is WorldDerivedConnexion)
            {           
                Console.WriteLine("Intercept ProtocolRequired");
            }
            else
                sender.Client.Send(message);
        }

    }
}
