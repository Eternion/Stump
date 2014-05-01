using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Game.Arena;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler
    {
        public static void SendGameRolePlayArenaFightPropositionMessage(IPacketReceiver client, ArenaPopup popup)
        {
            client.Send(new GameRolePlayArenaFightPropositionMessage(popup.Member.Team.Fight.Id, popup.Member.Team.GetAlliesInQueue().Select(x => x.Id), 10));
        }
    }
}