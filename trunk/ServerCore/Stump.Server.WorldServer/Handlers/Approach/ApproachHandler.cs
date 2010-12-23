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
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Database;
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
            AccountRecord ticketAccount = AccountManager.GetAccountByTicket(message.ticket);

            if (ticketAccount == null)
            {
                client.Send(new AuthenticationTicketRefusedMessage()); 
                client.Disconnect();
                return;
            }
          
            client.Account = ticketAccount;

            client.Characters = CharacterManager.GetCharactersByAccount(client);

            client.Send(new AuthenticationTicketAcceptedMessage());
            BasicHandler.SendBasicTimeMessage(client);
            SendAccountCapabilitiesMessage(client);
            BasicHandler.SendBasicNoOperationMessage(client);
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

        public static void SendConsoleCommandsListMessage(WorldClient client,List<string> aliases,List<string> args,List<string> descr)
        {
            client.Send(new ConsoleCommandsListMessage(aliases, args, descr));
        }
    }
}