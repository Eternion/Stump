using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Npcs;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {
        [WorldHandler(typeof(NpcGenericActionRequestMessage))]
        public static void HandleNpcGenericActionRequestMessage(WorldClient client, NpcGenericActionRequestMessage message)
        {
            var npc = client.ActiveCharacter.Map.Get<NpcSpawn>(message.npcId);

            if (npc == null)
                return;
            
            npc.StartDialog((NpcActionTypeEnum) message.npcActionId, client.ActiveCharacter);
        }

        [WorldHandler(typeof(NpcDialogReplyMessage))]
        public static void HandleNpcDialogReplyMessage(WorldClient client, NpcDialogReplyMessage message)
        {
            ((NpcDialog) client.ActiveCharacter.Dialog).Reply(message.replyId);
        }

        public static void SendNpcDialogCreationMessage(WorldClient client, NpcSpawn npc)
        {
            client.Send(new NpcDialogCreationMessage(npc.Map.Id, npc.ContextualId));
        }

        public static void SendNpcDialogQuestionMessage(WorldClient client, NpcDialogQuestion question)
        {
            client.Send(new NpcDialogQuestionMessage(question.Id, question.Parameters.ToList(), question.Replies.Keys.ToList()));
        }
    }
}