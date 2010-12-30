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
using System.Threading.Tasks;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class MapComplementaryInformationsDataMessageHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (MapComplementaryInformationsDataMessage))]
        public static void HandleMapComplementaryInformationsDataMessage(WorldClient client,
                                                                         MapComplementaryInformationsDataMessage message)
        {
            client.Send(message);

            client.MapNpcs.Clear();
            client.MapIOs.Clear();
            client.CurrentMap = message.mapId;

            Parallel.ForEach(message.actors, actor =>
            {
                DataFactory.HandleActorInformations(client, actor);

                if (actor is GameRolePlayNpcInformations)
                    client.MapNpcs.Add(( actor as GameRolePlayNpcInformations ).contextualId, (GameRolePlayNpcInformations)actor);
                else if (actor is GameRolePlayCharacterInformations && ( actor as GameRolePlayCharacterInformations ).contextualId == client.CharacterInformations.id)
                    client.Disposition = actor.disposition;
            });

            Parallel.ForEach(message.interactiveElements, entry =>
            {
                DataFactory.HandleInteractiveObject(client, entry);

                client.MapIOs.Add((int) entry.elementId, entry);
            });
        }
    }
}