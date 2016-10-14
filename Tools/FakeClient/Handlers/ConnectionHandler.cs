using System.Linq;
using System.Threading;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Game.Breeds;

namespace FakeClient.Handlers
{
    public class ConnectionHandler : FakeClientHandlerContainer
    {
        [FakeHandler(HelloConnectMessage.Id)]
        public static void HandleHelloConnectMessage(FakeClient client, HelloConnectMessage message)
        {
            var writer = new BigEndianWriter();
            writer.WriteUTF(client.AccountName);
            writer.WriteUTF(client.AccountPassword);

            client.Send(new IdentificationMessage(false, false, false, new Version(2, 10, 0, 65664, 0, (sbyte)BuildTypeEnum.RELEASE).ToVersionExtended(0, 0), "fr",
                writer.Data.Select(x => (sbyte)x), (short)WorldServer.ServerInformation.Id));
        }

        [FakeHandler(IdentificationFailedMessage.Id)]
        public static void HandleIdentificationFailedMessage(FakeClient client, IdentificationFailedMessage message)
        {
            client.Log("Connection error : " + (IdentificationFailureReasonEnum)message.reason);
        }

        [FakeHandler(SelectedServerDataMessage.Id)]
        public static void HandleSelectedServerDataMessage(FakeClient client, SelectedServerDataMessage message)
        {
            client.Disconnect(true);
            client.Ticket = message.ticket;
            client.WorldIp = message.address;
            client.WorldPort = message.port;
            //client.Connect(message.address, message.port);
        }

        [FakeHandler(ServersListMessage.Id)]
        public static void HandleServersListMessage(FakeClient client, ServersListMessage message)
        {
            client.ConnectingToWorld = true;
            client.Send(new ServerSelectionMessage((short)WorldServer.ServerInformation.Id));
        }

        [FakeHandler(HelloGameMessage.Id)]
        public static void HandleHelloGameMessage(FakeClient client, HelloGameMessage message)
        {
            client.ConnectingToWorld = false;
            client.Send(new AuthenticationTicketMessage("fr", client.Ticket));
        }

        [FakeHandler(AuthenticationTicketRefusedMessage.Id)]
        public static void HandleAuthenticationTicketRefusedMessage(FakeClient client, AuthenticationTicketRefusedMessage message)
        {
            client.Log("Ticket refused !");
        }

        [FakeHandler(AuthenticationTicketAcceptedMessage.Id)]
        public static void HandleAuthenticationTicketAcceptedMessage(FakeClient client, AuthenticationTicketAcceptedMessage message)
        {
            client.Send(new CharactersListRequestMessage());
        }

        [FakeHandler(CharactersListMessage.Id)]
        [FakeHandler(CharactersListWithModificationsMessage.Id)]
        public static void HandleCharactersListMessage(FakeClient client, CharactersListMessage message)
        {
            if (client.Id == 1)
                client.Send(new CharacterSelectionMessage(message.characters.First(x => x.name == "Loom2").id));
            if (client.Id == 2)
                client.Send(new CharacterSelectionMessage(message.characters.First(x => x.name == "Loom").id));
        }

        [FakeHandler(CharacterCreationResultMessage.Id)]
        public static void HandleCharacterCreationResultMessage(FakeClient client, CharacterCreationResultMessage message)
        {
            if ((CharacterCreationResultEnum)message.result != CharacterCreationResultEnum.OK)
                client.Log("Cannot create character : " + (CharacterCreationResultEnum)message.result);
        }

        [FakeHandler(CharacterSelectedSuccessMessage.Id)]
        public static void HandleCharacterSelectedSuccessMessage(FakeClient client, CharacterSelectedSuccessMessage message)
        {
            client.Send(new GameContextCreateRequestMessage());
            client.IsInGame = true;
        }

        [ FakeHandler(GameContextCreateMessage.Id)]
        public static void HandleGameContextCreateMessage( FakeClient client, GameContextCreateMessage message)
        {
            if (client.Id == 1)
            {
                client.Send(new NpcGenericActionRequestMessage(-1, 3, 83887104));
                Thread.Sleep(1000);
                client.Send(new NpcDialogReplyMessage(259));
            }
        }

        [ FakeHandler(ExchangeStartedMessage.Id)]
        public static void HandleExchangeStartedMessage( FakeClient client, ExchangeStartedMessage message)
        {
            client.Send(new ExchangeObjectMoveKamaMessage(-20000));
        }

        [ FakeHandler(StorageKamasUpdateMessage.Id)]
        public static void HandleStorageKamasUpdateMessage( FakeClient client, StorageKamasUpdateMessage message)
        {
            Thread.Sleep(1000);
            client.KamasTransferred = true;
        }
    }
}