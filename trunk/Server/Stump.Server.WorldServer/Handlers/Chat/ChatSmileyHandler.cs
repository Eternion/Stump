using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Chat
{
    public partial class ChatHandler : WorldHandlerContainer
    {
        [WorldHandler(ChatSmileyRequestMessage.Id)]
        public static void HandleChatSmileyRequestMessage(WorldClient client, ChatSmileyRequestMessage message)
        {
            client.ActiveCharacter.DisplaySmiley(message.smileyId);
        }

        public static void SendChatSmileyMessage(WorldClient client, Character character, byte smileyId)
        {
            client.Send(new ChatSmileyMessage(
                            character.Id,
                            smileyId,
                            (int) character.Client.Account.Id));
        }

        public static void SendChatSmileyMessage(WorldClient client, ContextActor entity, byte smileyId)
        {
            client.Send(new ChatSmileyMessage(
                            entity.Id,
                            smileyId,
                            0));
        }
    }
}