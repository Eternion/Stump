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
using System.Text.RegularExpressions;
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.AuthServer.Handlers
{
    public partial class ConnectionHandler
    {
        [AuthHandler(typeof (NicknameChoiceRequestMessage))]
        public static void HandleNicknameChoiceRequestMessage(AuthClient client, NicknameChoiceRequestMessage message)
        {
            string nickname = message.nickname;

            /* Check the Username */
            if (!CheckNickName(nickname))
            {
                client.Send(new NicknameRefusedMessage((uint) NicknameErrorEnum.INVALID_NICK));
                return;
            }

            /* Same as Login */
            if (nickname == client.Account.Login)
            {
                client.Send(new NicknameRefusedMessage((uint) NicknameErrorEnum.SAME_AS_LOGIN));
                return;
            }

            /* Look like Login */
            if (client.Account.Login.Contains(nickname))
            {
                client.Send(new NicknameRefusedMessage((uint) NicknameErrorEnum.TOO_SIMILAR_TO_LOGIN));
                return;
            }

            /* Already Used */
            if (AccountRecord.FindByNickname(nickname) != null)
            {
                client.Send(new NicknameRefusedMessage((uint) NicknameErrorEnum.ALREADY_USED));
                return;
            }

            /* Ok, it's good */
            client.Account.Nickname = nickname;
            client.Save();

            client.Send(new NicknameAcceptedMessage());
            SendIdentificationSuccessMessage(client, false);
            SendServersListMessage(client);
        }


        public static bool CheckNickName(string nickName)
        {
            return Regex.IsMatch(nickName, @"^[a-zA-Z]{4,18}$");
        }

    }
}