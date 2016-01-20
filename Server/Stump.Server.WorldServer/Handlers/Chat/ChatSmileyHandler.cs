using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Chat
{
    public partial class ChatHandler
    {
        [WorldHandler(ChatSmileyRequestMessage.Id)]
        public static void HandleChatSmileyRequestMessage(WorldClient client, ChatSmileyRequestMessage message)
        {
            client.Character.DisplaySmiley(message.smileyId);
        }

        public static void SendChatSmileyMessage(IPacketReceiver client, Character character, short smileyId)
        {
            client.Send(new ChatSmileyMessage(
                            character.Id,
                            smileyId,
                            character.Account.Id));
        }

        public static void SendChatSmileyMessage(IPacketReceiver client, ContextActor entity, short smileyId)
        {
            client.Send(new ChatSmileyMessage(
                            entity.Id,
                            smileyId,
                            0));
        }

        public static void SendChatSmileyExtraPackListMessage(IPacketReceiver client)
        {
            client.Send(new ChatSmileyExtraPackListMessage(new sbyte[] { 1, 2, 3, 4, 5 }));
        }
    }
}