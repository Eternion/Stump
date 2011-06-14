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
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Utils;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.DataProvider.Data.Threshold;
using Stump.Server.WorldServer.Helpers;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        /// <summary>
        /// Number of maximum authorized character deletion by day
        /// </summary>
        [Variable]
        public static int MaxDayCharacterDeletion = 5;

        [WorldHandler(typeof(CharacterDeletionRequestMessage))]
        public static void HandleCharacterDeletionRequestMessage(WorldClient client, CharacterDeletionRequestMessage message)
        {
            var character = client.GetCharacterById(message.characterId);

            /* check null */
            if (character == null)
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_NO_REASON));
                client.DisconnectLater(1000);
                return;
            }

            /* Bad secret Answer */
            if (ThresholdManager.Instance["CharacterExp"].GetLevel(character.Experience) > 20 && client.Account.SecretAnswer != null
                    && message.secretAnswerHash != StringUtils.GetMd5(message.characterId + "~" + client.Account.SecretAnswer))
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_BAD_SECRET_ANSWER));
                return;
            }


            /* Too many character deletion */
            if (AccountManager.GetNumberOfDayDeletedCharacter(client.Account.Id) > MaxDayCharacterDeletion)
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_TOO_MANY_CHAR_DELETION));
                return;
            }

            AccountManager.RemoveCharacterFromAccount(character, client);

            SendCharactersListMessage(client);
            BasicHandler.SendBasicNoOperationMessage(client);
        }

    }
}