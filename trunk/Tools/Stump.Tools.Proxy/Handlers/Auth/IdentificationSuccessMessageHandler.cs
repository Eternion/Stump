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
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Handlers.Auth;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Messages
{
    internal class IdentificationSuccessMessageHandler : AuthHandlerContainer
    {
        [AuthHandler(typeof (IdentificationSuccessMessage))]
        public static void HandleIdentificationSuccessMessage(AuthClient client, IdentificationSuccessMessage message)
        {
            //message.accountId = 1;
            //message.nickname = "MegaAdmin";
            message.hasRights = true;

            client.Send(message);
        }

        [AuthHandler(typeof(IdentificationFailedForBadVersionMessage))]
        public static void HandleIdentificationFailedForBadVersionMessage(AuthClient client, IdentificationFailedForBadVersionMessage message)
        {
            //message.accountId = 1;
            //message.nickname = "MegaAdmin";
          //  message.hasRights = true;

            client.Send(new IdentificationSuccessMessage("jeredejerede",1,1,true,"",0,false));
        }
    }
}