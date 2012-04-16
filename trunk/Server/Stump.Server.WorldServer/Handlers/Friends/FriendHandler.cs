using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Social;

namespace Stump.Server.WorldServer.Handlers.Friends
{
    public class FriendHandler : WorldHandlerContainer
    {
        [WorldHandler(FriendsGetListMessage.Id)]
        public static void HandleFriendsGetListMessage(WorldClient client, FriendsGetListMessage message)
        {
            SendFriendsListMessage(client, client.Character.FriendsBook.Friends);
        }

        [WorldHandler(IgnoredGetListMessage.Id)]
        public static void HandleIgnoredGetListMessage(WorldClient client, IgnoredGetListMessage message)
        {
            SendIgnoredListMessage(client, client.Character.FriendsBook.Ignoreds);
        }

        [WorldHandler(FriendAddRequestMessage.Id)]
        public static void HandleFriendAddRequestMessage(WorldClient client, FriendAddRequestMessage message)
        {
            var character = Game.World.Instance.GetCharacter(message.name);

            if (character != null)
            {
                client.Character.FriendsBook.AddFriend(character.Client.WorldAccount);
            }
            else
            {
                WorldServer.Instance.IOTaskPool.AddMessage(
                    () =>
                        {
                            WorldAccount record = WorldAccount.FindByNickname(message.name);

                            if (record != null)
                            {
                                client.Character.ExecuteInContext(
                                    () => client.Character.FriendsBook.AddFriend(record));
                            }
                            else
                            {
                                SendFriendAddFailureMessage(client, ListAddFailureEnum.LIST_ADD_FAILURE_NOT_FOUND);
                            }
                        });
            }
        }

        [WorldHandler(FriendDeleteRequestMessage.Id)]
        public static void HandleFriendDeleteRequestMessage(WorldClient client, FriendDeleteRequestMessage message)
        {
            var friend = client.Character.FriendsBook.Friends.FirstOrDefault(entry => entry.Account.Nickname == message.name);

            if (friend == null)
            {
                SendFriendDeleteResultMessage(client, false, message.name);
                return;
            }

            client.Character.FriendsBook.RemoveFriend(friend);
        }

        [WorldHandler(IgnoredAddRequestMessage.Id)]
        public static void HandleIgnoredAddRequestMessage(WorldClient client, IgnoredAddRequestMessage message)
        {
            var character = Game.World.Instance.GetCharacter(message.name);

            if (character != null)
            {
                var result = client.Character.FriendsBook.AddIgnored(character.Client.WorldAccount, message.session);
            }
            else
            {
                WorldServer.Instance.IOTaskPool.AddMessage(
                    () =>
                    {
                        WorldAccount record = WorldAccount.FindByNickname(message.name);

                        if (record != null)
                        {
                            client.Character.ExecuteInContext(
                                () => client.Character.FriendsBook.AddIgnored(record, message.session));
                        }
                        else
                        {
                            SendIgnoredAddFailureMessage(client, ListAddFailureEnum.LIST_ADD_FAILURE_NOT_FOUND);
                        }
                    });
            }
        }

        [WorldHandler(IgnoredDeleteRequestMessage.Id)]
        public static void HandleIgnoredDeleteRequestMessage(WorldClient client, IgnoredDeleteRequestMessage message)
        {
            var ignored = client.Character.FriendsBook.Ignoreds.FirstOrDefault(entry => entry.Account.Nickname == message.name);

            if (ignored == null)
            {
                SendIgnoredDeleteResultMessage(client, false, false, message.name);
                return;
            }

            client.Character.FriendsBook.RemoveIgnored(ignored);
        }

        [WorldHandler(FriendSetWarnOnConnectionMessage.Id)]
        public static void HandleFriendSetWarnOnConnectionMessage(WorldClient client, FriendSetWarnOnConnectionMessage message)
        {
            client.Character.FriendsBook.WarnOnConnection = message.enable;
        }

        [WorldHandler(FriendWarnOnLevelGainStateMessage.Id)]
        public static void HandleFriendWarnOnLevelGainStateMessage(WorldClient client, FriendWarnOnLevelGainStateMessage message)
        {
            client.Character.FriendsBook.WarnOnLevel = message.enable;
        }

        public static void SendFriendWarnOnConnectionStateMessage(IPacketReceiver client, bool state)
        {
            client.Send(new FriendWarnOnConnectionStateMessage(state));
        }

        public static void SendGuildMemberWarnOnConnectionStateMessage(IPacketReceiver client, bool state)
        {
            client.Send(new GuildMemberWarnOnConnectionStateMessage(state));
        }

        public static void SendFriendWarnOnLevelGainStateMessage(IPacketReceiver client, bool state)
        {
            client.Send(new FriendWarnOnLevelGainStateMessage(state));
        }

        public static void SendFriendAddFailureMessage(IPacketReceiver client, ListAddFailureEnum reason)
        {
            client.Send(new FriendAddFailureMessage((sbyte)reason));
        }

        public static void SendFriendDeleteResultMessage(IPacketReceiver client, bool success, string name)
        {
            client.Send(new FriendDeleteResultMessage(success, name));
        }

        public static void SendFriendUpdateMessage(IPacketReceiver client, Friend friend)
        {
            client.Send(new FriendUpdateMessage(friend.GetFriendInformations()));
        }

        public static void SendFriendsListMessage(IPacketReceiver client, IEnumerable<Friend> friends)
        {
            client.Send(new FriendsListMessage(friends.Select(entry => entry.GetFriendInformations())));
        }

        public static void SendIgnoredAddFailureMessage(IPacketReceiver client,  ListAddFailureEnum reason)
        {
            client.Send(new IgnoredAddFailureMessage((sbyte)reason));
        }

        public static void SendIgnoredDeleteResultMessage(IPacketReceiver client, bool success, bool session, string name)
        {
            client.Send(new IgnoredDeleteResultMessage(success, session, name));
        }

        public static void SendIgnoredListMessage(IPacketReceiver client, IEnumerable<Ignored> ignoreds)
        {
            client.Send(new IgnoredListMessage(ignoreds.Select(entry => entry.GetIgnoredInformations())));
        }
    }
}