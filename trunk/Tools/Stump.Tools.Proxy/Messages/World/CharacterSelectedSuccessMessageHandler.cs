using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Proxy.Messages
{
    class CharacterSelectedSuccessMessageHandler
    {

        /* In order to save select player infos */
        [Handler(typeof(CharacterSelectedSuccessMessage))]
        public static void ChatAbstractClientMessage(CharacterSelectedSuccessMessage message, DerivedConnexion sender)
        {
             (sender as WorldDerivedConnexion).Infos = message.infos;

             sender.Client.Send(message);
        }

    }
}
