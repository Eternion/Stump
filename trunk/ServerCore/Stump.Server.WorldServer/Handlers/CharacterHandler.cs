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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Stump.BaseCore.Framework.Utils;
using Stump.Database;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Breeds;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Handlers
{
    public class CharacterHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(CharacterCreationRequestMessage))]
        public static void HandleCharacterCreateRequest(WorldClient client, CharacterCreationRequestMessage message)
        {
            // 0) Check if we can create characters on this server
            /*         [ToDo]       */

            // 1) Check if client has reached his character's creation number
            if (IpcAccessor.Instance.ProxyObject.GetCharacterAccountCount(WorldServer.ServerInformation, client.Account.Id) >= World.MaxCharacterSlot)
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS));
                return;
            }

            // 2) Get char name
            string characterName = message.name;

            // 3) Check if character name exists
            if (CharacterDatabase.CharacterExists(characterName))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }

            // 4) Check name format/modify it if necessary (First upper next lower...)
            characterName = StringUtils.FirstLetterUpper(characterName.ToLower());

            if (!Regex.IsMatch(characterName, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$"))
            {
                client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.ERR_INVALID_NAME));
                return;
            }

            BaseBreed breed = BreedManager.GetBreed(message.breed);

            var charcolors = new List<int>();
            for (int i = 0; i < message.colors.Count; i++)
            {
                if (message.colors[i] == -1) // we must change base color (-1) by the real color
                {
                    charcolors.Add(!message.sex ? breed.MaleColors[i] : breed.FemaleColors[i]);
                }
                else
                    charcolors.Add(message.colors[i]);
            }

            var charskins = new List<short> {(short) (message.breed*10 + (message.sex ? 1 : 0))};

            // 5) Insert character in db
            var record = new CharacterRecord
                {
                    New = true,
                    Level = breed.StartLevel,
                    Name = characterName,
                    Classe = message.breed,
                    SexId = message.sex ? 1 : 0,
                    Skins = charskins,
                    Scale = breed.Scale,
                    Colors = charcolors,
                    AccountName = client.Account.Login,
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

            CharacterDatabase.CreateCharacter(record, client);

            // then we save the spells
            foreach (SpellIdEnum spellId in breed.StartSpells.Keys)
            {
                int position = breed.StartSpells[spellId];
                record.AddSpell(spellId, position, 1);
            }

            BasicHandler.SendBasicNoOperationMessage(client);
            client.Send(new CharacterCreationResultMessage((int)CharacterCreationResultEnum.OK));
            SendCharactersListMessage(client);
        }

        [WorldHandler(typeof(CharacterNameSuggestionRequestMessage))]
        public static void CharacterNameSuggestionRequest(WorldClient client, CharacterNameSuggestionRequestMessage message)
        {
            string generatedName = CharacterRecord.GenerateName();

            client.Send(new CharacterNameSuggestionSuccessMessage(generatedName));
        }

        [WorldHandler(typeof(CharactersListRequestMessage))]
        public static void HandleCharacterListRequest(WorldClient client, CharactersListRequestMessage message)
        {
            if (client.Account != null && client.Account.Login != "")
            {
                CharacterDatabase.LoadCharacters(client);
                SendCharactersListMessage(client);
            }
            else
            {
                client.Send(new IdentificationFailedMessage((int)IdentificationFailureReasonEnum.KICKED));
            }
        }

        [WorldHandler(typeof(CharacterDeletionRequestMessage))]
        public static void HandleCharacterDeletionRequest(WorldClient client, CharacterDeletionRequestMessage message)
        {
            //////////////////////////////////////////////////////////////////////////
            // TODO : - save a copy of the character during 30 days
            //////////////////////////////////////////////////////////////////////////

            uint characterId = message.characterId;

            CharacterRecord characterRecord = client.Characters.Find(o => o.Id == characterId &&
                o.AccountName.ToLower() == client.Account.Login.ToLower());

            if (characterId < 0 || characterRecord == null) // Incorrect Value.
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_NO_REASON));
                client.Disconnect();
            }

            string secretAnswerHash = message.secretAnswerHash;

            if (characterRecord.Level <= 20 || ( client.Account.SecretAnswer != null &&
                secretAnswerHash == StringUtils.GetMd5(characterId + "~" + client.Account.SecretAnswer) ))
            {
                CharacterDatabase.DeleteCharacter(characterRecord, client);

                SendCharactersListMessage(client);
                BasicHandler.SendBasicNoOperationMessage(client);
            }
            else
            {
                client.Send(new CharacterDeletionErrorMessage((int)CharacterDeletionErrorEnum.DEL_ERR_BAD_SECRET_ANSWER));
            }
        }

        [WorldHandler(typeof (StatsUpgradeRequestMessage))]
        public static void HandleStatsUpgradeRequestMessage(WorldClient client, StatsUpgradeRequestMessage message)
        {
            var statsid = (CaracteristicsIdEnum) message.statId;

            if (statsid < CaracteristicsIdEnum.Strength ||
                statsid > CaracteristicsIdEnum.Intelligence)
                throw new Exception("Wrong statsid");

            if (message.boostPoint <= 0)
                throw new Exception("Client given 0 as boostpoint. Forbidden value.");

            BaseBreed breed = BreedManager.GetBreed(client.ActiveCharacter.BreedId);
            int neededpts = breed.GetNeededPointForStats(client.ActiveCharacter.Stats[statsid.ToString()].Base, statsid);

            var boost = (int) (message.boostPoint/neededpts);

            if (boost < 0)
                throw new Exception("Client is attempt to use more points that he has.");

            // Exception for Sacrieur Vitality * 2
            if (breed.Id == BreedEnum.Sacrieur && statsid == CaracteristicsIdEnum.Vitality)
                boost *= 2;

            client.ActiveCharacter.Stats[statsid.ToString()].Base += boost;

            SendStatsUpgradeResultMessage(client, message.boostPoint);
            SendCharacterStatsListMessage(client);
        }

        [WorldHandler(typeof(LeaveDialogRequestMessage))]
        public static void HandleLeaveDialogRequestMessage(WorldClient client, LeaveDialogRequestMessage message)
        {
            if (client.ActiveCharacter.IsDialogRequested)
            {
                client.ActiveCharacter.DialogRequest.DeniedDialog();
            }
            else if (client.ActiveCharacter.IsInDialog)
            {
                client.ActiveCharacter.Dialog.EndDialog();
            }
        }

        public static void SendGameContextCreateMessage(WorldClient client, byte context)
        {
            client.Send(new GameContextCreateMessage(context));
        }

        public static void SendGameContextDestroyMessage(WorldClient client)
        {
            client.Send(new GameContextDestroyMessage());
        }

        public static void SendStatsUpgradeResultMessage(WorldClient client, uint usedpts)
        {
            client.Send(new StatsUpgradeResultMessage(usedpts));
        }

        public static void SendSetCharacterRestrictionsMessage(WorldClient client)
        {
            client.Send(new SetCharacterRestrictionsMessage(
                            new ActorRestrictionsInformations(
                                false, // cantBeAgressed
                                false, // cantBeChallenged
                                false, // cantTrade
                                false, // cantBeAttackedByMutant
                                false, // cantRun
                                false, // forceSlowWalk
                                false, // cantMinimize
                                false, // cantMove

                                true, // cantAggress
                                false, // cantChallenge
                                false, // cantExchange
                                false, // cantAttack
                                false, // cantChat
                                true, // cantBeMerchant
                                true, // cantUseObject
                                true, // cantUseTaxCollector

                                false, // cantUseInteractive
                                false, // cantSpeakToNPC
                                false, // cantChangeZone
                                false, // cantAttackMonster
                                false // cantWalk8Directions
                                )));
        }


        public static void SendCharactersListMessage(WorldClient client)
        {
            CharacterDatabase.UpdateCharactersCount(client); // Recount characters number and set it in Database

            var list = new List<CharacterBaseInformations>();

            foreach (CharacterRecord cr in client.Characters)
            {
                var colors = new List<int>();
                for (int i = 0; i < cr.Colors.Count; i++)
                {
                    // print format : 0x{colorheader}[red][green][blue]
                    // e.g : 0x5000000 is color black for the head

                    string hexColor = (i + 1) + cr.Colors[i].ToString("X6");

                    colors.Add(int.Parse(hexColor, NumberStyles.HexNumber));
                }

                list.Add(new CharacterBaseInformations(
                             CharacterBaseInformations.protocolId,
                             (uint) cr.Level,
                             cr.Name,
                             new EntityLook(
                                 1, // bonesId
                                 new List<uint>(), // skins
                                 colors,
                                 new List<int>(cr.Scale),
                                 new List<SubEntity>()),
                             cr.Classe,
                             cr.SexId != 0));
            }


            client.Send(new CharactersListMessage(
                            true, // HasStartupActions
                            false, // tutorialsavailable
                            list
                            ));
        }

        public static void SendCharacterStatsListMessage(WorldClient client)
        {
            client.Send(
                new CharacterStatsListMessage(
                    new CharacterCharacteristicsInformations(
                        0d, // EXPERIENCE
                        0d, // EXPERIENCE level floor 
                        110d, // EXPERIENCE nextlevel floor 
            
                        (uint) client.ActiveCharacter.Kamas,
                        // Amount of kamas.

                        (uint) client.ActiveCharacter.StatsPoint,
                        // Stats points
                        (uint) client.ActiveCharacter.SpellsPoints,
                        // Spell points

                        // Alignment
                        new ActorExtendedAlignmentInformations(
                            0, // alignmentSide
                            0, // alignmentValue 
                            10, // alignmentGrade 
                            0, // dishonor
                            0, // characterPower
                            0, // honor points
                            0, // honorGradeFloor
                            0, // honorNextGradeFloor
                            false // pvpEnabled
                            ),
                        (uint) client.ActiveCharacter.Stats["Health"].
                                   Total, // Life points
                        (uint) ((StatsHealth) client.ActiveCharacter.Stats["Health"]).
                                   TotalMax, // Max Life points

                        10000, // Energy points
                        10000, // maxEnergyPoints

                        (short) client.ActiveCharacter.Stats["AP"]
                                    .Total, // actionPointsCurrent
                        (short) client.ActiveCharacter.Stats["MP"]
                                    .Total, // movementPointsCurrent

                        client.ActiveCharacter.Stats["Initiative"],
                        client.ActiveCharacter.Stats["Prospecting"],
                        client.ActiveCharacter.Stats["AP"],
                        client.ActiveCharacter.Stats["MP"],
                        client.ActiveCharacter.Stats["Strength"],
                        client.ActiveCharacter.Stats["Vitality"],
                        client.ActiveCharacter.Stats["Wisdom"],
                        client.ActiveCharacter.Stats["Chance"],
                        client.ActiveCharacter.Stats["Agility"],
                        client.ActiveCharacter.Stats["Intelligence"],
                        client.ActiveCharacter.Stats["Range"],
                        client.ActiveCharacter.Stats["SummonLimit"],
                        client.ActiveCharacter.Stats["DamageReflection"],
                        client.ActiveCharacter.Stats["CriticalHit"],
                        client.ActiveCharacter.Inventory.WeaponCriticalHit,
                        client.ActiveCharacter.Stats["CriticalMiss"],
                        client.ActiveCharacter.Stats["HealBonus"],
                        client.ActiveCharacter.Stats["DamageBonus"],
                        client.ActiveCharacter.Stats["WeaponDamageBonus"],
                        client.ActiveCharacter.Stats["DamageBonusPercent"],
                        client.ActiveCharacter.Stats["TrapBonus"],
                        client.ActiveCharacter.Stats["TrapBonusPercent"],
                        client.ActiveCharacter.Stats["PermanentDamagePercent"],
                        client.ActiveCharacter.Stats["TackleBlock"],
                        client.ActiveCharacter.Stats["TackleEvade"],
                        client.ActiveCharacter.Stats["APAttack"],
                        client.ActiveCharacter.Stats["MPAttack"],
                        client.ActiveCharacter.Stats["PushDamageBonus"],
                        client.ActiveCharacter.Stats["CriticalDamageBonus"],
                        client.ActiveCharacter.Stats["NeutralDamageBonus"],
                        client.ActiveCharacter.Stats["EarthDamageBonus"],
                        client.ActiveCharacter.Stats["WaterDamageBonus"],
                        client.ActiveCharacter.Stats["AirDamageBonus"],
                        client.ActiveCharacter.Stats["FireDamageBonus"],
                        client.ActiveCharacter.Stats["DodgeAPProbability"],
                        client.ActiveCharacter.Stats["DodgeMPProbability"],
                        client.ActiveCharacter.Stats["NeutralResistPercent"],
                        client.ActiveCharacter.Stats["EarthResistPercent"],
                        client.ActiveCharacter.Stats["WaterResistPercent"],
                        client.ActiveCharacter.Stats["AirResistPercent"],
                        client.ActiveCharacter.Stats["FireResistPercent"],
                        client.ActiveCharacter.Stats["NeutralElementReduction"],
                        client.ActiveCharacter.Stats["EarthElementReduction"],
                        client.ActiveCharacter.Stats["WaterElementReduction"],
                        client.ActiveCharacter.Stats["AirElementReduction"],
                        client.ActiveCharacter.Stats["FireElementReduction"],
                        client.ActiveCharacter.Stats["PushDamageReduction"],
                        client.ActiveCharacter.Stats["CriticalDamageReduction"],
                        client.ActiveCharacter.Stats["PvpNeutralResistPercent"],
                        client.ActiveCharacter.Stats["PvpEarthResistPercent"],
                        client.ActiveCharacter.Stats["PvpWaterResistPercent"],
                        client.ActiveCharacter.Stats["PvpAirResistPercent"],
                        client.ActiveCharacter.Stats["PvpFireResistPercent"],
                        client.ActiveCharacter.Stats["PvpNeutralElementReduction"],
                        client.ActiveCharacter.Stats["PvpEarthElementReduction"],
                        client.ActiveCharacter.Stats["PvpWaterElementReduction"],
                        client.ActiveCharacter.Stats["PvpAirElementReduction"],
                        client.ActiveCharacter.Stats["PvpFireElementReduction"],
                        new List<CharacterSpellModification>()
                        )));
        }
    }
}