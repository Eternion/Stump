using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Security
{
    public class SecurityHandler : WorldHandlerContainer
    {
        [WorldHandler(ClientKeyMessage.Id)]
        public static void HandleClientKeyMessage(WorldClient client, ClientKeyMessage message)
        {
            client.Account.LastClientKey = message.key;
            IPCAccessor.Instance.Send(new UpdateAccountMessage(client.Account));
            IPCAccessor.Instance.SendRequest<BanClientKeyAnswerMessage>(new BanClientKeyRequestMessage { ClientKey = message.key },
                msg => WorldServer.Instance.IOTaskPool.AddMessage(() => OnClientKeyReceived(msg, client)), error => client.Disconnect());
            
        }

        private static void OnClientKeyReceived(BanClientKeyAnswerMessage message, WorldClient client)
        {
            if (!message.IsBanned)
                return;

            client.Character.SendSystemMessage(50, true, "Vous êtes banni jusqu'au", message.EndDate.ToString("dd/MM/yyyy - HH:mm:ss"));
            client.DisconnectLater(1000);
        }
    }
}
