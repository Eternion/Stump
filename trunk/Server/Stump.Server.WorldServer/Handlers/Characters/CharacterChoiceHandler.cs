using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.World.Actors.RolePlay;
using Stump.Server.WorldServer.World.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler
    {
        [WorldHandler(CharacterFirstSelectionMessage.Id)]
        public static void HandleCharacterFirstSelectionMessage(WorldClient client, CharacterFirstSelectionMessage message)
        {
            // TODO ADD TUTORIAL EFFECTS
            HandleCharacterSelectionMessage(client, message);
        }

        [WorldHandler(CharacterSelectionMessage.Id)]
        public static void HandleCharacterSelectionMessage(WorldClient client, CharacterSelectionMessage message)
        {
            CharacterRecord character = client.Characters.First(entry => entry.Id == message.id);

            /* Check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            /* Common selection */
            CommonCharacterSelection(client, character);
        }

        [WorldHandler(CharacterSelectionWithRecolorMessage.Id)]
        public static void HandleCharacterSelectionWithRecolorMessage(WorldClient client, CharacterSelectionWithRecolorMessage message)
        {
            CharacterRecord character = client.Characters.First(entry => entry.Id == message.id);

            /* Check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            /* Set Colors */
            character.EntityLook.indexedColors = message.indexedColor;
            character.Save();

            /* Common selection */
            CommonCharacterSelection(client, character);
        }

        [WorldHandler(CharacterSelectionWithRenameMessage.Id)]
        public static void HandleCharacterSelectionWithRenameMessage(WorldClient client, CharacterSelectionWithRenameMessage message)
        {
            CharacterRecord character = client.Characters.First(entry => entry.Id == message.id);

            /* Check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            /* Check if name is free */
            if (CharacterRecord.DoesNameExists(message.name) || !Regex.IsMatch(message.name.ToLower().FirstLetterUpper(),
                                                                               "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
            {
                client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }

            /* Set new name */
            character.Name = message.name.ToLower().FirstLetterUpper();
            character.Save();

            /* Common selection */
            CommonCharacterSelection(client, character);
        }

        public static void CommonCharacterSelection(WorldClient client, CharacterRecord character)
        {
            client.ActiveCharacter = new Character(character, client);

            /*// Check if we also have a world account
            if (client.WorldAccount == null)
            {
                if (!WorldAccountRecord.Exists(client.Account.Id))
                    client.WorldAccount = AccountManager.CreateWorldAccount(client);
                else
                    client.WorldAccount = WorldAccountRecord.FindWorldAccountById(client.Account.Id);
            }

            // Update LastConnection and Last Ip
            client.WorldAccount.LastConnection = DateTime.Now;
            client.WorldAccount.LastIp = client.IP;
            client.WorldAccount.Update();

            SendCharacterSelectedSuccessMessage(client);

            InventoryHandler.SendInventoryContentMessage(client);
            InventoryHandler.SendInventoryWeightMessage(client);

            InventoryHandler.SendSpellListMessage(client, true);
            ContextHandler.SendSpellForgottenMessage(client);
            ContextHandler.SendNotificationListMessage(client, new List<int>());

            ContextHandler.SendEmoteListMessage(client, new List<uint>());
            ChatHandler.SendEnabledChannelsMessage(client, new List<uint>(), new List<uint>());

            PvpHandler.SendAlignmentRankUpdateMessage(client);
            PvpHandler.SendAlignmentSubAreasListMessage(client);

            InitializationHandler.SendSetCharacterRestrictionsMessage(client);

            BasicHandler.SendTextInformationMessage(client, 1, 89);
            BasicHandler.SendTextInformationMessage(client, 0, 152,
                                                    client.Account.LastConnection.Year,
                                                    client.Account.LastConnection.Month,
                                                    client.Account.LastConnection.Day,
                                                    client.Account.LastConnection.Hour,
                                                    client.Account.LastConnection.Minute,
                                                    client.Account.LastIp ?? "(null)");

            InitializationHandler.SendOnConnectionEventMessage(client, 2);*/
        }

        [WorldHandler(CharactersListRequestMessage.Id)]
        public static void HandleCharacterListRequest(WorldClient client, CharactersListRequestMessage message)
        {
            if (client.Account != null && client.Account.Login != "")
            {
                SendCharactersListMessage(client);
            }
            else
            {
                client.Send(new IdentificationFailedMessage((int) IdentificationFailureReasonEnum.KICKED));
                client.DisconnectLater(1000);
            }
        }

        public static void SendCharactersListMessage(WorldClient client)
        {
            var characters = client.Characters.Select(
                characterRecord =>
                new CharacterBaseInformations(
                    characterRecord.Id,
                    ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience),
                    characterRecord.Name,
                    characterRecord.EntityLook,
                    (byte) characterRecord.Breed,
                    characterRecord.Sex != SexTypeEnum.SEX_MALE));

            client.Send(new CharactersListMessage(
                            true, //client.Account.StartupActions.Count != 0,
                            characters
                            ));
        }

        public static void SendCharactersListWithModificationsMessage(WorldClient client)
        {
            var characterBaseInformations = new List<CharacterBaseInformations>();
            var charactersToRecolor = new List<CharacterToRecolorInformation>();
            var charactersToRename = new List<int>();
            var unusableCharacters = new List<int>();

            foreach (CharacterRecord characterRecord in client.Characters)
            {
                characterBaseInformations.Add(new CharacterBaseInformations(characterRecord.Id,
                                                                            ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience),
                                                                            characterRecord.Name, characterRecord.EntityLook,
                                                                            (byte) characterRecord.Breed, characterRecord.Sex != SexTypeEnum.SEX_MALE));

                if (characterRecord.Rename)
                {
                    charactersToRename.Add(characterRecord.Id);
                }

                if (characterRecord.Recolor)
                {
                    charactersToRecolor.Add(new CharacterToRecolorInformation(characterRecord.Id, characterRecord.EntityLook.indexedColors));
                }

                if (!(characterRecord.Recolor && characterRecord.Rename))
                {
                    unusableCharacters.Add(characterRecord.Id);
                }
            }
            client.Send(new CharactersListWithModificationsMessage(false, //client.Account.StartupActions.Count != 0,
                                                                   characterBaseInformations,
                                                                   charactersToRecolor,
                                                                   charactersToRename,
                                                                   unusableCharacters));
        }

        public static void SendCharacterSelectedSuccessMessage(WorldClient client)
        {
            client.Send(new CharacterSelectedSuccessMessage(client.ActiveCharacter.GetCharacterBaseInformations()));
        }
    }
}