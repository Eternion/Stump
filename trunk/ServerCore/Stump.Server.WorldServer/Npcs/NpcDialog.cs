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