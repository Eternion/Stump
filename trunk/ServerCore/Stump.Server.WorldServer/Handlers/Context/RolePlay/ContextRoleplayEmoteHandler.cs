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
using Stump.Server.WorldServer.World.Actors.Character;
using Stump.Server.WorldServer.World.Entities.Actors;
using Stump.Server.WorldServer.World.Entities.Characters;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {

        [WorldHandler(typeof(EmotePlayRequestMessage))]
        public static void HandleEmotePlayRequestMessage(WorldClient client, EmotePlayRequestMessage message)
        {
            // todo : found the duration of each emote
            client.ActiveCharacter.StartEmote((EmotesEnum)message.emoteId, 0);
        }

        public static void SendEmotePlayMessage(WorldClient client, Character character, EmotesEnum emote, uint duration)
        {
            client.Send(new EmotePlayMessage((byte)emote, duration, (int)character.Id, (int)character.Client.Account.Id));
        }

        public static void SendEmotePlayMessage(WorldClient client, Actor actor, EmotesEnum emote, uint duration)
        {
            client.Send(new EmotePlayMessage((byte)emote, duration, (int)actor.Id, 0));
        }

        public static void SendEmoteListMessage(WorldClient client, IEnumerable<uint> emoteList)
        {
            client.Send(new EmoteListMessage(emoteList.ToList()));
        }

    }
}