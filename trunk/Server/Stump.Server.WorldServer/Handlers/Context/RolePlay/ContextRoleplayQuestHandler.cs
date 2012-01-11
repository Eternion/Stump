using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        [WorldHandler(QuestListRequestMessage.Id)]
        public static void HandleQuestListRequestMessage(WorldClient client, QuestListRequestMessage message)
        {
            SendQuestListMessage(client);
        }

        public static void SendQuestListMessage(IPacketReceiver client)
        {
            client.Send(new QuestListMessage(new short[0], new short[0], new QuestActiveInformations[0]));
        }
    }
}