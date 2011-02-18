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
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.World.Actors.Actor;
using Stump.Server.WorldServer.World.Actors.Character;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ChatHandler : WorldHandlerContainer
    {

        [WorldHandler(typeof(ChatSmileyRequestMessage))]
        public static void HandleChatSmileyRequestMessage(WorldClient client, ChatSmileyRequestMessage message)
        {
            client.ActiveCharacter.DisplaySmiley((byte)message.smileyId);
        }

        public static void SendChatSmileyMessage(WorldClient client, Character character, uint smileyId)
        {
            client.Send(new ChatSmileyMessage((int)character.Id, smileyId, character.Client.Account.Id));
        }

        public static void SendChatSmileyMessage(WorldClient client, Actor actor, uint smileyId)
        {
            client.Send(new ChatSmileyMessage((int)actor.Id, smileyId, 0));
        }
    }
}