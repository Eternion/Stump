using System;
using System.Globalization;
using MongoDB.Bson;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Logging;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Social;

namespace Stump.Server.WorldServer.Handlers.Chat
{
    public partial class ChatHandler
    {
        [WorldHandler(ChatClientPrivateMessage.Id)]
        public static void HandleChatClientPrivateMessage(WorldClient client, ChatClientPrivateMessage message)
        {
            if (String.IsNullOrEmpty(message.content))
                return;

            var chr = World.Instance.GetCharacter(message.receiver);

            if (chr != null)
            {
                if (!chr.IsAway)
                {
                    // send a copy to sender
                    SendChatServerCopyMessage(client, chr, chr, ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE,
                        message.content);

                    // Send to receiver
                    SendChatServerMessage(chr.Client, client.Character,
                        ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE,
                        message.content);

                    var document = new BsonDocument
                    {
                        { "SenderId", client.Character.Id },
                        { "ReceiverId", chr.Id },
                        { "Message", message.content },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

                    MongoLogger.Instance.Insert("PrivateMSG", document);
                }
                else
                {
                    client.Send(new ChatErrorMessage((sbyte)ChatErrorEnum.CHAT_ERROR_RECEIVER_NOT_FOUND));
                }
            }
            else
            {
                client.Send(new ChatErrorMessage((sbyte)ChatErrorEnum.CHAT_ERROR_RECEIVER_NOT_FOUND));
            }
        }

        [WorldHandler(ChatClientMultiMessage.Id)]
        public static void HandleChatClientMultiMessage(WorldClient client, ChatClientMultiMessage message)
        {
            var document = new BsonDocument
                    {
                        { "SenderId", client.Character.Id },
                        { "Message", message.content },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

            MongoLogger.Instance.Insert("MultiMessage", document);

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

        public static void SendChatServerMessage(IPacketReceiver client, Character sender, ChatActivableChannelsEnum channel, string message, int timestamp, string fingerprint)
        {
            if (String.IsNullOrEmpty(message))
                return;

            if (sender.Account.Role <= RoleEnum.Moderator)
                message = message.HtmlEntities();

            client.Send(new ChatServerMessage(
                (sbyte) channel,
                message,
                timestamp,
                fingerprint,
                sender.Id,
                sender.Name,
                sender.Account.Id));
        }

        public static void SendChatServerMessage(IPacketReceiver client, ChatActivableChannelsEnum channel, string message, int timestamp, string fingerprint, int senderId, string senderName, int accountId)
        {
            if (!String.IsNullOrEmpty(message))
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
            if (!String.IsNullOrEmpty(message))
            {
                client.Send(new ChatAdminServerMessage((sbyte) channel,
                                                       message,
                                                       timestamp,
                                                       fingerprint,
                                                       senderId,
                                                       senderName,
                                                       accountId));
            }
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