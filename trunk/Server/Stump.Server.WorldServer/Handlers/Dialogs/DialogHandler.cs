using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Dialogs
{
    public class DialogHandler : WorldHandlerContainer
    {
        [WorldHandler(LeaveDialogRequestMessage.Id)]
        public static void HandleLeaveDialogRequestMessage(WorldClient client, LeaveDialogRequestMessage message)
        {
            client.ActiveCharacter.DenyRequest();
        }

        [WorldHandler(LeaveDialogMessage.Id)]
        public static void HandleLeaveDialogMessage(WorldClient client, LeaveDialogMessage message)
        {
            if (!client.ActiveCharacter.IsDialoging())
                return;

            client.ActiveCharacter.Dialog.Close();
        }

        public static void SendLeaveDialogMessage(WorldClient client)
        {
            client.Send(new LeaveDialogMessage());
        }
    }
}
