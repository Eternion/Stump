using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextHandler
    {
        /*[WorldHandler(typeof (NpcGenericActionRequestMessage))]
        public static void HandleNpcGenericActionRequestMessage(WorldClient client,
                                                                NpcGenericActionRequestMessage message)
        {
            var npc = client.ActiveCharacter.Map.Get<NpcSpawn>(message.npcId);

            if (npc == null)
                return;

            npc.Interact((NpcActionTypeEnum) message.npcActionId, client.ActiveCharacter);
        }

        [WorldHandler(typeof (NpcDialogReplyMessage))]
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
            client.Send(new NpcDialogQuestionMessage(question.Id, question.Parameters.ToList(),
                                                     question.Replies.Keys.ToList()));
        }*/
    }
}