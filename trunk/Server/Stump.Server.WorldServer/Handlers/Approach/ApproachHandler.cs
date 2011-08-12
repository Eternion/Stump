using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Worlds.Accounts;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Breeds;

namespace Stump.Server.WorldServer.Handlers.Approach
{
    public class ApproachHandler : WorldHandlerContainer
    {
        [WorldHandler(AuthenticationTicketMessage.Id)]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            /* Get Ticket */
            AccountData ticketAccount = AccountManager.Instance.GetAccountByTicket(message.ticket);

            /* Check null ticket */
            if (ticketAccount == null)
            {
                client.Send(new AuthenticationTicketRefusedMessage());
                client.DisconnectLater(1000);
                return;
            }

            /* Bind WorldAccount if exist */
            if (WorldAccount.Exists(ticketAccount.Id))
                client.WorldAccount = WorldAccount.FindById(ticketAccount.Id);
            
            
            /* Bind Account & Characters */
            client.Account = ticketAccount;
            client.Characters = CharacterManager.Instance.GetCharactersByAccount(client);

            /* Ok */
            client.Send(new AuthenticationTicketAcceptedMessage());
            SendAccountCapabilitiesMessage(client);

            client.Send(new TrustStatusMessage(true)); // usage -> ?

            /* Just to get console AutoCompletion */
            if (client.Account.Role >= RoleEnum.Moderator)
                SendConsoleCommandsListMessage(client);
        }

        public static void SendAccountCapabilitiesMessage(WorldClient client)
        {
            client.Send(new AccountCapabilitiesMessage(
                            (int)client.Account.Id,
                            false,
                            (short)client.Account.BreedFlags,
                            (short)BreedManager.Instance.AvailableBreedsFlags));
        }

        public static void SendConsoleCommandsListMessage(WorldClient client)
        {
            client.Send(
                new ConsoleCommandsListMessage(
                    WorldServer.Instance.CommandManager.CommandsByAlias.Values.SelectMany(c => c.Aliases),
                    new string[0],  new string[0]));
        }
    }
}