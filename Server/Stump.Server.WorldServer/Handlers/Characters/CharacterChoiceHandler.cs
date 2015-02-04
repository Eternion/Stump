using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Chat;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Guilds;
using Stump.Server.WorldServer.Handlers.Friends;
using Stump.Server.WorldServer.Handlers.Initialization;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Handlers.Mounts;
using Stump.Server.WorldServer.Handlers.PvP;
using Stump.Server.WorldServer.Handlers.Shortcuts;
using Stump.Server.WorldServer.Handlers.Startup;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler
    {
        [WorldHandler(CharacterFirstSelectionMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterFirstSelectionMessage(WorldClient client, CharacterFirstSelectionMessage message)
        {
            // TODO ADD TUTORIAL EFFECTS
            HandleCharacterSelectionMessage(client, message);
        }

        [WorldHandler(CharacterSelectionMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterSelectionMessage(WorldClient client, CharacterSelectionMessage message)
        {
            var character = client.Characters.First(entry => entry.Id == message.id);

            /* Check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            /* Common selection */
            CommonCharacterSelection(client, character);
        }

        [WorldHandler(CharacterSelectionWithRecolorMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterSelectionWithRecolorMessage(WorldClient client, CharacterSelectionWithRecolorMessage message)
        {
            var character = client.Characters.First(entry => entry.Id == message.id);

            /* Check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            if (character.Recolor)
            {
                /* Get character Breed */
                var breed = BreedManager.Instance.GetBreed((int)character.Breed);

                if (breed == null)
                {
                    client.Send(new CharacterSelectedErrorMessage());
                    return;
                }

                /* Set Colors */
                var breedColors = character.Sex == SexTypeEnum.SEX_MALE ? breed.MaleColors : breed.FemaleColors;

                character.EntityLook.SetColors(
                    message.indexedColor.Select((x, i) => x == -1 ? Color.FromArgb((int)breedColors[i]) : Color.FromArgb(x)).ToArray());

                character.Recolor = false;

                WorldServer.Instance.DBAccessor.Database.Update(character);
            }

            /* Common selection */
            CommonCharacterSelection(client, character);
        }

        [WorldHandler(CharacterSelectionWithRelookMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterSelectionWithRelookMessage(WorldClient client, CharacterSelectionWithRelookMessage message)
        {
            var character = client.Characters.First(entry => entry.Id == message.id);

            /* Check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            if (character.Relook > 0)
            {
                if (character.Relook == 2)
                    character.Sex = character.Sex == SexTypeEnum.SEX_MALE ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE;

                /* Set Look */
                var head = BreedManager.Instance.GetHead(message.cosmeticId);

                if (head.Breed != (int)character.Breed || head.Gender != (int)character.Sex)
                {
                    client.Send(new CharacterSelectedErrorMessage());
                    return;
                }

                character.Head = head.Id;
                character.Relook = 0;

                WorldServer.Instance.DBAccessor.Database.Update(character);
            }

            /* Common selection */
            CommonCharacterSelection(client, character);
        }

        [WorldHandler(CharacterSelectionWithRenameMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterSelectionWithRenameMessage(WorldClient client, CharacterSelectionWithRenameMessage message)
        {
            var character = client.Characters.First(entry => entry.Id == message.id);

            /* Check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            if (character.Rename)
            {
                /* Check if name is valid */
                if (!Regex.IsMatch(message.name, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
                {
                    client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_INVALID_NAME));
                    return;
                }

                /* Check if name is free */
                if (CharacterManager.Instance.DoesNameExist(message.name))
                {
                    client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                    return;
                }

                /* Set new name */
                character.Name = message.name;
                character.Rename = false;

                WorldServer.Instance.DBAccessor.Database.Update(character);
            }

            /* Common selection */
            CommonCharacterSelection(client, character);
        }

        public static void CommonCharacterSelection(WorldClient client, CharacterRecord character)
        {
            // Check if we also have a world account
            if (client.WorldAccount == null)
            {
                var account = AccountManager.Instance.FindById(client.Account.Id) ??
                              AccountManager.Instance.CreateWorldAccount(client);
                client.WorldAccount = account;
            }

            client.Character = new Character(character, client);

            SendCharacterSelectedSuccessMessage(client);

            ContextHandler.SendNotificationListMessage(client, new[] { 0x7FFFFFFF });


            InventoryHandler.SendInventoryContentMessage(client);

            ShortcutHandler.SendShortcutBarContentMessage(client, ShortcutBarEnum.GENERAL_SHORTCUT_BAR);
            ShortcutHandler.SendShortcutBarContentMessage(client, ShortcutBarEnum.SPELL_SHORTCUT_BAR);
            //ContextHandler.SendSpellForgottenMessage(client);

            ContextRoleplayHandler.SendEmoteListMessage(client, Enumerable.Range(0, 21).Select(entry => (sbyte)entry).ToList());
            ChatHandler.SendEnabledChannelsMessage(client, new sbyte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13 }, new sbyte[] {});

            PvPHandler.SendAlignmentRankUpdateMessage(client);
            PvPHandler.SendAlignmentSubAreasListMessage(client);

            InventoryHandler.SendSpellListMessage(client, true);
            
            InitializationHandler.SendSetCharacterRestrictionsMessage(client);

            InventoryHandler.SendInventoryWeightMessage(client);

            //Guild
            if (client.Character.GuildMember != null)
                GuildHandler.SendGuildMembershipMessage(client,client.Character.GuildMember);

            //Mount
            if (client.Character.Mount != null)
            {
                MountHandler.SendMountSetMessage(client, client.Character.Mount.GetMountClientData());
                MountHandler.SendMountXpRatioMessage(client, client.Character.Mount.GivenExperience);
            }

            FriendHandler.SendFriendWarnOnConnectionStateMessage(client, client.Character.FriendsBook.WarnOnConnection);
            FriendHandler.SendFriendWarnOnLevelGainStateMessage(client, client.Character.FriendsBook.WarnOnLevel);
            GuildHandler.SendGuildMemberWarnOnConnectionStateMessage(client, client.Character.WarnOnGuildConnection);

            client.Character.SendConnectionMessages();

            //InitializationHandler.SendOnConnectionEventMessage(client, 3);

            //Start Cinematic(Doesn't work for now)
            if (client.Character.Record.LastUsage == null)
                BasicHandler.SendCinematicMessage(client, 10);

            ContextRoleplayHandler.SendGameRolePlayArenaUpdatePlayerInfosMessage(client, client.Character);

            SendCharacterCapabilitiesMessage(client);

            // Update LastConnection and Last Ip
            client.WorldAccount.LastConnection = DateTime.Now;
            client.WorldAccount.LastIp = client.IP;
            client.WorldAccount.ConnectedCharacter = character.Id;
            WorldServer.Instance.DBAccessor.Database.Update(client.WorldAccount);

            character.LastUsage = DateTime.Now;
            WorldServer.Instance.DBAccessor.Database.Update(character);
        }


        [WorldHandler(CharactersListRequestMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterListRequest(WorldClient client, CharactersListRequestMessage message)
        {
            if (client.Account != null && client.Account.Login != "")
            {
                SendCharactersListWithModificationsMessage(client);


                if (client.WorldAccount != null && client.StartupActions.Count > 0)
                {
                    StartupHandler.SendStartupActionsListMessage(client, client.StartupActions);
                }
            }
            else
            {
                client.Send(new IdentificationFailedMessage((int) IdentificationFailureReasonEnum.KICKED));
                client.DisconnectLater(1000);
            }
        }

        public static void SendCharactersListMessage(WorldClient client)
        {
            var characters = client.Characters.OrderByDescending(x => x.LastUsage).Select(
                characterRecord =>
                new CharacterBaseInformations(
                    characterRecord.Id,
                    ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience, characterRecord.PrestigeRank),
                    characterRecord.Name,
                    characterRecord.EntityLook.GetEntityLook(),
                    (sbyte) characterRecord.Breed,
                    characterRecord.Sex != SexTypeEnum.SEX_MALE)).ToList();

            client.Send(new CharactersListMessage(
                            false,
                            characters
                            ));
        }

        public static void SendCharactersListWithModificationsMessage(WorldClient client)
        {
            var characterBaseInformations = new List<CharacterBaseInformations>();
            var charactersToRecolor = new List<CharacterToRecolorInformation>();
            var charactersToRelook = new List<CharacterToRelookInformation>();
            var charactersToRename = new List<int>();
            var unusableCharacters = new List<int>();

            foreach (var characterRecord in client.Characters.OrderByDescending(x => x.LastUsage))
            {
                characterBaseInformations.Add(new CharacterBaseInformations(characterRecord.Id,
                                                                            ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience, characterRecord.PrestigeRank),
                                                                            characterRecord.Name, characterRecord.EntityLook.GetEntityLook(),
                                                                            (sbyte) characterRecord.Breed,
                                                                            characterRecord.Relook == 2 ? characterRecord.Sex == SexTypeEnum.SEX_MALE : characterRecord.Sex != SexTypeEnum.SEX_MALE));

                if (characterRecord.Rename)
                {
                    charactersToRename.Add(characterRecord.Id);
                }

                if (characterRecord.Recolor)
                {
                    charactersToRecolor.Add(new CharacterToRecolorInformation(characterRecord.Id, characterRecord.EntityLook.GetEntityLook().indexedColors));
                }

                if (characterRecord.Relook > 0)
                {
                    charactersToRelook.Add(new CharacterToRelookInformation(characterRecord.Id, characterRecord.Head));
                }

                /*if (!(characterRecord.Recolor && characterRecord.Rename))
                {
                    unusableCharacters.Add(characterRecord.Id);
                }*/
            }
            client.Send(new CharactersListWithModificationsMessage(false,
                                                                   characterBaseInformations,
                                                                   charactersToRecolor,
                                                                   charactersToRename,
                                                                   unusableCharacters,
                                                                   charactersToRelook));
        }

        public static void SendCharacterSelectedSuccessMessage(WorldClient client)
        {
            client.Send(new CharacterSelectedSuccessMessage(client.Character.GetCharacterBaseInformations()));
        }

        public static void SendCharacterCapabilitiesMessage(WorldClient client)
        {
            client.Send(new CharacterCapabilitiesMessage(4095));
        }
    }
}