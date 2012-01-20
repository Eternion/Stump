using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class InteractiveObjectsHandler : WorldHandlerContainer
    {
        [WorldHandler(InteractiveUseRequestMessage.Id)]
        public static void HandleInteractiveUseRequestMessage(WorldClient client, InteractiveUseRequestMessage message)
        {
            client.Server.Send(message);

            client.GuessSkillAction = new Tuple<Map, InteractiveUseRequestMessage, InteractiveUsedMessage>(client.CurrentMap, message, null);
        }

        [WorldHandler(InteractiveUsedMessage.Id)]
        public static void HandleInteractiveUsedMessage(WorldClient client, InteractiveUsedMessage message)
        {
            client.Send(message);

            if (message.entityId != client.CharacterInformations.id)
                return;

            if (!Equals(client.GuessSkillAction, null))
                client.GuessSkillAction = Tuple.Create(client.GuessSkillAction.Item1, client.GuessSkillAction.Item2, message);
        }
    }
}