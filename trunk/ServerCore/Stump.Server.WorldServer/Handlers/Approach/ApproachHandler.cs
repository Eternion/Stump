// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Breeds;
using Stump.Server.WorldServer.Manager;

namespace Stump.Server.WorldServer.Handlers
{
    public class ApproachHandler : WorldHandlerContainer
    {

        [WorldHandler(typeof(AuthenticationTicketMessage))]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            /* Get Ticket */
            var ticketAccount = AccountManager.GetAccountByTicket(message.ticket);

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
                (int)client.Account.Id,
                true,
                client.Account.DbAvailableBreeds,
                BreedManager.BreedsToFlag(BreedManager.AvailableBreeds)
                ));
        }

        public static void SendAccountLoggingKickedMessage(WorldClient client)
        {
            var date = client.WorldAccount.BanRemainingTime;
            client.Send(new AccountLoggingKickedMessage((uint)date.Days, (uint)date.Hours, (uint)date.Minutes));
        }

        public static void SendConsoleCommandsListMessage(WorldClient client)
        {
            client.Send(new ConsoleCommandsListMessage(WorldServer.Instance.CommandManager.AvailableCommands.SelectMany(c => c.Aliases).ToList(), new List<string>(), new List<string>()));
        }
    }
}