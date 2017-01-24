using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        public static void SendAlmanachCalendarDateMessage(IPacketReceiver client)
        {
            client.Send(new AlmanachCalendarDateMessage(377));
        }
    }
}
