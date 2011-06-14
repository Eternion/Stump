
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class DialogHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (LeaveDialogRequestMessage))]
        public static void HandleLeaveDialogRequestMessage(WorldClient client, LeaveDialogRequestMessage message)
        {
            if (client.ActiveCharacter.IsDialogRequested)
            {
                client.ActiveCharacter.DialogRequest.DeniedDialog();
            }
            else if (client.ActiveCharacter.IsInDialog)
            {
                client.ActiveCharacter.Dialog.EndDialog();
            }
        }

        public static void SendLeaveDialogMessage(WorldClient client)
        {
            client.Send(new LeaveDialogMessage());
        }
    }
}