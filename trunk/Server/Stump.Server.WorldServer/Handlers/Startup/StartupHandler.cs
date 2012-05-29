using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Startup;
using Stump.Server.WorldServer.Game.Accounts.Startup;

namespace Stump.Server.WorldServer.Handlers.Startup
{
    public class StartupHandler : WorldHandlerContainer
    {
        [WorldHandler(StartupActionsObjetAttributionMessage.Id, RequiresLogin = false, IsGamePacket = false)]
        public static void HandleStartupActionsObjetAttributionMessage(WorldClient client, StartupActionsObjetAttributionMessage message)
        {
            if (client.WorldAccount == null || client.StartupActions == null)
                return;

            var action = client.StartupActions.FirstOrDefault(entry => entry.Id == message.actionId);

            if (action == null)
                return;

            var character = client.Characters.FirstOrDefault(entry => entry.Id == message.characterId);

            if (character == null)
                return;

            action.GiveGiftTo(character);

            client.StartupActions.Remove(action);
            client.WorldAccount.StartupActions.Remove(action.Record);
            client.WorldAccount.Update();

            SendStartupActionFinishedMessage(client, action, true);
        }

        public static void SendStartupActionsListMessage(IPacketReceiver client, IEnumerable<StartupAction> actions)
        {
            client.Send(new StartupActionsListMessage(actions.Select(entry => entry.GetStartupActionAddObject())));
        }

        public static void SendStartupActionFinishedMessage(IPacketReceiver client, StartupAction action, bool success)
        {
            client.Send(new StartupActionFinishedMessage(success, true, action.Id));
        }
    }
}