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
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class StartupHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (StartupActionsExecuteMessage))]
        public static void HandleStartupActionsListRequestMessage(WorldClient client,
                                                                  StartupActionsExecuteMessage message)
        {
            SendStartupActionsListMessage(client);
        }

        [WorldHandler(typeof (StartupActionsObjetAttributionMessage))]
        public static void HandleStartupActionsObjetAttributionMessage(WorldClient client,
                                                                       StartupActionsObjetAttributionMessage message)
        {
            if (message.characterId != 0)
            {
                // TODO Ajout de l'item au personnage

                CharacterRecord character = client.Characters.FirstOrDefault(c => c.Id == message.characterId);


                SendStartupActionFinishedMessage(client);
            }
        }

        public static void SendStartupActionsListMessage(WorldClient client)
        {
            IEnumerable<StartupActionAddObject> startupsActions =
                client.Account.StartupActions.Select(
                    s => new StartupActionAddObject(s.Id, s.Title, s.Text, s.DescUrl, s.PictureUrl, s.Items.Select(
                        i => new ObjectItemMinimalInformation(i.ItemTemplate, 0, false, new List<ObjectEffect>())).ToList()));

            client.Send(new StartupActionsListMessage(startupsActions.ToList()));
        }

        public static void SendStartupActionFinishedMessage(WorldClient client)
        {
            client.Send(new StartupActionFinishedMessage());
        }
    }
}