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
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Npcs;

namespace Stump.Server.WorldServer.Actions.ActionsNpcs
{
    public class ActionDialogQuestion : NpcAction
    {
        private ActionDialogQuestion()
        {
        }

        public ActionDialogQuestion(int messageId)
        {
            MessageId = messageId;
        }

        public int MessageId
        {
            get;
            set;
        }

        public override void Execute(NpcSpawn npc, Character executer)
        {
            if (!executer.IsInDialogWithNpc)
                return;

            var question = NpcManager.GetQuestion(MessageId);

            if (question == null)
                return;

            ( (NpcDialog) executer.Dialog ).ChangeQuestion(question);
        }
    }
}