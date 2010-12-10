using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Messages;
using System.Net;

namespace Stump.Tools.Proxy.Messages
{
    class NpcDialogQuestionMessageHandler
    {


        [Handler(typeof(NpcDialogQuestionMessage))]
        static void HandleNpcDialogQuestionMessage(NpcDialogQuestionMessage message, DerivedConnexion sender)
        {
            var lMessage = (sender.LastClientMessage as NpcDialogCreationMessage);

            if (lMessage != null)
            {
                
            }

             sender.Client.Send(message);
        }

    }
}
