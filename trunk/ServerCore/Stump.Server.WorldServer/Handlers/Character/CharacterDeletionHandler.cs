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
using Stump.BaseCore.Framework.Utils;
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Database;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (CharacterDeletionRequestMessage))]
        public static void HandleCharacterDeletionRequest(WorldClient client, CharacterDeletionRequestMessage message)
        {
            //////////////////////////////////////////////////////////////////////////
            // TODO : - save a copy of the character during 30 days
            //////////////////////////////////////////////////////////////////////////

            uint characterId = message.characterId;

            CharacterRecord characterRecord = client.Characters.Find(o => o.Id == characterId &&
                                                                          o.AccountName.ToLower() ==
                                                                          client.Account.Login.ToLower());

            if (characterId < 0 || characterRecord == null) // Incorrect Value.
            {
                client.Send(new CharacterDeletionErrorMessage((int) CharacterDeletionErrorEnum.DEL_ERR_NO_REASON));
                client.Disconnect();
            }

            string secretAnswerHash = message.secretAnswerHash;

            if (characterRecord.Level <= 20 || (client.Account.SecretAnswer != null &&
                                                secretAnswerHash ==
                                                StringUtils.GetMd5(characterId + "~" + client.Account.SecretAnswer)))
            {
                CharacterDatabase.DeleteCharacter(characterRecord, client);

                SendCharactersListMessage(client);
                BasicHandler.SendBasicNoOperationMessage(client);
            }
            else
            {
                client.Send(new CharacterDeletionErrorMessage((int) CharacterDeletionErrorEnum.DEL_ERR_BAD_SECRET_ANSWER));
            }
        }
    }
}