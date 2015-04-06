using System.Collections.Generic;
using Stump.DofusProtocol.Enums.Custom;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Game.Fights.Challenges;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler
    {
        public static void SendChallengeInfoMessage(IPacketReceiver client, DefaultChallenge challenge)
        {
            client.Send(new ChallengeInfoMessage((short)challenge.Id, challenge.TargetId, challenge.Bonus, 0, challenge.Bonus, 0));
        }

        public static void SendChallengeResultMessage(IPacketReceiver client, DefaultChallenge challenge)
        {
            client.Send(new ChallengeResultMessage((short)challenge.Id, challenge.Status == ChallengeStatusEnum.SUCCESS));
        }

        public static void SendChallengeTargetsListMessage(IPacketReceiver client, IEnumerable<int> targetIds, IEnumerable<short> targetCells)
        {
            client.Send(new ChallengeTargetsListMessage(targetIds, targetCells));
        }
    }
}
