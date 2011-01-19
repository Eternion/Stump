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
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class FriendHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(IgnoredGetListMessage))]
        public static void HandleIgnoredGetListMessage(WorldClient client, IgnoredGetListMessage message)
        {
            //  var ignoreds = client.WorldAccount.Ignored.Select(i => new IgnoredInformations(i.Nickname, i.Id));

            //SendIgnoredListMessage(client, ignoreds);
        }

        [WorldHandler(typeof(FriendsGetListMessage))]
        public static void HandleFriendsGetListMessage(WorldClient client, FriendsGetListMessage message)
        {
            var friends = client.WorldAccount.Friends.Select(f =>

                new FriendInformations(f.Nickname, (uint)PlayerStateEnum.NOT_CONNECTED, f.LastConnectionTimeStamp));

            SendFriendsListMessage(client, friends);
        }


        [WorldHandler(typeof(FriendSetWarnOnConnectionMessage))]
        public static void HandleFriendSetWarnOnConnectionMessage(WorldClient client, FriendSetWarnOnConnectionMessage message)
        {
            client.WarnOnFriendConnection = message.enable;
        }

        [WorldHandler(typeof(FriendSetWarnOnLevelGainMessage))]
        public static void HandleFriendSetWarnOnLevelGainMessage(WorldClient client, FriendSetWarnOnLevelGainMessage message)
        {
            client.WarnOnFriendLevelGain = message.enable;
        }

        [WorldHandler(typeof(FriendAddRequestMessage))]
        public static void HandleFriendAddRequestMessage(WorldClient client, FriendAddRequestMessage message)
        {
            if (message.name == client.ActiveCharacter.Name)
            {
                client.Send(new FriendAddFailureMessage((uint)ListAddFailureEnum.LIST_ADD_FAILURE_EGOCENTRIC));
                return;
            }

            var account = WorldServer.Instance.GetCharacters().FirstOrDefault(c => c.ActiveCharacter.Name == message.name);

            if (account == null || account.WorldAccount == null)
            {
                client.Send(new FriendAddFailureMessage((uint)ListAddFailureEnum.LIST_ADD_FAILURE_NOT_FOUND));
                return;
            }

            if (client.WorldAccount.Friends.Contains(account.WorldAccount))
            {
                client.Send(new FriendAddFailureMessage((uint)ListAddFailureEnum.LIST_ADD_FAILURE_IS_DOUBLE));
                return;
            }

            if (client.WorldAccount.Friends.Count >= 100)
            {
                client.Send(new FriendAddFailureMessage((uint)ListAddFailureEnum.LIST_ADD_FAILURE_OVER_QUOTA));
                return;
            }

            client.WorldAccount.Friends.Add(account.WorldAccount);
            client.Send(
                new FriendAddedMessage(new FriendInformations(account.WorldAccount.Nickname, (uint)PlayerStateEnum.GAME_TYPE_FIGHT, account.WorldAccount.LastConnectionTimeStamp)));
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