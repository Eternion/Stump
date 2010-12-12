using System.Collections.Generic;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {
        [WorldHandler(typeof(QuestListRequestMessage))]
        public static void HandleQuestListRequestMessage(WorldClient client, QuestListRequestMessage message)
        {
            SendQuestListMessage(client);
        }

        public static void SendQuestListMessage(WorldClient client)
        {
            client.Send(new QuestListMessage(new List<uint>(), new List<uint>()));
        }
    }
}