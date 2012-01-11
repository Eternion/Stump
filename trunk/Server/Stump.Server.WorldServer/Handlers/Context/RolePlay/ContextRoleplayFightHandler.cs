using System.Diagnostics.Contracts;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        public static void SendGameRolePlayPlayerFightFriendlyAnsweredMessage(IPacketReceiver client, Character replier,
                                                                              Character source, Character target,
                                                                              bool accepted)
        {
            Contract.Requires(client != null);
            Contract.Requires(replier != null);
            Contract.Requires(source != null);
            Contract.Requires(target != null);

            client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage(replier.Id,
                                                                           source.Id,
                                                                           target.Id,
                                                                           accepted));
        }

        public static void SendGameRolePlayPlayerFightFriendlyRequestedMessage(IPacketReceiver client, Character requester,
                                                                               Character source,
                                                                               Character target)
        {
            Contract.Requires(client != null);
            Contract.Requires(requester != null);
            Contract.Requires(source != null);
            Contract.Requires(target != null);

            client.Send(new GameRolePlayPlayerFightFriendlyRequestedMessage(requester.Id, source.Id,
                                                                            target.Id));
        }

        public static void SendGameRolePlayArenaUpdatePlayerInfosMessage(IPacketReceiver client)
        {
            Contract.Requires(client != null);

            client.Send(new GameRolePlayArenaUpdatePlayerInfosMessage(0, 0, 0, 0, 0));
        }
    }
}