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
using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Chat;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ChatHandler : WorldHandlerContainer
    {
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
                            (uint) channel,
                            message,
                            (uint) timestamp,
                            fingerprint,
                            (int) sender.Id,
                            sender.Name,
                            (int) sender.Client.Account.Id));
        }
    }
}