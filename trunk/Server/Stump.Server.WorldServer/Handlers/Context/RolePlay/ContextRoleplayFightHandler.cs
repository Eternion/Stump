using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        public static void SendGameRolePlayPlayerFightFriendlyAnsweredMessage(WorldClient client, Character replier,
                                                                              Character source, Character target,
                                                                              bool accepted)
        {
            client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage(replier.Id,
                                                                           source.Id,
                                                                           target.Id,
                                                                           accepted));
        }

        public static void SendGameRolePlayPlayerFightFriendlyRequestedMessage(WorldClient client, Character requester,
                                                                               Character source,
                                                                               Character target)
        {
            client.Send(new GameRolePlayPlayerFightFriendlyRequestedMessage(requester.Id, source.Id,
                                                                            target.Id));
        }

        public static void SendGameRolePlayArenaUpdatePlayerInfosMessage(WorldClient client)
        {
            client.Send(new GameRolePlayArenaUpdatePlayerInfosMessage(0, 0, 0, 0, 0));
        }
    }
}