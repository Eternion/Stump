using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Breeds;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler
    {

        [WorldHandler(CharacterReplayRequestMessage.Id, RequiresLogin = false, IsGamePacket = false)]
        public static void HandleCharacterReplayRequestMessage(WorldClient client, CharacterReplayRequestMessage message)
        {
            // TODO mhh ?
        }

        [WorldHandler(CharacterReplayWithRenameRequestMessage.Id, RequiresLogin = false, IsGamePacket = false)]
        public static void HandleCharacterReplayWithRenameRequestMessage(WorldClient client, CharacterReplayWithRenameRequestMessage message)
        {
            var character = client.Characters.Find(o => o.Id == message.characterId);

            /* check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                client.DisconnectLater(1000);
                return;
            }

            /* Check if name is free */
            if (CharacterRecord.DoesNameExists(message.name))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }

            string characterName = message.name.ToLower().FirstLetterUpper();

            /* Check name */
            if (!Regex.IsMatch(characterName, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_INVALID_NAME));
                return;
            }

            /* Bind look and save character */
            character.Name = characterName;
            character.Save();
        }

        [WorldHandler(CharacterReplayWithRecolorRequestMessage.Id, RequiresLogin = false, IsGamePacket = false)]
        public static void HandleCharacterReplayWithRecolorRequestMessage(WorldClient client, CharacterReplayWithRecolorRequestMessage message)
        {
            var character = client.Characters.Find(entry => entry.Id == message.characterId);

            /* check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                client.DisconnectLater(1000);
                return;
            }

            /* Get character Breed */
            Breed breed = BreedManager.Instance.GetBreed((int) character.Breed);

            if (breed == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            /* Parse character colors */
            var indexedColors = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                int color = message.indexedColor.ElementAt(i);

                if (color == -1)
                    color = (int)(character.Sex == SexTypeEnum.SEX_MALE ? breed.MaleColors[i] : breed.FemaleColors[i]);

                indexedColors.Add((i + 1) << 24 | color);
            }

            var breedLook = character.Sex == SexTypeEnum.SEX_MALE ? breed.MaleLook.Copy() : breed.FemaleLook.Copy();
            breedLook.indexedColors = indexedColors;

            /* Bind look and save character */
            character.EntityLook = breedLook;
            character.Save();
        }

    }
}