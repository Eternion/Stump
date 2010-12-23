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
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcDialog : IDialog
    {
        public NpcSpawn Npc
        {
            get;
            set;
        }

        public NpcDialoger Dialoger
        {
            get;
            set;
        }

        public NpcDialogQuestion CurrentQuestion
        {
            get;
            private set;
        }

        public NpcDialog(NpcSpawn npc, Character dialoger)
        {
            Npc = npc;
            Dialoger = new NpcDialoger(dialoger, this);
        }

        public void ChangeQuestion(NpcDialogQuestion dialogQuestion)
        {
            CurrentQuestion = dialogQuestion;

            ContextHandler.SendNpcDialogQuestionMessage(Dialoger.Character.Client, CurrentQuestion);
        }

        public void Reply(uint replyId)
        {
            CurrentQuestion.CallReply(replyId, Npc, Dialoger.Character);
        }

        public void EndDialog()
        {
            try
            {
                DialogHandler.SendLeaveDialogMessage(Dialoger.Character.Client);
            }
            finally
            {
                Dialoger.Character.Dialoger = null;
            }
        }
    }
}