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

namespace Stump.Tools.Proxy.Messages
{
    class SelectedServerDataMessageHandler
    {

        /* Intercept and modify adress & port of the server */
        [Handler(typeof(SelectedServerDataMessage))]
        public static void HandleSelectedServerDataMessage(SelectedServerDataMessage message, DerivedConnexion sender)
        {
             WorldDerivedConnexion.Tickets.Add(message.ticket, message);

             var mess = new SelectedServerDataMessage();

             mess.canCreateNewCharacter = true;

             mess.serverId = message.serverId;

             mess.ticket = message.ticket;

             mess.address = Proxy.worldClientListener.Host;

             mess.port = (uint)Proxy.worldClientListener.Port;

             sender.Client.Send(mess);

             sender.Client.Disconnect();
        }

    }
}
