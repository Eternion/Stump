using System.Diagnostics.Contracts;
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
            Contract.Requires(client != null);
            Contract.Requires(client.ActiveCharacter != null);

            client.ActiveCharacter.LeaveDialog();
        }

        [WorldHandler(LeaveDialogMessage.Id)]
        public static void HandleLeaveDialogMessage(WorldClient client, LeaveDialogMessage message)
        {
            Contract.Requires(client != null);
            Contract.Requires(client.ActiveCharacter != null);

            client.ActiveCharacter.LeaveDialog();
        }

        public static void SendLeaveDialogMessage(IPacketReceiver client)
        {
            Contract.Requires(client != null);
            client.Send(new LeaveDialogMessage());
        }
    }
}
