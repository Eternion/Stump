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
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.AuthServer.Handlers
{
    public partial class ConnectionHandler
    {
        [AuthHandler(typeof(AcquaintanceSearchMessage))]
        public static void HandleAcquaintanceSearchMessage(AuthClient client, AcquaintanceSearchMessage message)
        {
            var ac = AccountRecord.FindAccountByNickname(message.nickname);

            if (ac == null)
            {
                SendAcquaintanceSearchErrorMessage(client, AcquaintanceErrorEnum.NO_RESULT);
                return;
            }

            SendAcquaintanceSearchServerListMessage(client, ac.Characters.Select(wcr => wcr.WorldId).Distinct().ToList());
        }

        public static void SendAcquaintanceSearchServerListMessage(AuthClient client, List<int> serverIds)
        {
            client.Send(new AcquaintanceServerListMessage(serverIds));
        }

        public static void SendAcquaintanceSearchErrorMessage(AuthClient client, AcquaintanceErrorEnum reason)
        {
            client.Send(new AcquaintanceSearchErrorMessage((uint)reason));
        }



    }
}