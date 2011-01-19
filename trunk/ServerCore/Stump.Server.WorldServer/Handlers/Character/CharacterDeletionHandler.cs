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
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Manager;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (CharacterDeletionRequestMessage))]
        public static void HandleCharacterDeletionRequest(WorldClient client, CharacterDeletionRequestMessage message)
        {
            uint characterId = message.characterId;

            CharacterRecord characterRecord = client.Characters.Find(o => o.Id == characterId);

            /* Le personnage n'existe pas */
            if (characterRecord == null)
            {
                client.Send(new CharacterDeletionErrorMessage((int) CharacterDeletionErrorEnum.DEL_ERR_NO_REASON));
                client.Disconnect();
                return;
            }

            string secretAnswerHash = message.secretAnswerHash;

            /* Vérification de la réponse secrete si level > 20 */
            if (characterRecord.Level <= 20 || (client.Account.SecretAnswer != null &&
                                                secretAnswerHash ==
                                                StringUtils.GetMd5(characterId + "~" + client.Account.SecretAnswer)))
            {
                if (AccountManager.ExceedsDeletedCharactersQuota(client.Account.Id))
                {
                    client.Send(
                        new CharacterDeletionErrorMessage(
                            (int) CharacterDeletionErrorEnum.DEL_ERR_TOO_MANY_CHAR_DELETION));
                    return;
                }

                CharacterManager.DeleteCharacter(characterRecord, client);

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