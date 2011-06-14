using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Manager;
using Stump.Server.WorldServer.Threshold;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        [Variable]
        public static int MaxDayCharacterDeletion = 5;

        [WorldHandler(typeof(CharacterDeletionRequestMessage))]
        public static void HandleCharacterDeletionRequestMessage(WorldClient client, CharacterDeletionRequestMessage message)
        {
            CharacterRecord character = client.Characters.Find(o => o.Id == message.characterId);

            /* check null */
            if (character == null)
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_NO_REASON));
                client.DisconnectLater(1000);
                return;
            }

            string secretAnswerHash = message.secretAnswerHash;

            /* Level < 20 or > 20 and Good secret Answer */
            if (ThresholdManager.Thresholds["CharacterExp"].GetLevel(character.Experience) <= 20 || (client.Account.SecretAnswer != null
                    && secretAnswerHash == StringExtensions.GetMd5(message.characterId + "~" + client.Account.SecretAnswer)))
            {
                /* Too many character deletion */
                if (CharacterManager.GetAccountDeletedCharactersNumber(client.Account.Id) > MaxDayCharacterDeletion)
                {
                    client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_TOO_MANY_CHAR_DELETION));
                    return;
                }

                CharacterManager.DeleteCharacterOnAccount(character, client);

                SendCharactersListMessage(client);
                BasicHandler.SendBasicNoOperationMessage(client);
            }
            else
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_BAD_SECRET_ANSWER));
            }
        }

    }
}