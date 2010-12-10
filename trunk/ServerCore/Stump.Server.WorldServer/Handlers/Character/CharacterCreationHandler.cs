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
using System.Text.RegularExpressions;
using Stump.BaseCore.Framework.Utils;
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Breeds;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (CharacterCreationRequestMessage))]
        public static void HandleCharacterCreateRequest(WorldClient client, CharacterCreationRequestMessage message)
        {
            // 0) Check if we can create characters on this server
            /*         [ToDo]       */

            // 1) Check if client has reached his character's creation number
            if (
                IpcAccessor.Instance.ProxyObject.GetCharacterAccountCount(WorldServer.ServerInformation,
                                                                          client.Account.Id) >= World.MaxCharacterSlot)
            {
                client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS));
                return;
            }

            // 2) Get char name
            string characterName = message.name;

            // 3) Check if character name exists
            if (CharacterDatabase.CharacterExists(characterName))
            {
                client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS));
                return;
            }

            // 4) Check name format/modify it if necessary (First upper next lower...)
            characterName = StringUtils.FirstLetterUpper(characterName.ToLower());

            if (!Regex.IsMatch(characterName, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$"))
            {
                client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.ERR_INVALID_NAME));
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
            client.Send(new CharacterCreationResultMessage((int) CharacterCreationResultEnum.OK));
            SendCharactersListMessage(client);
        }

        [WorldHandler(typeof (CharacterNameSuggestionRequestMessage))]
        public static void CharacterNameSuggestionRequest(WorldClient client,
                                                          CharacterNameSuggestionRequestMessage message)
        {
            string generatedName = CharacterRecord.GenerateName();

            client.Send(new CharacterNameSuggestionSuccessMessage(generatedName));
        }
    }
}