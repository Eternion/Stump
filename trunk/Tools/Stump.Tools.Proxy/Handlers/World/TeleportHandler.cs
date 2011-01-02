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
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class TeleportHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (CurrentMapMessage))]
        public static void HandleCurrentMapMessage(WorldClient client, CurrentMapMessage message)
        {
            client.Send(message);

            if (client.HasReceive(typeof(LeaveDialogMessage), 2))
                client.GuessNpcReply = client.LastNpcReply;

            if(client.GuessAction)
            {
                client.CallWhenTeleported(() => DataFactory.BuildActionTeleport(client, message));
            }
        }

        [WorldHandler(typeof (TeleportDestinationsListMessage))]
        public static void HandleTeleportDestinationsListMessage(WorldClient client, TeleportDestinationsListMessage message)
        {
            client.Send(message);
        }
    }
}