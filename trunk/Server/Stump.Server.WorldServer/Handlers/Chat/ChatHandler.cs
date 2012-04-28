using System;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Social;

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
                SendChatServerCopyMessage(client, chr, chr, ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, message.content);

                // Send to receiver
                SendChatServerMessage(chr.Client, client.Character, ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, message.content);
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

        public static void SendChatServerMessage(IPacketReceiver client, string message)
        {
            SendChatServerMessage(client, ChatActivableChannelsEnum.PSEUDO_CHANNEL_INFO, message, DateTime.Now.GetUnixTimeStamp(), "", 0, "", 0);
        }

        public static void SendChatServerMessage(IPacketReceiver client, INamedActor sender, ChatActivableChannelsEnum channel, string message)
        {
            SendChatServerMessage(client, sender, channel, message, DateTime.Now.GetUnixTimeStamp(), "");
        }

        public static void SendChatServerMessage(IPacketReceiver client, INamedActor sender, ChatActivableChannelsEnum channel, string message,
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

        public static void SendChatServerMessage(IPacketReceiver client, Character sender, ChatActivableChannelsEnum channel, string message)
        {
            SendChatServerMessage(client, sender, channel, message, DateTime.Now.GetUnixTimeStamp(), "");
        }

        public static void SendChatServerMessage(IPacketReceiver client, Character sender, ChatActivableChannelsEnum channel, string message,
                                                 int timestamp, string fingerprint)
        {
            if (sender.Account.Role <= RoleEnum.Moderator)
                message = message.HtmlEntities();

            client.Send(new ChatServerMessage(
                            (sbyte) channel,
                            message,
                            timestamp,
                            fingerprint,
                            sender.Id,
                            sender.Name,
                            (int) sender.Account.Id));
        }

        public static void SendChatServerMessage(IPacketReceiver client, ChatActivableChannelsEnum channel, string message,
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

        public static void SendChatAdminServerMessage(IPacketReceiver client, Character sender, ChatActivableChannelsEnum channel, string message)
        {
            SendChatAdminServerMessage(client, sender, channel, message, DateTime.Now.GetUnixTimeStamp(), "");
        }

        public static void SendChatAdminServerMessage(IPacketReceiver client, Character sender, ChatActivableChannelsEnum channel, string message,
                                                      int timestamp, string fingerprint)
        {
            SendChatAdminServerMessage(client, channel,
                                       message,
                                       timestamp,
                                       fingerprint,
                                       sender.Id,
                                       sender.Name,
                                       (int) sender.Account.Id);
        }

        public static void SendChatAdminServerMessage(IPacketReceiver client, ChatActivableChannelsEnum channel, string message,
                                                      int timestamp, string fingerprint, int senderId, string senderName,
                                                      int accountId)
        {
            client.Send(new ChatAdminServerMessage((sbyte) channel,
                                                   message,
                                                   timestamp,
                                                   fingerprint,
                                                   senderId,
                                                   senderName,
                                                   accountId));
        }

        public static void SendChatServerCopyMessage(IPacketReceiver client, Character sender, Character receiver, ChatActivableChannelsEnum channel,
                                                     string message)
        {
            SendChatServerCopyMessage(client, sender, receiver, channel, message, DateTime.Now.GetUnixTimeStamp(), "");
        }

        public static void SendChatServerCopyMessage(IPacketReceiver client, Character sender, Character receiver, ChatActivableChannelsEnum channel,
                                                     string message,
                                                     int timestamp, string fingerprint)
        {
            if (sender.Account.Role <= RoleEnum.Moderator)
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