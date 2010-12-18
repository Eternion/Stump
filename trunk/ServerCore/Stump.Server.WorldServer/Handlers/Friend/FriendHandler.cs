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
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class FriendHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(IgnoredGetListMessage))]
        public static void HandleIgnoredGetListMessage(WorldClient client, IgnoredGetListMessage message)
        {
            SendIgnoredListMessage(client, new List<IgnoredInformations>());
        }

        [WorldHandler(typeof(FriendsGetListMessage))]
        public static void HandleFriendsGetListMessage(WorldClient client, FriendsGetListMessage message)
        {
            SendFriendsListMessage(client, new List<FriendInformations>());
        }

        public static void SendFriendWarnOnLevelGainStateMessage(WorldClient client, bool enable)
        {
            client.Send(new FriendWarnOnLevelGainStateMessage(enable));
        }

        public static void SendGuildMemberWarnOnConnectionStateMessage(WorldClient client, bool enable)
        {
            client.Send(new GuildMemberWarnOnConnectionStateMessage(enable));
        }

        public static void SendFriendWarnOnConnectionStateMessage(WorldClient client, bool enable)
        {
            client.Send(new FriendWarnOnConnectionStateMessage(enable));
        }

        public static void SendIgnoredListMessage(WorldClient client, IEnumerable<IgnoredInformations> ignoreds)
        {
            client.Send(new IgnoredListMessage(ignoreds.ToList()));
        }

        public static void SendFriendsListMessage(WorldClient client, IEnumerable<FriendInformations> friends)
        {
            client.Send(new FriendsListMessage(friends.ToList()));
        }
    }
}