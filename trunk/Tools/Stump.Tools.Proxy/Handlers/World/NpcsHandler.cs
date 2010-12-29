﻿// /*************************************************************************
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
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class NpcsHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (NpcDialogQuestionMessage))]
        public static void HandleNpcDialogQuestionMessage(WorldClient client, NpcDialogQuestionMessage message)
        {
            client.Send(message);

            DataFactory.HandleNpcQuestion(client, message);
        }

        [WorldHandler(typeof(NpcDialogReplyMessage))]
        public static void HandleNpcDialogReplyMessage(WorldClient client, NpcDialogReplyMessage message)
        {
            client.GuessNpcReply = message;

            client.Server.Send(message);
        }

        [WorldHandler(typeof (LeaveDialogMessage))]
        public static void HandleLeaveDialogMessage(WorldClient client, LeaveDialogMessage message)
        {
            client.Send(message);

            DataFactory.BuildActionNpcLeave(client, message);
        }

        [WorldHandler(typeof(NpcGenericActionRequestMessage))]
        public static void HandleNpcGenericActionRequestMessage(WorldClient client, NpcGenericActionRequestMessage message)
        {
            client.GuessNpcFirstAction = message;

            client.Server.Send(message);
        }


        [WorldHandler(typeof (ExchangeStartOkNpcShopMessage))]
        public static void HandleExchangeStartOkNpcShopMessage(WorldClient client, ExchangeStartOkNpcShopMessage message)
        {
            client.Send(message);

            DataFactory.BuildActionNpcShop(client, message);
        }
    }
}