using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Friends
{
    public class FriendHandler : WorldHandlerContainer
    {
        [WorldHandler(FriendsGetListMessage.Id)]
        public static void HandleFriendsGetListMessage(WorldClient client, FriendsGetListMessage message)
        {
            SendFriendsListMessage(client, new FriendInformations[0]);
        }

        [WorldHandler(IgnoredGetListMessage.Id)]
        public static void HandleIgnoredGetListMessage(WorldClient client, IgnoredGetListMessage message)
        {
            SendIgnoredListMessage(client, new IgnoredInformations[0]);
        }

        public static void SendFriendsListMessage(WorldClient client, IEnumerable<FriendInformations> friends)
        {
            client.Send(new FriendsListMessage(friends));
        }

        public static void SendIgnoredListMessage(WorldClient client, IEnumerable<IgnoredInformations> ignoreds)
        {
            client.Send(new IgnoredListMessage(ignoreds));
        }

        public static void SendFriendWarnOnConnectionStateMessage(WorldClient client, bool state)
        {
            client.Send(new FriendWarnOnConnectionStateMessage(state));
        }

        public static void SendGuildMemberWarnOnConnectionStateMessage(WorldClient client, bool state)
        {
            client.Send(new GuildMemberWarnOnConnectionStateMessage(state));
        }

        public static void SendFriendWarnOnLevelGainStateMessage(WorldClient client, bool state)
        {
            client.Send(new FriendWarnOnLevelGainStateMessage(state));
        }
    }
}