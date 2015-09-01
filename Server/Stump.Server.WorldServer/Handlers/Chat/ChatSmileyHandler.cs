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
            client.Character.DisplaySmiley((sbyte)message.smileyId);
        }

        public static void SendChatSmileyMessage(IPacketReceiver client, Character character, sbyte smileyId)
        {
            client.Send(new ChatSmileyMessage(
                            character.Id,
                            smileyId,
                            character.Account.Id));
        }

        public static void SendChatSmileyMessage(IPacketReceiver client, ContextActor entity, sbyte smileyId)
        {
            client.Send(new ChatSmileyMessage(
                            entity.Id,
                            smileyId,
                            0));
        }
    }
}