using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Chat;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Guilds;
using Stump.Server.WorldServer.Handlers.Friends;
using Stump.Server.WorldServer.Handlers.Initialization;
using Stump.Server.WorldServer.Handlers.Inventory;
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

        [WorldHandler(CharacterSelectionWithRecolorMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
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
            var indexes = message.indexedColor.Select(x => x >> 24).ToArray();
            var colors = message.indexedColor.Select(x => Color.FromArgb(x | 0xFFFFFF)).ToArray();

            character.EntityLook.SetColors(indexes, colors);
            WorldServer.Instance.DBAccessor.Database.Update(character);

            /* Common selection */
            CommonCharacterSelection(client, character);
        }

        [WorldHandler(CharacterSelectionWithRenameMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
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
            if (CharacterManager.Instance.DoesNameExist(message.name) || !Regex.IsMatch(message.name.ToLower().FirstLetterUpper(),
                                                                               "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
            {
                client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }

            /* Set new name */
            character.Name = message.name.ToLower().FirstLetterUpper();
            WorldServer.Instance.DBAccessor.Database.Update(character);

            /* Common selection */
            CommonCharacterSelection(client, character);
        }

        public static void CommonCharacterSelection(WorldClient client, CharacterRecord character)
        {            
            
            // Check if we also have a world account
            if (client.WorldAccount == null)
            {
                var account = AccountManager.Instance.FindById(client.Account.Id);
                if (account == null)
                    account = AccountManager.Instance.CreateWorldAccount(client);
                client.WorldAccount = account;
            }

            client.Character = new Character(character, client);

            SendCharacterSelectedSuccessMessage(client);

            ContextRoleplayHandler.SendGameRolePlayArenaUpdatePlayerInfosMessage(client);
            ContextHandler.SendNotificationListMessage(client, new[] { 0x7FFFFFFF });


            InventoryHandler.SendInventoryContentMessage(client);

            ShortcutHandler.SendShortcutBarContentMessage(client, ShortcutBarEnum.GENERAL_SHORTCUT_BAR);
            ShortcutHandler.SendShortcutBarContentMessage(client, ShortcutBarEnum.SPELL_SHORTCUT_BAR);
            //ContextHandler.SendSpellForgottenMessage(client);

            ContextRoleplayHandler.SendEmoteListMessage(client, Enumerable.Range(0, 21).Select(entry => (sbyte)entry).ToList()));
            ChatHandler.SendEnabledChannelsMessage(client, new sbyte[] { 0, 1, 2, 3, 4, 5, 6, 7, 12, 13, 9, 10 }, new sbyte[] {8, 7});

            PvPHandler.SendAlignmentRankUpdateMessage(client);
            PvPHandler.SendAlignmentSubAreasListMessage(client);

            InventoryHandler.SendSpellListMessage(client, true);
            
            InitializationHandler.SendSetCharacterRestrictionsMessage(client);

            InventoryHandler.SendInventoryWeightMessage(client);

            //Guild
            if (client.Character.GuildMember != null)
                GuildHandler.SendGuildMembershipMessage(client,client.Character.GuildMember);

            FriendHandler.SendFriendWarnOnConnectionStateMessage(client, client.Character.FriendsBook.WarnOnConnection);
            FriendHandler.SendFriendWarnOnLevelGainStateMessage(client, client.Character.FriendsBook.WarnOnLevel);
            GuildHandler.SendGuildMemberWarnOnConnectionStateMessage(client, client.Character.WarnOnGuildConnection);

            client.Character.SendConnectionMessages();


            //InitializationHandler.SendOnConnectionEventMessage(client, 2);

            ContextRoleplayHandler.SendGameRolePlayArenaUpdatePlayerInfosMessage(client);

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
                SendCharactersListMessage(client);


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
            List<CharacterBaseInformations> characters = client.Characters.Select(
                characterRecord =>
                new CharacterBaseInformations(
                    characterRecord.Id,
                    ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience),
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

            foreach (CharacterRecord characterRecord in client.Characters)
            {
                characterBaseInformations.Add(new CharacterBaseInformations(characterRecord.Id,
                                                                            ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience),
                                                                            characterRecord.Name, characterRecord.EntityLook.GetEntityLook(),
                                                                            (sbyte) characterRecord.Breed, characterRecord.Sex != SexTypeEnum.SEX_MALE));

                if (characterRecord.Rename)
                {
                    charactersToRename.Add(characterRecord.Id);
                }

                if (characterRecord.Recolor)
                {
                    charactersToRecolor.Add(new CharacterToRecolorInformation(characterRecord.Id, characterRecord.EntityLook.GetEntityLook().indexedColors));
                }

                if (characterRecord.Relook)
                {
                    charactersToRelook.Add(new CharacterToRelookInformation(characterRecord.Id, characterRecord.Head));
                }

                if (!(characterRecord.Recolor && characterRecord.Rename))
                {
                    unusableCharacters.Add(characterRecord.Id);
                }
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