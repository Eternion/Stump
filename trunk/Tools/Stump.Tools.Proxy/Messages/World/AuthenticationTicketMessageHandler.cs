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
using System.Text;
using Stump.DofusProtocol.Messages;
using System.Net;

namespace Stump.Tools.Proxy.Messages
{
    class AuthenticationTicketMessageHandler
    {


        [Handler(typeof(AuthenticationTicketMessage))]
        public static void HandleAuthenticationTicketMessage(AuthenticationTicketMessage message, DerivedConnexion sender)
        {
             if (WorldDerivedConnexion.Tickets.ContainsKey(message.ticket))
             {
                 SelectedServerDataMessage mess = WorldDerivedConnexion.Tickets[message.ticket];
                 WorldDerivedConnexion.Tickets.Remove(message.ticket);
                 (sender as WorldDerivedConnexion).Ticket = mess.ticket;
                 sender.BindToServer(new IPEndPoint(IPAddress.Parse(mess.address), (int)mess.port));
             }
             else
                 sender.Client.Disconnect();
        }

    }
}
