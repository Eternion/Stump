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
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {
        [WorldHandler(typeof (EmotePlayRequestMessage))]
        public static void HandleEmotePlayRequestMessage(WorldClient client, EmotePlayRequestMessage message)
        {
            if ((message.emoteId == (byte) EmotesEnum.EMOTE_SIT || message.emoteId == (byte) EmotesEnum.EMOTE_REST) &&
                client.ActiveCharacter.EmoteId == message.emoteId) // Emote sit/lay down && already down
                message.emoteId = 0; // he's standing now

            client.ActiveCharacter.EmoteId = (int) message.emoteId;

            // todo : found each emote duration
            client.ActiveCharacter.Map.CallOnAllCharactersWithoutFighters(
                charac => SendEmotePlayMessage(charac.Client, client.ActiveCharacter, (EmotesEnum) message.emoteId, 0));
        }

        public static void SendEmotePlayMessage(WorldClient client, Entity entity, EmotesEnum emote, uint duration)
        {
            client.Send(new EmotePlayMessage(
                            (byte) emote,
                            duration,
                            (int) entity.Id,
                            (int) client.Account.Id
                            ));
        }

        public static void SendEmoteListMessage(WorldClient client, IEnumerable<uint> emoteList)
        {
            client.Send(new EmoteListMessage(emoteList.ToList()));
        }
    }
}