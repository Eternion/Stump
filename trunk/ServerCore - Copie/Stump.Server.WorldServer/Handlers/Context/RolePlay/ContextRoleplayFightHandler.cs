
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {
        public static void SendGameRolePlayPlayerFightFriendlyAnsweredMessage(WorldClient client, Character replier,
                                                                              Character source, Character target,
                                                                              bool accepted)
        {
            client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage((int) replier.Id,
                                                                           (uint) source.Id,
                                                                           (uint) target.Id,
                                                                           accepted));
        }

        public static void SendGameRolePlayPlayerFightFriendlyRequestedMessage(WorldClient client, Character requester,
                                                                               Character source,
                                                                               Character target)
        {
            client.Send(new GameRolePlayPlayerFightFriendlyRequestedMessage((uint) requester.Id, (uint) source.Id,
                                                                            (uint) target.Id));
        }
    }
}