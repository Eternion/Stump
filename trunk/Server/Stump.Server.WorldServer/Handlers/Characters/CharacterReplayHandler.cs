using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Breeds;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler
    {

        [WorldHandler(CharacterReplayRequestMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterReplayRequestMessage(WorldClient client, CharacterReplayRequestMessage message)
        {
            // TODO mhh ?
        }

        [WorldHandler(CharacterReplayWithRenameRequestMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
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
            if (CharacterManager.Instance.DoesNameExist(message.name))
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
            WorldServer.Instance.DBAccessor.Database.Update(character);
        }

        [WorldHandler(CharacterReplayWithRecolorRequestMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
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
            var breed = BreedManager.Instance.GetBreed((int) character.Breed);

            if (breed == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            var breedColors = character.Sex == SexTypeEnum.SEX_MALE ? breed.MaleColors : breed.FemaleColors;

            character.EntityLook.SetColors(
                message.indexedColor.Select((x, i) => x == -1 ? Color.FromArgb((int) breedColors[i]) : Color.FromArgb(x)).ToArray());

            WorldServer.Instance.DBAccessor.Database.Update(character);
        }

    }
}