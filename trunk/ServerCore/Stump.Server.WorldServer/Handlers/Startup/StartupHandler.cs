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
using System.Linq;
using System.Collections.Generic;
using Stump.Database;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Manager;

namespace Stump.Server.WorldServer.Handlers
{
    public class StartupHandler : WorldHandlerContainer
    {

        [WorldHandler(typeof(StartupActionsExecuteMessage))]
        public static void HandleStartupActionsListRequestMessage(WorldClient client,StartupActionsExecuteMessage message)
        {
            SendStartupActionsListMessage(client);
        }

        [WorldHandler(typeof(StartupActionsObjetAttributionMessage))]
        public static void HandleStartupActionsObjetAttributionMessage(WorldClient client, StartupActionsObjetAttributionMessage message)
        {
            
        }

        public static void SendStartupActionsListMessage(WorldClient client)
        {
            var startupsActions =
                AccountManager.GetAccountStartupActions(client.Account.Id).Select(
                    s =>
                    new StartupActionAddObject(s.Id, s.Title, s.Text, s.DescUrl, s.PictureUrl,
                                               new List<ObjectItemMinimalInformation>(){new ObjectItemMinimalInformation(1,1,false,new List<ObjectEffect>())}));
            
            client.Send(new StartupActionsListMessage(startupsActions.ToList()));
        }

        public static void SendStartupActionFinishedMessage(WorldClient client)
        {
            client.Send(new StartupActionFinishedMessage());
        }

    }
}