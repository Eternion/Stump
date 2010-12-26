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
using System.Net;
using System.Net.Sockets;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Proxy.Network
{
    public class WorldClient : ProxyClient
    {
        private static readonly Dictionary<string, SelectedServerDataMessage> m_tickets = new Dictionary<string, SelectedServerDataMessage>();

        public static void PushTicket(string ticket, SelectedServerDataMessage server)
        {
            m_tickets.Add(ticket, server);
        }

        public static SelectedServerDataMessage PopTicket(string ticket)
        {
            if (!m_tickets.ContainsKey(ticket))
                return null;

            var result = m_tickets[ticket];

            m_tickets.Remove(ticket);

            return result;
        }

        public string Ticket
        {
            get;
            set;
        }

        public CharacterBaseInformations CharacterInformations
        {
            get;
            set;
        }

        public NpcDialogReplyMessage GuessNpcReply
        {
            get;
            set;
        }

        public NpcGenericActionRequestMessage GuessNpcFirstAction
        {
            get;
            set;
        }

        public Dictionary<int, GameRolePlayNpcInformations> MapNpcs
        {
            get;
            set;
        }

        public bool GuessAction
        {
            get
            {
                return GuessNpcReply != null || GuessNpcFirstAction != null;
            }
        }

        public WorldClient(Socket socket)
            : base(socket)
        {
            MapNpcs = new Dictionary<int, GameRolePlayNpcInformations>();

            Send(new ProtocolRequired(1304, 1304));
            Send(new HelloGameMessage());
        }
    }
}