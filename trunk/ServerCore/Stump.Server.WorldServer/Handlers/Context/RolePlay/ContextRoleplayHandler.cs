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
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.World.Actors.Actor;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {

        [WorldHandler(typeof(ChangeMapMessage))]
        public static void HandleChangeMapMessage(WorldClient client, ChangeMapMessage message)
        {
            var res = client.ActiveCharacter.Restrictions;
            Map nextMap = World.World.Instance.GetMap((int) message.mapId);

            /* changement de zone non autorisé */
            if (res.cantChangeZone && client.ActiveCharacter.Map.SubArea != nextMap.SubArea)
            {
                // TODO send message to chat
                return;
            }

            /* deplacement non autorisé */
            if (res.cantMove)
            {
                // TODO send message to chat
                return;
            }

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
            client.Send(new CurrentMapMessage((uint)mapId));
        }

        public static void SendMapComplementaryInformationsDataMessage(WorldClient client)
        {
            var c = client.ActiveCharacter;

            client.Send(new MapComplementaryInformationsDataMessage(
                            (uint)c.Map.SubArea.Id,
                            (uint)c.Map.Id,
                            (int)c.Map.SubArea.AlignmentSide,
                            new List<HouseInformations>(),
                            c.Map.GetActors(),
                            client.ActiveCharacter.Map.InteractiveObjects.Select(
                                entry => entry.Value.ToNetworkElement(client)).ToList(),
                            new List<StatedElement>(),
                            new List<MapObstacle>(),
                            new List<FightCommonInformations>()));
        }


        public static void SendGameRolePlayShowActorMessage(WorldClient client, Actor actor)
        {
            client.Send(new GameRolePlayShowActorMessage(actor.ToGameRolePlayActor()));
        }
    }
}