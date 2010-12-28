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
using System.Linq;
using Stump.Database;
using Stump.Server.WorldServer.Manager;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler
    {
        [WorldHandler(typeof (CharacterSelectionMessage))]
        public static void HandleCharacterSelectionMessage(WorldClient client, CharacterSelectionMessage message)
        {
            var character = client.Characters.First(entry => entry.Id == message.id);
            
            /* Check null */
            if (character == null)
            {
                client.Send(new CharacterSelectedErrorMessage());
                return;
            }

            client.ActiveCharacter = new Character(character, client);


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

            FriendHandler.SendFriendWarnOnConnectionStateMessage(client, false);
            FriendHandler.SendFriendWarnOnLevelGainStateMessage(client, false);

            BasicHandler.SendTextInformationMessage(client, 1, 89);
            BasicHandler.SendTextInformationMessage(client, 0, 152,
                                                    client.Account.LastConnection.Year.ToString(),
                                                    client.Account.LastConnection.Month.ToString(),
                                                    client.Account.LastConnection.Day.ToString(),
                                                    client.Account.LastConnection.Hour.ToString(),
                                                    client.Account.LastConnection.Minute.ToString(),
                                                    client.Account.LastIp ?? "(null)");

            InitializationHandler.SendOnConnectionEventMessage(client, 2);
        }

        [WorldHandler(typeof (CharactersListRequestMessage))]
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
            List<CharacterBaseInformations> list = client.Characters.Select(
                characterRecord =>
                new CharacterBaseInformations(
                    (uint) characterRecord.Id,
                    (uint) characterRecord.Level,
                    characterRecord.Name,
                   CharacterManager.GetStuffedCharacterLook(characterRecord).EntityLook ,//look
                    characterRecord.Breed,
                    characterRecord.SexId != 0)).ToList();

            
            client.Send(new CharactersListMessage(
                            false, // HasStartupActions
                            list
                            ));
        }

        public static void SendCharacterSelectedSuccessMessage(WorldClient client)
        {
            client.Send(new CharacterSelectedSuccessMessage(client.ActiveCharacter.GetBaseInformations()));
        }
    }
}