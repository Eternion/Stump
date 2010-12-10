using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Proxy.Messages
{
    class ChatAbstractClientMessageHandler
    {
        /* In order to process player commands */
        [Handler(typeof(ChatAbstractClientMessage))]
        static void ChatAbstractClientMessage(ChatAbstractClientMessage message, DerivedConnexion sender)
        {
            if ((sender as WorldDerivedConnexion).Infos != null)
            {
                Commands.ProcessClientCommand(message.content.Substring(1).Split(' '), sender);
            }
        }

    }
}
