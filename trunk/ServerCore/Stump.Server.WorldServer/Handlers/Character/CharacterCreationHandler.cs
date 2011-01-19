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
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Utils;
using Stump.Database;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Breeds;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.IPC;
using Stump.Server.WorldServer.Manager;
using Stump.Server.WorldServer.Threshold;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        [Variable]
        public static bool EnableNameSuggestion = true;

        [WorldHandler(typeof(CharacterCreationRequestMessage))]
        public static void HandleCharacterCreationRequestMessage(WorldClient client, CharacterCreationRequestMessage message)
        {
            /* Check if we can create characters on this server */
            if (client.Characters.Count >= World.MaxCharacterSlot)
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS));
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

            /* Get character Breed */
            BaseBreed breed = BreedManager.GetBreed(message.breed);

            /* Check if breed is available */
            if (!client.Account.CanUseBreed(message.breed) || !BreedManager.AvailableBreeds.Contains(breed.Id))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NOT_ALLOWED));
                return;
            }

            /* Parse character colors */
            var indexedColors = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                int color = message.colors[i];

                if (color == -1)
                    color = (int)(!message.sex ? breed.MaleColors[i] : breed.FemaleColors[i]);

                indexedColors.Add(int.Parse((i + 1) + color.ToString("X6"), NumberStyles.HexNumber));
            }

            var breedLook = !message.sex ? breed.MaleLook.Copy() : breed.FemaleLook.Copy();
            breedLook.indexedColors = indexedColors;


            /* Create Character */
            var character = new CharacterRecord
            {
                Experience = ThresholdManager.Thresholds["CharacterExp"].GetLowerBound((uint)breed.StartLevel),
                Name = characterName,
                Breed = message.breed,
                Sex = message.sex ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE,
                BaseLook = breedLook,
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
                Agility = 0,
            };


            /* Set Character Inventory */
            // TODO ADD START OBJECTS
            character.Inventory = new InventoryRecord { Kamas = (uint)breed.StartKamas };
            character.Inventory.SaveAndFlush();

            /* Set Character SpellCollection */
            foreach (SpellIdEnum spellId in breed.StartSpells.Keys)
            {
                int position = breed.StartSpells[spellId];
                character.Spells.Add(new SpellRecord { SpellId = (uint)spellId, Position = position, Level = 1 });
            }

            /* Save it */
            CharacterManager.CreateCharacterOnAccount(character, client);


            BasicHandler.SendBasicNoOperationMessage(client);
            client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.OK));
            SendCharactersListMessage(client);
        }

        [WorldHandler(typeof(CharacterNameSuggestionRequestMessage))]
        public static void HandleCharacterNameSuggestionRequestMessage(WorldClient client, CharacterNameSuggestionRequestMessage message)
        {
            if (!EnableNameSuggestion)
            {
                client.Send(new CharacterNameSuggestionFailureMessage((uint)NicknameGeneratingFailureEnum.NICKNAME_GENERATOR_UNAVAILABLE));
                return;
            }

            string generatedName = CharacterManager.GenerateName();

            client.Send(new CharacterNameSuggestionSuccessMessage(generatedName));
        }
    }
}