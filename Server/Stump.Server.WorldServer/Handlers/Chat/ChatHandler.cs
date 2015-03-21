using System;
using System.Collections.Generic;
using System.Globalization;
using MongoDB.Bson;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
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
                if (client.Character.IsMuted())
                {
                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 123,
                        (int)client.Character.GetMuteRemainingTime().TotalSeconds);
                }
                else
                {
                    if (client.Character != chr)
                    {
                        if (!chr.FriendsBook.IsIgnored(client.Account.Id))
                        {
                            if (!chr.IsAway || chr.FriendsBook.IsFriend(client.Account.Id))
                            {
                                if (client.Character.IsAway)
                                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 72);

                                // send a copy to sender
                                SendChatServerCopyMessage(client, chr, chr, ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE,
                                    message.content);

                                // Send to receiver
                                SendChatServerMessage(chr.Client, client.Character,
                                    ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE,
                                    message.content);
                            }
                            else
                            {
                                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 14,
                                    chr.Name);
                            }
                        }
                        else
                        {
                            client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 14,
                                chr.Name);
                        }
                    }
                    else
                    {
                        SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_INTERIOR_MONOLOGUE);
                    }
                }
            }
            else
            {
                SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_RECEIVER_NOT_FOUND);
            }
        }
    
        [WorldHandler(ChatClientPrivateWithObjectMessage.Id)]
        public static void HandleChatpriva(WorldClient client, ChatClientPrivateWithObjectMessage message)
        {
            if (String.IsNullOrEmpty(message.content))
                return;

            var chr = World.Instance.GetCharacter(message.receiver);

            if (chr != null)
            {
                if (client.Character.IsMuted())
                {
                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 123,
                        (int)client.Character.GetMuteRemainingTime().TotalSeconds);
                }
                else
                {
                    if (client.Character != chr)
                    {
                        if (!chr.FriendsBook.IsIgnored(client.Account.Id))
                        {
                            if (!chr.IsAway || chr.FriendsBook.IsFriend(client.Account.Id))
                            {
                                if (client.Character.IsAway)
                                    client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 72);

                                // send a copy to sender
                                SendChatServerCopyWithObjectMessage(client, chr, chr, ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE,
                                    message.content, message.objects);

                                // Send to receiver
                                SendChatServerWithObjectMessage(chr.Client, client.Character,
                                    ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE,
                                    message.content, "", message.objects);
                            }
                            else
                            {
                                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 14,
                                    chr.Name);
                            }
                        }
                        else
                        {
                            client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 14,
                                chr.Name);
                        }
                    }
                    else
                    {
                        SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_INTERIOR_MONOLOGUE);
                    }
                }
            }
            else
            {
                SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_RECEIVER_NOT_FOUND);
            }
        }

        [WorldHandler(ChatClientMultiMessage.Id)]
        public static void HandleChatClientMultiMessage(WorldClient client, ChatClientMultiMessage message)
        {
            ChatManager.Instance.HandleChat(client, (ChatActivableChannelsEnum)message.channel, message.content);
        }

        [WorldHandler(ChatClientMultiWithObjectMessage.Id)]
        public static void HandleChatClientMultiWithObjectMessage(WorldClient client, ChatClientMultiWithObjectMessage message)
        {
            ChatManager.Instance.HandleChat(client, (ChatActivableChannelsEnum)message.channel, message.content, message.objects);
        }

        public static  void SendChatServerWithObjectMessage(IPacketReceiver client, INamedActor sender, ChatActivableChannelsEnum channel, string content, string fingerprint, IEnumerable<ObjectItem> objectItems)
        {
            client.Send(new ChatServerWithObjectMessage((sbyte)channel, content, DateTime.Now.GetUnixTimeStamp(), fingerprint, sender.Id, sender.Name, 0, objectItems));
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
                            (sbyte)channel,
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

            if (sender.UserGroup.Role <= RoleEnum.Moderator)
                message = message.HtmlEntities();

            client.Send(new ChatServerMessage(
                (sbyte)channel,
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
                                (sbyte)channel,
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
                                       sender.Account.Id);
        }

        public static void SendChatAdminServerMessage(IPacketReceiver client, ChatActivableChannelsEnum channel, string message,
                                                      int timestamp, string fingerprint, int senderId, string senderName,
                                                      int accountId)
        {
            if (!String.IsNullOrEmpty(message))
            {
                client.Send(new ChatAdminServerMessage((sbyte)channel,
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
            if (sender.UserGroup.Role <= RoleEnum.Moderator)
                message = message.HtmlEntities();

            client.Send(new ChatServerCopyMessage(
                            (sbyte)channel,
                            message,
                            timestamp,
                            fingerprint,
                            receiver.Id,
                            receiver.Name));
        }

        public static void SendChatServerCopyWithObjectMessage(IPacketReceiver client, Character sender, Character receiver, ChatActivableChannelsEnum channel,
                                                     string message, IEnumerable<ObjectItem> objectItems)
        {
            SendChatServerCopyWithObjectMessage(client, sender, receiver, channel, message, DateTime.Now.GetUnixTimeStamp(), "", objectItems);
        }

        public static void SendChatServerCopyWithObjectMessage(IPacketReceiver client, Character sender, Character receiver, ChatActivableChannelsEnum channel,
                                                     string message,
                                                     int timestamp, string fingerprint, IEnumerable<ObjectItem> objectItems)
        {
            if (sender.UserGroup.Role <= RoleEnum.Moderator)
                message = message.HtmlEntities();

            client.Send(new ChatServerCopyWithObjectMessage(
                            (sbyte)channel,
                            message,
                            timestamp,
                            fingerprint,
                            receiver.Id,
                            receiver.Name,
                            objectItems));
        }

        public static void SendChatErrorMessage(IPacketReceiver client, ChatErrorEnum error)
        {
            client.Send(new ChatErrorMessage((sbyte)error));
        }
    }
}