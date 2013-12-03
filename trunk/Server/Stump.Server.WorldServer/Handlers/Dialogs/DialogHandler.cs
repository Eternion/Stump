using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Dialogs
{
    public class DialogHandler : WorldHandlerContainer
    {
        [WorldHandler(LeaveDialogRequestMessage.Id)]
        public static void HandleLeaveDialogRequestMessage(WorldClient client, LeaveDialogRequestMessage message)
        {
            client.Character.LeaveDialog();
        }

        [WorldHandler(LeaveDialogMessage.Id)]
        public static void HandleLeaveDialogMessage(WorldClient client, LeaveDialogMessage message)
        {
            client.Character.LeaveDialog();
        }

        public static void SendLeaveDialogMessage(IPacketReceiver client, DialogTypeEnum dialogType)
        {
            client.Send(new LeaveDialogMessage((sbyte)dialogType));
        }
    }
}
