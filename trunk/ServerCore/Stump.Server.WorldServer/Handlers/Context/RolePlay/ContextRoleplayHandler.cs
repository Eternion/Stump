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
using System.Linq;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {
        [WorldHandler(typeof(ChangeMapMessage))]
        public static void HandleChangeMapMessage(WorldClient client, ChangeMapMessage message)
        {
            Map nextMap = World.Instance.GetMap((int)message.mapId);

            client.ActiveCharacter.ChangeMap(nextMap);
        }

        [WorldHandler(typeof(MapInformationsRequestMessage))]
        public static void HandleMapInformationsRequestMessage(WorldClient client, MapInformationsRequestMessage message)
        {
            if (!client.ActiveCharacter.InWorld)
                client.ActiveCharacter.LogIn();

            SendMapComplementaryInformationsDataMessage(client);
        }

        public static void SendCurrentMapMessage(WorldClient client, int mapId)
        {
            client.Send(new CurrentMapMessage((uint) mapId));
        }

        public static void SendMapComplementaryInformationsDataMessage(WorldClient client)
        {
            client.Send(new MapComplementaryInformationsDataMessage(
                (uint)client.ActiveCharacter.Zone.Id,
                (uint)client.ActiveCharacter.Map.Id,
                0,
                new List<HouseInformations>(),
                client.ActiveCharacter.Map.Entities.Select(entry => entry.Value.ToNetworkActor(client)).ToList(),
                client.ActiveCharacter.Map.InteractiveObjects.Select(entry => entry.Value.ToNetworkElement(client)).ToList(),
                new List<StatedElement>(),
                new List<MapObstacle>(),
                new List<FightCommonInformations>()));
        }


        public static void SendGameRolePlayShowActorMessage(WorldClient client, Entity entity)
        {
            client.Send(new GameRolePlayShowActorMessage(entity.ToNetworkActor(client)));
        }
    }
}