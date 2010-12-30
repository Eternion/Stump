using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Proxy.Messages
{
    class AccountCapabilitiesMessageHandler
    {

        /* Intercept and modify adress & port of the server */
        [Handler(typeof(AccountCapabilitiesMessage))]
        public static void HandleAccountCapabilitiesMessage(AccountCapabilitiesMessage message, DerivedConnexion sender)
        {
             

             var mess = new AccountCapabilitiesMessage();
             mess.accountId = message.accountId;
            mess.breedsAvailable = 8192;
            mess.breedsVisible = 8192;

             sender.Client.Send(mess);
        }

    }
}
