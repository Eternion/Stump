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
using Stump.Database;
using Stump.Server.WorldServer.Database;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class ApproachHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (AuthenticationTicketMessage))]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            /* On récupère le compte associé au ticket */
            AccountRecord ticketAccount = AccountManager.GetAccountByTicket(message.ticket);

            /* S'il n'y en avait pas */
            if (ticketAccount == null)
            {
                client.Send(new AuthenticationTicketRefusedMessage()); 
                client.Disconnect();
                return;
            }

            /* On associe le Client avec ce compte */
            client.Account = ticketAccount;

            /* On associe les personnages */
            client.Characters = CharacterManager.GetCharactersByAccount(client);

            client.Send(new AuthenticationTicketAcceptedMessage());
        }
    }
}