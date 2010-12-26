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
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Commands;

namespace Stump.Server.WorldServer.Handlers
{
    public class AuthorizedHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(AdminQuietCommandMessage))]
        public static void HandleAdminQuietCommandMessage(WorldClient client, AdminQuietCommandMessage message)
        {
            if (client.Account.Role < RoleEnum.GameMaster_Padawan)
                return;

            string[] data = message.content.Split(' ');
            string command = data[0];
            string[] args = data[1].Split(' ');

            switch (command)
            {
                case ("look"):
                    {
                        client.ActiveCharacter.Look = new ExtendedLook(data[2].ToEntityLook());
                        ContextHandler.SendGameContextRefreshEntityLookMessage(client, client.ActiveCharacter);
                        break;
                    }
                //case ("moveto"):
                //    {

                //    }
            }
        }


        [WorldHandler(typeof(AdminCommandMessage))]
        public static void HandleAdminCommandMessage(WorldClient client, AdminCommandMessage message)
        {
            if (client.Account.Role < RoleEnum.GameMaster_Padawan)
                return;

            WorldServer.Instance.CommandManager.HandleCommand(new TriggerConsole(new StringStream(message.content),
                                                                                 client.ActiveCharacter));
        }

        public static void SendConsoleMessage(WorldClient client, string text)
        {
            SendConsoleMessage(client, ConsoleMessageTypeEnum.CONSOLE_TEXT_MESSAGE, text);
        }

        public static void SendConsoleMessage(WorldClient client, ConsoleMessageTypeEnum type, string text)
        {
            client.Send(new ConsoleMessage((uint)type, text));
        }
    }
}