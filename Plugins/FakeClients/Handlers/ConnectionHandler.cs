using System.Linq;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Game.Breeds;

namespace FakeClients.Handlers
{
    public class ConnectionHandler : FakeClientHandlerContainer
    {
        [FakeHandler(HelloConnectMessage.Id)]
        public static void HandleHelloConnectMessage(FakeClient client, HelloConnectMessage message)
        {
            var writer = new BigEndianWriter();
            writer.WriteUTF(FakeClientManager.AccountName + client.Id);
            writer.WriteUTF(FakeClientManager.AccountPassword);

            client.Send(new IdentificationMessage(false, false, false, VersionExtension.ExpectedVersion.ToVersionExtended(0,0), "fr",
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
            client.Connect(message.address, message.port);
        }

        [FakeHandler(ServersListMessage.Id)]
        public static void HandleServersListMessage(FakeClient client, ServersListMessage message)
        {
            client.ConnectingToWorld = true;
            client.Send(new ServerSelectionMessage((short) WorldServer.ServerInformation.Id));
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
            if (!message.characters.Any())
            {
                var head = BreedManager.Instance.GetHead(x => x.Breed == (int) PlayableBreedEnum.Cra);
                client.Send(new CharacterCreationRequestMessage("FakeCharacter#" + client.Id, (sbyte)PlayableBreedEnum.Cra,
                    false, Enumerable.Repeat(-1, 5), head.Id));
            }
            else
            {
                client.Send(new CharacterSelectionMessage(message.characters.First().id));
            }
        }

        [FakeHandler(CharacterCreationResultMessage.Id)]
        public static void HandleCharacterCreationResultMessage(FakeClient client, CharacterCreationResultMessage message)
        {
            if ((CharacterCreationResultEnum) message.result != CharacterCreationResultEnum.OK)
                client.Log("Cannot create character : " + (CharacterCreationResultEnum) message.result);
        }

        [ FakeHandler(CharacterSelectedSuccessMessage.Id)]
        public static void HandleCharacterSelectedSuccessMessage( FakeClient client, CharacterSelectedSuccessMessage message)
        {
            client.Send(new GameContextCreateRequestMessage());
        }   
    }
}