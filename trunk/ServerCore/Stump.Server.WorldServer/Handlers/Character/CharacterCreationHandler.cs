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
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.IPC;
using Stump.Server.WorldServer.Manager;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(CharacterCreationRequestMessage))]
        public static void HandleCharacterCreateRequest(WorldClient client, CharacterCreationRequestMessage message)
        {
            // 0) Check if we can create characters on this server
            if (client.Characters.Count >= World.MaxCharacterSlot)
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS));
                return;
            }

            if (CharacterRecord.IsNameExists(message.name))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }

            string characterName = StringUtils.FirstLetterUpper(message.name.ToLower());

            /* have bad name */
            if (!Regex.IsMatch(characterName, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_INVALID_NAME));
                return;
            }

            BaseBreed breed = BreedManager.GetBreed(message.breed);

            if (!client.Account.CanUseBreed(message.breed) ||
                !BreedManager.AvailableBreeds.Contains(breed.Id))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NOT_ALLOWED));
                return;
            }

            var indexedColors = new List<int>();
            for (int i = 0; i < 5; i++) // we ignore the sixth color
            {
                int color = message.colors[i];

                if (color == -1)
                    color = (int)( !message.sex ? breed.MaleColors[i] : breed.FemaleColors[i] );

                indexedColors.Add(int.Parse((i + 1) + color.ToString("X6"), NumberStyles.HexNumber));
            }

            var breedLook = !message.sex ? breed.MaleLook.Copy() : breed.FemaleLook.Copy();
            breedLook.indexedColors = indexedColors;


            /* Create Character */
            var character = new CharacterRecord
                {
                    New = true,
                    Account = client.Account,
                    Level = breed.StartLevel,
                    Name = characterName,
                    Breed = message.breed,
                    SexId = message.sex ? 1 : 0,
                    BaseLook = breedLook,
                    Kamas = breed.StartKamas,
                    MapId = (int)breed.StartMap,
                    CellId = breed.StartCellId,
                    BaseHealth = breed.StartHealthPoint,
                    DamageTaken = 0,
                    StatsPoints = 0,
                    SpellsPoints = 0,
                    Strength = 0,
                    Vitality = 0,
                    Wisdom = 0,
                    Intelligence = 0,
                    Chance = 0,
                    Agility = 0
                };

            /* Save it */
            CharacterManager.CreateCharacter(character, client);

            /* Add Spell */
            foreach (SpellIdEnum spellId in breed.StartSpells.Keys)
            {
                int position = breed.StartSpells[spellId];
                character.AddSpell(spellId, position, 1);
            }

            BasicHandler.SendBasicNoOperationMessage(client);
            client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.OK));
            SendCharactersListMessage(client);
        }


        [WorldHandler(typeof(CharacterNameSuggestionRequestMessage))]
        public static void CharacterNameSuggestionRequest(WorldClient client, CharacterNameSuggestionRequestMessage message)
        {
            string generatedName = CharacterManager.GenerateName();

            client.Send(new CharacterNameSuggestionSuccessMessage(generatedName));
        }
    }
}