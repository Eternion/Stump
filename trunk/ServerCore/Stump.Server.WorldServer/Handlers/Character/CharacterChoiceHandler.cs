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
using Stump.Database;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Database;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class CharacterHandler
    {
        [WorldHandler(typeof (CharactersListRequestMessage))]
        public static void HandleCharacterListRequest(WorldClient client, CharactersListRequestMessage message)
        {
            if (client.Account != null && client.Account.Login != "")
            {
                CharacterDatabase.LoadCharacters(client);
                SendCharactersListMessage(client);
            }
            else
            {
                client.Send(new IdentificationFailedMessage((int) IdentificationFailureReasonEnum.KICKED));
            }
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
    }
}