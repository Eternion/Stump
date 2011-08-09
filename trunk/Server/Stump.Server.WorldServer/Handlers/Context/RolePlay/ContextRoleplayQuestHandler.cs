using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextHandler
    {
        [WorldHandler(QuestListRequestMessage.Id)]
        public static void HandleQuestListRequestMessage(WorldClient client, QuestListRequestMessage message)
        {
            SendQuestListMessage(client);
        }

        public static void SendQuestListMessage(WorldClient client)
        {
            client.Send(new QuestListMessage(new List<short>(), new List<short>()));
        }
    }
}