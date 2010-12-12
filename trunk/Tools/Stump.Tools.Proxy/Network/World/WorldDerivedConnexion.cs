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
using System.Net;
using System.Net.Sockets;
using NLog;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Classes;

namespace Stump.Tools.Proxy
{
    class WorldDerivedConnexion : DerivedConnexion
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static Dictionary<string, SelectedServerDataMessage> Tickets = new Dictionary<string, SelectedServerDataMessage>();

        public CharacterBaseInformations Infos
        { get; set; }

        public uint MapId
        { get; set; }

        public string Ticket
        { get; set; }

        public WorldDerivedConnexion(Client client)
            : base(client)
        {
            client.Send(new ProtocolRequired().initProtocolRequired(1304, 1304));
            Console.WriteLine("Have Send False Protocol Required");
            client.Send(new HelloGameMessage().initHelloGameMessage());
            Console.WriteLine("Have Send False Hello Game");
        }

    }
}
