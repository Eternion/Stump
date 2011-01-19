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
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Breeds;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Manager;
using Stump.Server.WorldServer.Threshold;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
<<<<<<< HEAD
        [Variable]
        public static bool EnableNameSuggestion = true;

        [WorldHandler(typeof(CharacterCreationRequestMessage))]
        public static void HandleCharacterCreationRequestMessage(WorldClient client, CharacterCreationRequestMessage message)
=======
        [WorldHandler(typeof (CharacterCreationRequestMessage))]
        public static void HandleCharacterCreateRequest(WorldClient client, CharacterCreationRequestMessage message)
>>>>>>> 23e9354153cb54a4bfe880de59e0d30f32883ff8
        {
            /* Check if we can create characters on this server */
            if (client.Characters.Count >= World.MaxCharacterSlot)
            {
                client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS));
                return;
            }

            /* Check if name is free */
            if (CharacterRecord.IsNameExists(message.name))
            {
                client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }

            string characterName = StringUtils.FirstLetterUpper(message.name.ToLower());

<<<<<<< HEAD
            /* Check name */
            if (!Regex.IsMatch(characterName, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
=======
            /* have bad name */
            if (
                !Regex.IsMatch(characterName, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
>>>>>>> 23e9354153cb54a4bfe880de59e0d30f32883ff8
            {
                client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.ERR_INVALID_NAME));
                return;
            }

            /* Get character Breed */
            BaseBreed breed = BreedManager.GetBreed(message.breed);

            /* Check if breed is available */
            if (!client.Account.CanUseBreed(message.breed) || !BreedManager.AvailableBreeds.Contains(breed.Id))
            {
                client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.ERR_NOT_ALLOWED));
                return;
            }

            /* Parse character colors */
            var indexedColors = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                int color = message.colors[i];

                if (color == -1)
<<<<<<< HEAD
                    color = (int)(!message.sex ? breed.MaleColors[i] : breed.FemaleColors[i]);
=======
                    color = (int) (!message.sex ? breed.MaleColors[i] : breed.FemaleColors[i]);
>>>>>>> 23e9354153cb54a4bfe880de59e0d30f32883ff8

                indexedColors.Add(int.Parse((i + 1) + color.ToString("X6"), NumberStyles.HexNumber));
            }

            EntityLook breedLook = !message.sex ? breed.MaleLook.Copy() : breed.FemaleLook.Copy();
            breedLook.indexedColors = indexedColors;


            /* Create Character */
            var character = new CharacterRecord
<<<<<<< HEAD
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
=======
                            {
                                New = true,
                                Account = client.Account,
                                Level = breed.StartLevel,
                                Name = characterName,
                                Breed = message.breed,
                                SexId = message.sex ? 1 : 0,
                                BaseLook = breedLook,
                                Kamas = breed.StartKamas,
                                MapId = (int) breed.StartMap,
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
>>>>>>> 23e9354153cb54a4bfe880de59e0d30f32883ff8
            foreach (SpellIdEnum spellId in breed.StartSpells.Keys)
            {
                int position = breed.StartSpells[spellId];
                character.Spells.Add(new SpellRecord { SpellId = (uint)spellId, Position = position, Level = 1 });
            }

            /* Save it */
            CharacterManager.CreateCharacterOnAccount(character, client);


            BasicHandler.SendBasicNoOperationMessage(client);
            client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.OK));
            SendCharactersListMessage(client);
        }

<<<<<<< HEAD
        [WorldHandler(typeof(CharacterNameSuggestionRequestMessage))]
        public static void HandleCharacterNameSuggestionRequestMessage(WorldClient client, CharacterNameSuggestionRequestMessage message)
=======

        [WorldHandler(typeof (CharacterNameSuggestionRequestMessage))]
        public static void CharacterNameSuggestionRequest(WorldClient client,
                                                          CharacterNameSuggestionRequestMessage message)
>>>>>>> 23e9354153cb54a4bfe880de59e0d30f32883ff8
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