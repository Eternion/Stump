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
using System.Collections.Generic;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class PvpHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (SetEnablePVPRequestMessage))]
        public static void HandleSetEnablePvpRequestMessage(WorldClient client, SetEnablePVPRequestMessage message)
        {
        }

        [WorldHandler(typeof (GetPVPActivationCostMessage))]
        public static void HandleGetPvpActivationCostMessage(WorldClient client, GetPVPActivationCostMessage message)
        {
        }


        public static void SendAlignmentSubAreasListMessage(WorldClient client)
        {
            client.Send(new AlignmentSubAreasListMessage(new List<int>(), new List<int>()));
        }

        public static void SendAlignmentAreaUpdateMessage(WorldClient client)
        {
            client.Send(new AlignmentAreaUpdateMessage());
        }

        public static void SendAlignmentSubAreaUpdateMessage(WorldClient client)
        {
            client.Send(new AlignmentSubAreaUpdateMessage());
        }

        public static void SendAlignmentRankUpdateMessage(WorldClient client)
        {
            client.Send(new AlignmentRankUpdateMessage(0, false));
        }
    }
}