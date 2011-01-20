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
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Stump.BaseCore.Framework.Utils;
using Stump.Database;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Breeds;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {

        [WorldHandler(typeof(CharacterReplayRequestMessage))]
        public static void HandleCharacterReplayRequestMessage(WorldClient client, CharacterReplayRequestMessage message)
        {
            // TODO mhh ?
        }

        [WorldHandler(typeof(CharacterReplayWithRenameRequestMessage))]
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
            if (CharacterRecord.IsNameExists(message.name))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }

            string characterName = StringUtils.FirstLetterUpper(message.name.ToLower());

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

        [WorldHandler(typeof(CharacterReplayWithRecolorRequestMessage))]
        public static void HandleCharacterReplayWithRecolorRequestMessage(WorldClient client, CharacterReplayWithRecolorRequestMessage message)
        {
            var character = client.Characters.Find(o => o.Id == message.characterId);

            /* check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                client.DisconnectLater(1000);
                return;
            }

            /* Get character Breed */
            BaseBreed breed = BreedManager.GetBreed(character.Breed);

            /* Parse character colors */
            var indexedColors = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                int color = message.indexedColor[i];

                if (color == -1)
                    color = (int)(character.Sex == SexTypeEnum.SEX_MALE ? breed.MaleColors[i] : breed.FemaleColors[i]);

                indexedColors.Add(int.Parse((i + 1) + color.ToString("X6"), NumberStyles.HexNumber));
            }

            var breedLook = character.Sex == SexTypeEnum.SEX_MALE ? breed.MaleLook.Copy() : breed.FemaleLook.Copy();
            breedLook.indexedColors = indexedColors;

            /* Bind look and save character */
            character.BaseLook = breedLook;
            character.Save();
        }

    }
}