using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class InteractiveObjectsHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (InteractiveUseRequestMessage))]
        public static void HandleInteractiveUseRequestMessage(WorldClient client, InteractiveUseRequestMessage message)
        {
            client.GuessSkillAction = new Tuple<uint, InteractiveUseRequestMessage, InteractiveUsedMessage>(client.CurrentMap, message, null);

            client.Server.Send(message);
        }

        [WorldHandler(typeof (InteractiveUsedMessage))]
        public static void HandleInteractiveUsedMessage(WorldClient client, InteractiveUsedMessage message)
        {
            if (!Equals(client.GuessSkillAction, null))
                client.GuessSkillAction = Tuple.Create(client.GuessSkillAction.Item1, client.GuessSkillAction.Item2, message);

            client.Send(message);
        }
    }
}