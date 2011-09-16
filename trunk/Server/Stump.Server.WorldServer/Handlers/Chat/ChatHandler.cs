using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Chats;

namespace Stump.Server.WorldServer.Handlers.Chat
{
    public partial class ChatHandler : WorldHandlerContainer
    {
        [WorldHandler(ChatClientPrivateMessage.Id)]
        public static void HandleChatClientPrivateMessage(WorldClient client, ChatClientPrivateMessage message)
        {
            Character chr = World.Instance.GetCharacter(message.receiver);

            if (chr != null)
            {
                // send a copy to sender
                SendChatServerCopyMessage(client, chr, ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, message.content);

                // Send to receiver
                SendChatServerMessage(chr.Client, client.ActiveCharacter, ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, message.content);
            }
            else
            {
                client.Send(new ChatErrorMessage((sbyte) ChatErrorEnum.CHAT_ERROR_RECEIVER_NOT_FOUND));
            }
        }

        [WorldHandler(ChatClientMultiMessage.Id)]
        public static void HandleChatClientMultiMessage(WorldClient client, ChatClientMultiMessage message)
        {
            ChatManager.Instance.HandleChat(client, (ChatActivableChannelsEnum) message.channel, message.content);
        }

        public static void SendChatServerMessage(WorldClient client, string message)
        {
            SendChatServerMessage(client, ChatActivableChannelsEnum.PSEUDO_CHANNEL_INFO, message, 0, "", 0, "", 0);
        }

        public static void SendChatServerMessage(WorldClient client, NamedActor sender, ChatActivableChannelsEnum channel, string message)
        {
            SendChatServerMessage(client, sender, channel, message, 0, "");
        }

        public static void SendChatServerMessage(WorldClient client, NamedActor sender, ChatActivableChannelsEnum channel, string message,
                                                 int timestamp, string fingerprint)
        {
            client.Send(new ChatServerMessage(
                            (sbyte) channel,
                            message,
                            timestamp,
                            fingerprint,
                            sender.Id,
                            sender.Name,
                            0));
        }

        public static void SendChatServerMessage(WorldClient client, Character sender, ChatActivableChannelsEnum channel, string message)
        {
            SendChatServerMessage(client, sender, channel, message, 0, "");
        }

        public static void SendChatServerMessage(WorldClient client, Character sender, ChatActivableChannelsEnum channel, string message,
                                                 int timestamp, string fingerprint)
        {
            if (sender.Client.Account.Role <= RoleEnum.Moderator)
                message = message.HtmlEntities();

            client.Send(new ChatServerMessage(
                            (sbyte) channel,
                            message,
                            timestamp,
                            fingerprint,
                            sender.Id,
                            sender.Name,
                            (int) sender.Client.Account.Id));
        }

        public static void SendChatServerMessage(WorldClient client, ChatActivableChannelsEnum channel, string message,
                                                 int timestamp, string fingerprint, int senderId, string senderName,
                                                 int accountId)
        {
            client.Send(new ChatServerMessage(
                            (sbyte) channel,
                            message,
                            timestamp,
                            fingerprint,
                            senderId,
                            senderName,
                            accountId));
        }

        public static void SendChatServerCopyMessage(WorldClient client, Character receiver, ChatActivableChannelsEnum channel,
                                                     string message)
        {
            SendChatServerCopyMessage(client, receiver, channel, message, 0, "");
        }

        public static void SendChatServerCopyMessage(WorldClient client, Character receiver, ChatActivableChannelsEnum channel,
                                                     string message,
                                                     int timestamp, string fingerprint)
        {
            {
                if (client.Account.Role <= RoleEnum.Moderator)
                    message = message.HtmlEntities();

                client.Send(new ChatServerCopyMessage(
                                (sbyte) channel,
                                message,
                                timestamp,
                                fingerprint,
                                receiver.Id,
                                receiver.Name));
            }
        }
    }
}