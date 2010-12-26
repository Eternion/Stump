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
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Npcs;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {
        [WorldHandler(typeof(NpcGenericActionRequestMessage))]
        public static void HandleNpcGenericActionRequestMessage(WorldClient client, NpcGenericActionRequestMessage message)
        {
            var npc = client.ActiveCharacter.Map.Get<NpcSpawn>(message.npcId);

            if (npc == null)
                return;
            
            npc.Interact((NpcActionTypeEnum) message.npcActionId, client.ActiveCharacter);
        }

        [WorldHandler(typeof(NpcDialogReplyMessage))]
        public static void HandleNpcDialogReplyMessage(WorldClient client, NpcDialogReplyMessage message)
        {
            ((NpcDialog) client.ActiveCharacter.Dialog).Reply(message.replyId);
        }

        public static void SendNpcDialogCreationMessage(WorldClient client, NpcSpawn npc)
        {
            client.Send(new NpcDialogCreationMessage(npc.Map.Id, npc.ContextualId));
        }

        public static void SendNpcDialogQuestionMessage(WorldClient client, NpcDialogQuestion question)
        {
            client.Send(new NpcDialogQuestionMessage(question.Id, question.Parameters.ToList(), question.Replies.Keys.ToList()));
        }
    }
}