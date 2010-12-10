using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Proxy.Messages
{
    class IdentificationSuccessMessageHandler
    {

        
        /* In order to set player Admin and give him console access*/
        [Handler(typeof(IdentificationSuccessMessage))]
        static void HandleIdentificationSuccessMessage(IdentificationSuccessMessage message, DerivedConnexion sender)
        {

            message.hasRights = true;

             sender.Client.Send(message);
        }

    }
}
