// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Chat;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ChatHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(ChatClientPrivateMessage))]
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
                client.Send(new ChatErrorMessage((uint)ChatErrorEnum.CHAT_ERROR_RECEIVER_NOT_FOUND));
            }
        }

        [WorldHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatClientMultiMessage(WorldClient client, ChatClientMultiMessage message)
        {
            if (ChatManager.ChatHandlers.Length <= (int)message.channel)
                return;

            ChatManager.ChatParserDelegate handler = ChatManager.ChatHandlers[message.channel];

            if (handler != null)
            {
                handler(client, (ChannelId)message.channel, message.content);
            }
        }

        public static void SendChatServerMessage(WorldClient client, string message)
        {
            SendChatServerMessage(client, ChannelId.Information, message, 0, "", 0, "Server", 0);
        }

        public static void SendChatServerMessage(WorldClient client, Entity sender, ChannelId channel, string message)
        {
            SendChatServerMessage(client, sender, channel, message, 0, "");
        }

        public static void SendChatServerMessage(WorldClient client, Entity sender, ChannelId channel, string message,
                                         int timestamp, string fingerprint)
        {
            client.Send(new ChatServerMessage(
                            (uint)channel,
                            message,
                            (uint)timestamp,
                            fingerprint,
                            (int)sender.Id,
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
                message = StringUtils.HtmlEntities(message);

            client.Send(new ChatServerMessage(
                            (uint)channel,
                            message,
                            (uint)timestamp,
                            fingerprint,
                            (int)sender.Id,
                            sender.Name,
                            (int)sender.Client.Account.Id));
        }

        public static void SendChatServerMessage(WorldClient client, ChannelId channel, string message,
                                         int timestamp, string fingerprint, int senderId, string senderName, int accountId)
        {
            client.Send(new ChatServerMessage(
                            (uint)channel,
                            message,
                            (uint)timestamp,
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
                    message = StringUtils.HtmlEntities(message);

                client.Send(new ChatServerCopyMessage(
                                (uint)channel,
                                message,
                                (uint)timestamp,
                                fingerprint,
                                (uint)receiver.Id,
                                receiver.Name));
            }
        }
    }
}