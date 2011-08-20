using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Chat;

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
                SendChatServerCopyMessage(client, chr, ChannelId.Private, message.content);

                // Send to receiver
                SendChatServerMessage(chr.Client, client.ActiveCharacter, ChannelId.Private, message.content);
            }
            else
            {
                client.Send(new ChatErrorMessage((byte) ChatErrorEnum.CHAT_ERROR_RECEIVER_NOT_FOUND));
            }
        }

        [WorldHandler(ChatClientMultiMessage.Id)]
        public static void HandleChatClientMultiMessage(WorldClient client, ChatClientMultiMessage message)
        {
            var handler = ChatManager.Instance.ChatHandlers[message.channel];

            if (handler != null)
            {
                handler(client, (ChannelId) message.channel, message.content);
            }
        }

        public static void SendChatServerMessage(WorldClient client, string message)
        {
            SendChatServerMessage(client, ChannelId.Information, message, 0, "", 0, "Server", 0);
        }

        public static void SendChatServerMessage(WorldClient client, NamedActor sender, ChannelId channel, string message)
        {
            SendChatServerMessage(client, sender, channel, message, 0, "");
        }

        public static void SendChatServerMessage(WorldClient client, NamedActor sender, ChannelId channel, string message,
                                                 int timestamp, string fingerprint)
        {
            client.Send(new ChatServerMessage(
                            (byte) channel,
                            message,
                            timestamp,
                            fingerprint,
                            sender.Id,
                            sender.Name,
                            0));
        }

        public static void SendChatServerMessage(WorldClient client, Character sender, ChannelId channel, string message)
        {
            SendChatServerMessage(client, sender, channel, message, 0, "");
        }

        public static void SendChatServerMessage(WorldClient client, Character sender, ChannelId channel, string message,
                                                 int timestamp, string fingerprint)
        {
            if (sender.Client.Account.Role <= RoleEnum.Moderator)
                message = message.HtmlEntities();

            client.Send(new ChatServerMessage(
                            (byte) channel,
                            message,
                            timestamp,
                            fingerprint,
                            sender.Id,
                            sender.Name,
                            (int) sender.Client.Account.Id));
        }

        public static void SendChatServerMessage(WorldClient client, ChannelId channel, string message,
                                                 int timestamp, string fingerprint, int senderId, string senderName,
                                                 int accountId)
        {
            client.Send(new ChatServerMessage(
                            (byte) channel,
                            message,
                            timestamp,
                            fingerprint,
                            senderId,
                            senderName,
                            accountId));
        }

        public static void SendChatServerCopyMessage(WorldClient client, Character receiver, ChannelId channel,
                                                     string message)
        {
            SendChatServerCopyMessage(client, receiver, channel, message, 0, "");
        }

        public static void SendChatServerCopyMessage(WorldClient client, Character receiver, ChannelId channel,
                                                     string message,
                                                     int timestamp, string fingerprint)
        {
            {
                if (client.Account.Role <= RoleEnum.Moderator)
                    message = message.HtmlEntities();

                client.Send(new ChatServerCopyMessage(
                                (byte) channel,
                                message,
                                timestamp,
                                fingerprint,
                                receiver.Id,
                                receiver.Name));
            }
        }
    }
}