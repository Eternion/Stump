
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Database.AuthServer;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Breeds;
using Stump.Server.WorldServer.Manager;

namespace Stump.Server.WorldServer.Handlers
{
    public class ApproachHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (AuthenticationTicketMessage))]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            /* Get Ticket */
            AccountRecord ticketAccount = AccountManager.GetAccountByTicket(message.ticket);

            /* Check null ticket */
            if (ticketAccount == null)
            {
                client.Send(new AuthenticationTicketRefusedMessage());
                client.DisconnectLater(1000);
                return;
            }

            /* Bind WorldAccount if exist */
            if (WorldAccountRecord.Exists(ticketAccount.Id))
                client.WorldAccount = WorldAccountRecord.FindWorldAccountById(ticketAccount.Id);

            /* WorldAccount is banned */
            if (client.WorldAccount != null && client.WorldAccount.BanRemainingTime != TimeSpan.Zero)
            {
                SendAccountLoggingKickedMessage(client);
                return;
            }

            /* Bind Account & Characters */
            client.Account = ticketAccount;
            client.Characters = CharacterManager.GetCharactersByAccount(client);

            /* Ok */
            client.Send(new AuthenticationTicketAcceptedMessage());
            BasicHandler.SendBasicTimeMessage(client);
            SendAccountCapabilitiesMessage(client);
            BasicHandler.SendBasicNoOperationMessage(client);

            /* Just to get console AutoCompletion */
            if (client.Account.Role >= RoleEnum.Moderator)
                SendConsoleCommandsListMessage(client);
        }

        public static void SendAccountCapabilitiesMessage(WorldClient client)
        {
            client.Send(new AccountCapabilitiesMessage(
                            (int) client.Account.Id,
                            true,
                            client.Account.DbAvailableBreeds,
                            BreedManager.BreedsToFlag(BreedManager.AvailableBreeds)
                            ));
        }

        public static void SendAccountLoggingKickedMessage(WorldClient client)
        {
            TimeSpan date = client.WorldAccount.BanRemainingTime;
            client.Send(new AccountLoggingKickedMessage((uint) date.Days, (uint) date.Hours, (uint) date.Minutes));
        }

        public static void SendConsoleCommandsListMessage(WorldClient client)
        {
            client.Send(
                new ConsoleCommandsListMessage(
                    WorldServer.Instance.CommandManager.AvailableCommands.SelectMany(c => c.Aliases).ToList(),
                    new List<string>(), new List<string>()));
        }
    }
}