using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Breeds;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        [Variable]
        public static bool EnableNameSuggestion = true;

        /// <summary>
        ///   Maximum number of characters you can create/store in your account
        /// </summary>
        [Variable]
        public static uint MaxCharacterSlot = 5;

        [WorldHandler(CharacterCreationRequestMessage.Id)]
        public static void HandleCharacterCreationRequestMessage(WorldClient client, CharacterCreationRequestMessage message)
        {
            /* Check if we can create characters on this server */
            if (client.Characters.Count >= MaxCharacterSlot)
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS));
                return;
            }

            /* Check if name is free */
            if (CharacterRecord.DoesNameExists(message.name))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }

            string characterName = message.name.ToLower().FirstLetterUpper();

            /* Check is name is well formatted */
            if (!Regex.IsMatch(characterName, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_INVALID_NAME));
                return;
            }

            /* Get character Breed */
            Breed breed = BreedManager.Instance.GetBreed(message.breed);

            /* Check if breed is available */
            if (breed == null || !client.Account.CanUseBreed(message.breed) || !BreedManager.Instance.IsBreedAvailable(breed.Id))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NOT_ALLOWED));
                return;
            }

            /* Parse character colors */
            var indexedColors = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                int color = message.colors.ElementAt(i);

                if (color == -1)
                    color = (int)(!message.sex ? breed.MaleColors[i] : breed.FemaleColors[i]);

                indexedColors.Add((i + 1) << 24 | color);
            }

            var breedLook = !message.sex ? breed.MaleLook.Copy() : breed.FemaleLook.Copy();
            breedLook.indexedColors = indexedColors;


            /* Create Character */
            var character = new CharacterRecord(breed)
            {
                Experience = 0,//ThresholdManager.Thresholds["CharacterExp"].GetLowerBound((uint)breed.StartLevel),
                Name = characterName,
                Sex = message.sex ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE,
                EntityLook = breedLook,
            };

            /* Create Inventory */
            // TODO ADD START OBJECTS
            var inventory = new InventoryRecord(character);

            /* Set Character SpellCollection */
            /*foreach (SpellIdEnum spellId in breed.StartSpells.Keys)
            {
                var spell = new SpellRecord { SpellId = (uint)spellId, Position = breed.StartSpells[spellId], Level = 1 };
                spell.Create();
                character.Spells.Add(spell);
            }*/


            /* Save it */
            CharacterManager.Instance.CreateCharacterOnAccount(character, client);


            character.Inventory = inventory;
            inventory.Create();

            BasicHandler.SendBasicNoOperationMessage(client);
            client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.OK));
            SendCharactersListMessage(client);
        }

        [WorldHandler(CharacterNameSuggestionRequestMessage.Id)]
        public static void HandleCharacterNameSuggestionRequestMessage(WorldClient client, CharacterNameSuggestionRequestMessage message)
        {
            if (!EnableNameSuggestion)
            {
                client.Send(new CharacterNameSuggestionFailureMessage((sbyte)NicknameGeneratingFailureEnum.NICKNAME_GENERATOR_UNAVAILABLE));
                return;
            }

            string generatedName = CharacterManager.Instance.GenerateName();

            client.Send(new CharacterNameSuggestionSuccessMessage(generatedName));
        }
    }
}