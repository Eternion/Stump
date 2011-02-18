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
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.World.Actors.Character;

namespace Stump.Server.WorldServer.Handlers
{
    public class BasicHandler : WorldHandlerContainer
    {

        [WorldHandler(typeof (BasicSwitchModeRequestMessage))]
        public static void HandleBasicSwitchModeRequestMessage(WorldClient client, BasicSwitchModeRequestMessage message)
        {

        }

        [WorldHandler(typeof (BasicWhoAmIRequestMessage))]
        public static void HandleBasicWhoAmIRequestMessage(WorldClient client, BasicWhoAmIRequestMessage message)
        {
            /* Get Current character */
            Character character = client.ActiveCharacter;

            /* Send informations about it */
            client.Send(new BasicWhoIsMessage(true, (int) character.Client.Account.Role,
                                              character.Client.WorldAccount.Nickname, character.Name, character.Map.SubArea.Area.Id));
        }

        [WorldHandler(typeof (BasicWhoIsRequestMessage))]
        public static void HandleBasicWhoIsRequestMessage(WorldClient client, BasicWhoIsRequestMessage message)
        {
            /* Get character */
            Character character = World.World.Instance.GetCharacter(message.search);

            /* check null */
            if (character == null)
            {
                client.Send(new BasicWhoIsNoMatchMessage(message.search));
            }
                /* Send info about it */
            else
            {
                client.Send(new BasicWhoIsMessage(message.search == client.ActiveCharacter.Name,
                                                  (int) character.Client.Account.Role,
                                                  character.Client.WorldAccount.Nickname, character.Name,
                                                  character.Map.SubArea.Area.Id));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name = "client"></param>
        /// <param name = "msgType"></param>
        /// <param name = "msgId"></param>
        /// <param name = "arguments"></param>
        /// <remarks>
        ///   Message id = <paramref name = "msgType" /> * 10000 + <paramref name = "msgId" />
        /// </remarks>
        public static void SendTextInformationMessage(WorldClient client, uint msgType, uint msgId,params string[] arguments)
        {
            client.Send(new TextInformationMessage(msgType, msgId, arguments.ToList()));
        }

        public static void SendTextInformationMessage(WorldClient client, uint msgType, uint msgId,params object[] arguments)
        {
            client.Send(new TextInformationMessage(msgType, msgId, arguments.Select(entry => entry.ToString()).ToList()));
        }

        public static void SendTextInformationMessage(WorldClient client, uint msgType, uint msgId)
        {
            client.Send(new TextInformationMessage(msgType, msgId, new List<string>()));
        }

        public static void SendBasicTimeMessage(WorldClient client)
        {
            var unixTimeStamp = (uint) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            var offset = (int) TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds;
            client.Send(new BasicTimeMessage(unixTimeStamp, offset));
        }

        public static void SendBasicNoOperationMessage(WorldClient client)
        {
            client.Send(new BasicNoOperationMessage());
        }

    }
}