using System;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler
    {
        [Variable]
        public static int MaxDayCharacterDeletion = 5;

        [WorldHandler(CharacterDeletionRequestMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterDeletionRequestMessage(WorldClient client, CharacterDeletionRequestMessage message)
        {
            var character = client.Characters.Find(entry => entry.Id == message.characterId);

            /* check null */
            if (character == null)
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_NO_REASON));
                client.DisconnectLater(1000);
                return;
            }

            var secretAnswerHash = message.secretAnswerHash;

            /* Level < 20 or > 20 and Good secret Answer */
            if (ExperienceManager.Instance.GetCharacterLevel(character.Experience, character.PrestigeRank) <= 20 || (client.Account.SecretAnswer != null
                    && secretAnswerHash == (message.characterId + "~" + client.Account.SecretAnswer).GetMD5()))
            {
                /* Too many character deletion */
                if (client.Account.DeletedCharactersCount > MaxDayCharacterDeletion)
                {
                    client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_TOO_MANY_CHAR_DELETION));
                    return;
                }

                character.DeletedDate = DateTime.Now;
                CharacterManager.Instance.Database.Update(character);

                SendCharactersListWithRemodelingMessage(client);
                BasicHandler.SendBasicNoOperationMessage(client);
            }
            else
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_BAD_SECRET_ANSWER));
            }
        }

    }
}