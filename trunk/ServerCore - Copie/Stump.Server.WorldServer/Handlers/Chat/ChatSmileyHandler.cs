
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ChatHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (ChatSmileyRequestMessage))]
        public static void HandleChatSmileyRequestMessage(WorldClient client, ChatSmileyRequestMessage message)
        {
            client.ActiveCharacter.DisplaySmiley((byte) message.smileyId);
        }

        public static void SendChatSmileyMessage(WorldClient client, Character character, uint smileyId)
        {
            client.Send(new ChatSmileyMessage(
                            (int) character.Id,
                            smileyId,
                            character.Client.Account.Id));
        }

        public static void SendChatSmileyMessage(WorldClient client, Entity entity, uint smileyId)
        {
            client.Send(new ChatSmileyMessage(
                            (int) entity.Id,
                            smileyId,
                            0));
        }
    }
}