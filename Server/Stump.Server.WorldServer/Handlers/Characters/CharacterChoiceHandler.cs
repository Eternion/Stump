using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Logging;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Game.Fights;
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

        [WorldHandler(CharacterSelectionWithRemodelMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterSelectionWithRemodelMessage(WorldClient client, CharacterSelectionWithRemodelMessage message)
        {
            var character = client.Characters.First(entry => entry.Id == message.id);

            /* Check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            var remodel = message.remodel;

            if (((character.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME)
                || ((character.PossibleChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME))
            {
                /* Check if name is valid */
                if (!Regex.IsMatch(remodel.name, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
                {
                    client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_INVALID_NAME));
                    return;
                }

                /* Check if name is free */
                if (CharacterManager.Instance.DoesNameExist(remodel.name))
                {
                    client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                    return;
                }

                /* Set new name */
                character.Name = remodel.name;
            }

            if (((character.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER)
                || ((character.PossibleChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER))
            {
                character.Sex = remodel.sex ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE;
            }

            if (((character.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED)
                || ((character.PossibleChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED))
            {
                client.Character = new Character(character, client);

                BreedManager.Instance.ChangeBreed(client.Character, (PlayableBreedEnum)remodel.breed);
                client.Character.SaveNow();

                character = client.Character.Record;
                client.Character = null;
            }

            if (((character.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC)
                || ((character.PossibleChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC))
            {
                /* Get Head */
                var head = BreedManager.Instance.GetHead(remodel.cosmeticId);
                /* Get character Breed */
                var breed = BreedManager.Instance.GetBreed((int)character.Breed);

                if (breed == null || head.Breed != (int)character.Breed || head.Gender != (int)character.Sex)
                {
                    client.Send(new CharacterSelectedErrorMessage());
                    return;
                }

                character.Head = head.Id;

                foreach (var scale in character.Sex == SexTypeEnum.SEX_MALE ? breed.MaleLook.Scales : breed.FemaleLook.Scales)
                    character.EntityLook.SetScales(scale);
            }

            if (((character.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS)
                || ((character.PossibleChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS))
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
                var m_colors = new Dictionary<int, Color>();

                foreach (var color in remodel.colors)
                {
                    var index = color >> 24;
                    var c = Color.FromArgb(color);

                    m_colors.Add(index, c);
                }

                var i = 0;
                foreach (var breedColor in breedColors)
                {
                    if (!m_colors.ContainsKey(i))
                        m_colors.Add(i, Color.FromArgb((int)breedColor));

                    i++;
                }

                character.EntityLook.SetColors(m_colors.Select(x => x.Value).ToArray());
            }

            character.MandatoryChanges = 0;
            character.PossibleChanges = 0;

            WorldServer.Instance.DBAccessor.Database.Update(character);

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

            if (client.Character.Inventory.Presets.Any())
                InventoryHandler.SendInventoryContentAndPresetMessage(client);
            else
                InventoryHandler.SendInventoryContentMessage(client);

            ShortcutHandler.SendShortcutBarContentMessage(client, ShortcutBarEnum.GENERAL_SHORTCUT_BAR);
            ShortcutHandler.SendShortcutBarContentMessage(client, ShortcutBarEnum.SPELL_SHORTCUT_BAR);
            //ContextHandler.SendSpellForgottenMessage(client);

            ContextRoleplayHandler.SendEmoteListMessage(client, client.Character.AvailableEmotes.Select(x => (byte)x));
            ChatHandler.SendEnabledChannelsMessage(client, new sbyte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13 }, new sbyte[] { });

            PvPHandler.SendAlignmentRankUpdateMessage(client);

            InventoryHandler.SendSpellListMessage(client, true);

            InitializationHandler.SendSetCharacterRestrictionsMessage(client, client.Character);

            InventoryHandler.SendInventoryWeightMessage(client);

            FriendHandler.SendFriendWarnOnConnectionStateMessage(client, client.Character.FriendsBook.WarnOnConnection);
            FriendHandler.SendFriendWarnOnLevelGainStateMessage(client, client.Character.FriendsBook.WarnOnLevel);
            GuildHandler.SendGuildMemberWarnOnConnectionStateMessage(client, client.Character.WarnOnGuildConnection);

            //Guild
            if (client.Character.GuildMember != null)
                GuildHandler.SendGuildMembershipMessage(client, client.Character.GuildMember);

            //Mount
            if (client.Character.Mount != null)
            {
                MountHandler.SendMountSetMessage(client, client.Character.Mount.GetMountClientData());
                MountHandler.SendMountXpRatioMessage(client, client.Character.Mount.GivenExperience);
            }

            client.Character.SendConnectionMessages();

            //InitializationHandler.SendOnConnectionEventMessage(client, 3);

            //Start Cinematic(Doesn't work for now)
            if ((DateTime.Now - client.Character.Record.CreationDate).TotalSeconds <= 30)
                BasicHandler.SendCinematicMessage(client, 10);

            ContextRoleplayHandler.SendGameRolePlayArenaUpdatePlayerInfosMessage(client, client.Character);

            SendCharacterCapabilitiesMessage(client);

            //Loading complete
            SendCharacterLoadingCompleteMessage(client);

            // Update LastConnection and Last Ip
            client.WorldAccount.LastConnection = DateTime.Now;
            client.WorldAccount.LastIp = client.IP;
            client.WorldAccount.ConnectedCharacter = character.Id;
            WorldServer.Instance.DBAccessor.Database.Update(client.WorldAccount);

            character.LastUsage = DateTime.Now;
            WorldServer.Instance.DBAccessor.Database.Update(character);

            var document = new BsonDocument
            {
                { "AcctId", client.WorldAccount.Id },
                { "CharacterId", character.Id },
                { "IPAddress", client.IP },
                { "Action", "Login" },
                { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
            };

            MongoLogger.Instance.Insert("characters_connections", document);
        }


        [WorldHandler(CharactersListRequestMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleCharacterListRequest(WorldClient client, CharactersListRequestMessage message)
        {
            if (client.Account != null && client.Account.Login != "")
            {
                //SendCharactersListMessage(client);
                SendCharactersListWithRemodelingMessage(client);

                var characterInFight = FindCharacterFightReconnection(client);
                if (characterInFight != null)
                {
                    client.ForceCharacterSelection = characterInFight;
                    SendCharacterSelectedForceMessage(client, characterInFight.Id);
                }

                if (client.WorldAccount != null && client.StartupActions.Count > 0)
                {
                    StartupHandler.SendStartupActionsListMessage(client, client.StartupActions);
                }
            }
            else
            {
                client.Send(new IdentificationFailedMessage((int)IdentificationFailureReasonEnum.KICKED));
                client.DisconnectLater(1000);
            }
        }

        [WorldHandler(CharacterSelectedForceReadyMessage.Id, IsGamePacket = false, ShouldBeLogged = false)]
        public static void HandleCharacterSelectedForceReadyMessage(WorldClient client, CharacterSelectedForceReadyMessage message)
        {
            if (client.ForceCharacterSelection == null)
                client.Disconnect();
            else
                CommonCharacterSelection(client, client.ForceCharacterSelection);
        }

        private static CharacterRecord FindCharacterFightReconnection(WorldClient client)
        {
            return (from characterInFight in client.Characters.Where(x => x.LeftFightId != null)
                    let fight = FightManager.Instance.GetFight(characterInFight.LeftFightId.Value)
                    where fight != null
                    let fighter = fight.GetLeaver(characterInFight.Id)
                    where fighter != null
                    select characterInFight).FirstOrDefault();
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
                    (sbyte)characterRecord.Breed,
                    characterRecord.Sex != SexTypeEnum.SEX_MALE)).ToList();

            client.Send(new CharactersListMessage(
                            characters,
                            false
                            ));
        }

        public static void SendCharactersListWithRemodelingMessage(WorldClient client)
        {
            var characterBaseInformations = new List<CharacterBaseInformations>();
            var charactersToRemodel = new List<CharacterToRemodelInformations>();

            foreach (var characterRecord in client.Characters.OrderByDescending(x => x.LastUsage))
            {
                characterBaseInformations.Add(new CharacterBaseInformations((short)characterRecord.Id,
                                                                            ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience, characterRecord.PrestigeRank),
                                                                            characterRecord.Name, characterRecord.EntityLook.GetEntityLook(),
                                                                            (sbyte)characterRecord.Breed,
                                                                            characterRecord.Sex == SexTypeEnum.SEX_MALE));

                if (((characterRecord.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED)
                != (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED)
                && ((characterRecord.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS)
                != (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS)
                && ((characterRecord.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC)
                != (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC)
                && ((characterRecord.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER)
                != (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER)
                && ((characterRecord.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME)
                != (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME))
                    continue;

                charactersToRemodel.Add(new CharacterToRemodelInformations(characterRecord.Id, characterRecord.Name,
                                                                                        (sbyte)characterRecord.Breed, characterRecord.Sex != SexTypeEnum.SEX_MALE,
                                                                                        (short)characterRecord.Head, characterRecord.EntityLook.Colors.Values.Select(x => x.ToArgb()),
                                                                                        characterRecord.PossibleChanges, characterRecord.MandatoryChanges));
            }
            client.Send(new CharactersListWithRemodelingMessage(characterBaseInformations,
                                                                   false,
                                                                   charactersToRemodel));
        }

        public static void SendCharacterSelectedSuccessMessage(WorldClient client)
        {
            client.Send(new CharacterSelectedSuccessMessage(client.Character.GetCharacterBaseInformations(), true));
        }

        public static void SendCharacterSelectedForceMessage(IPacketReceiver client, int id)
        {
            client.Send(new CharacterSelectedForceMessage(id));
        }

        public static void SendCharacterCapabilitiesMessage(WorldClient client)
        {
            client.Send(new CharacterCapabilitiesMessage(4095));
        }

        public static void SendCharacterLoadingCompleteMessage(WorldClient client)
        {
            client.Send(new CharacterLoadingCompleteMessage());
        }
    }
}