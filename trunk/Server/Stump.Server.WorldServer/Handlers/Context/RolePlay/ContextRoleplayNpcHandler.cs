using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        [WorldHandler(NpcGenericActionRequestMessage.Id)]
        public static void HandleNpcGenericActionRequestMessage(WorldClient client,
                                                                NpcGenericActionRequestMessage message)
        {
            var npc = client.ActiveCharacter.Map.GetActor<Npc>(message.npcId);

            if (npc == null)
                return;

            npc.InteractWith((NpcActionTypeEnum) message.npcActionId, client.ActiveCharacter);
        }

        [WorldHandler(NpcDialogReplyMessage.Id)]
        public static void HandleNpcDialogReplyMessage(WorldClient client, NpcDialogReplyMessage message)
        {
            client.ActiveCharacter.ReplyToNpc(message.replyId);
        }

        public static void SendNpcDialogCreationMessage(WorldClient client, Npc npc)
        {
            client.Send(new NpcDialogCreationMessage(npc.Position.Map.Id, npc.Id));
        }

        public static void SendNpcDialogQuestionMessage(WorldClient client, NpcMessage message)
        {
            client.Send(new NpcDialogQuestionMessage((short) message.Id, message.Parameters,
                                                     message.Replies.Select(entry => (short) entry.Id)));
        }
    }
}