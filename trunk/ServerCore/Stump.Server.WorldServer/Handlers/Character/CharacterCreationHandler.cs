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
using System.Linq;
using System.Text.RegularExpressions;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Utils;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.DataProvider.Data.Breeds;
using Stump.Server.DataProvider.Data.Threshold;
using Stump.Server.WorldServer.Helpers;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        /// <summary>
        /// Enable or disable random name suggestion when creation a character
        /// </summary>
        [Variable]
        public static bool EnableNameSuggestion = true;

        /// <summary>
        ///   Maximum number of characters you can create/store in your account
        /// </summary>
        [Variable]
        public static int MaxCharacterSlot = 5;

        /// <summary>
        /// List of available breeds
        /// </summary>
        [Variable]
        public static List<PlayableBreedEnum> AvailableBreeds = new List<PlayableBreedEnum>
            {
                PlayableBreedEnum.Feca,
                PlayableBreedEnum.Osamodas,
                PlayableBreedEnum.Enutrof,
                PlayableBreedEnum.Sram,
                PlayableBreedEnum.Xelor,
                PlayableBreedEnum.Ecaflip,
                PlayableBreedEnum.Eniripsa,
                PlayableBreedEnum.Iop,
                PlayableBreedEnum.Cra,
                PlayableBreedEnum.Sadida,
                PlayableBreedEnum.Sacrieur,
                PlayableBreedEnum.Pandawa,
                PlayableBreedEnum.Roublard,
                //BreedEnum.Zobal,
            };

        [WorldHandler(typeof(CharacterCreationRequestMessage))]
        public static void HandleCharacterCreationRequestMessage(WorldClient client, CharacterCreationRequestMessage message)
        {
            /* Check if we can create characters on this server */
            if (client.Characters.Count >= MaxCharacterSlot)
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

            var characterName = StringUtils.FirstLetterUpper(message.name.ToLower());

            /* Check is name is well formatted */
            if (!Regex.IsMatch(characterName, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_INVALID_NAME));
                return;
            }

            /* Get character Breed */
            var breedTemplate = BreedTemplateManager.Instance.Get((PlayableBreedEnum)message.breed);

            /* Check if breed is available */
            if (!client.Account.CanUseBreed(message.breed) || !AvailableBreeds.Contains(breedTemplate.Id))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NOT_ALLOWED));
                return;
            }

            /* Parse character colors */
            var indexedColors = new List<int>(5);
            for (int i = 0; i < 5; i++)
            {
                var color = message.colors[i];

                if (color == -1)
                    color = (int)(!message.sex ? breedTemplate.MaleColors[i] : breedTemplate.FemaleColors[i]);

                indexedColors.Add(int.Parse((i + 1) + color.ToString("X6"), NumberStyles.HexNumber));
            }

            var breedLook = !message.sex ? breedTemplate.MaleLook.Copy() : breedTemplate.FemaleLook.Copy();
            breedLook.indexedColors = indexedColors;

            /* Create Inventory */
            var inventory = new InventoryRecord();
            inventory.Create();

            /* Create Character */
            var character = new CharacterRecord
            {
                Experience = ThresholdManager.Instance["CharacterExp"].GetLowerBound(breedTemplate.StartLevel),
                Name = characterName,
                Breed = message.breed,
                Sex = message.sex ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE,
                BaseLook = breedLook,
                MapId = breedTemplate.StartMap,
                CellId = breedTemplate.StartCell,
                BaseHealth = breedTemplate.StartHealthPoint,
                DamageTaken = 0,
                StatsPoints = breedTemplate.StartStatsPoints,
                SpellsPoints = breedTemplate.StartSpellsPoints,
                Strength = breedTemplate.StartStrength,
                Vitality = breedTemplate.StartVitality,
                Wisdom = breedTemplate.StartWisdom,
                Intelligence = breedTemplate.StartIntelligence,
                Chance = breedTemplate.StartChance,
                Agility = breedTemplate.StartAgility,
                Inventory = inventory
            };
            character.Create();

            /* Set Character SpellCollection */
            var startPos = 64;
            foreach (var spellTemplate in breedTemplate.LearnableSpells.Where(s => s.ObtainLevel == 1))
            {
                var spell = new SpellRecord { SpellId = (uint)spellTemplate.Id, Character = character, Position = startPos++, Level = 1 };
                spell.Create();
                character.Spells.Add(spell);
            }

            /* Save it */
            AccountManager.AddCharacterOnAccount(character, client);

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
            var generatedName = CharacterManager.GetRandomName();

            client.Send(new CharacterNameSuggestionSuccessMessage(generatedName));
        }
    }
}