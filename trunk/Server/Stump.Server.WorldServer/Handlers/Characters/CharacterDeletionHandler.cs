using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        [Variable]
        public static int MaxDayCharacterDeletion = 5;

        [WorldHandler(CharacterDeletionRequestMessage.Id, RequiresLogin = false, IsGamePacket = false)]
        public static void HandleCharacterDeletionRequestMessage(WorldClient client, CharacterDeletionRequestMessage message)
        {
            if (!IpcAccessor.Instance.IsConnected)
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_NO_REASON));
                client.DisconnectLater(1000);
                return;
            }

            CharacterRecord character = client.Characters.Find(entry => entry.Id == message.characterId);

            /* check null */
            if (character == null)
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_NO_REASON));
                client.DisconnectLater(1000);
                return;
            }

            string secretAnswerHash = message.secretAnswerHash;

            /* Level < 20 or > 20 and Good secret Answer */
            if (ExperienceManager.Instance.GetCharacterLevel(character.Experience) <= 20 || (client.Account.SecretAnswer != null
                    && secretAnswerHash == (message.characterId + "~" + client.Account.SecretAnswer).GetMD5()))
            {
                /* Too many character deletion */
                if (client.Account.DeletedCharactersCount > MaxDayCharacterDeletion)
                {
                    client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_TOO_MANY_CHAR_DELETION));
                    return;
                }

                CharacterManager.Instance.DeleteCharacterOnAccount(character, client);

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