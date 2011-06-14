using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Npcs.StartActions
{
    public class TalkAction : NpcStartAction
    {
        private TalkAction()
        {
            
        }

        public TalkAction(int npcId, int messageId)
        {
            NpcId = npcId;
            MessageId = messageId;
            Condition = "";
        }

        public override NpcActionTypeEnum ActionType
        {
            get { return NpcActionTypeEnum.ACTION_TALK; }
        }

        public int MessageId;
        public string Condition; // todo : not used for the moment

        public override void Execute(NpcSpawn npc, Character executer)
        {
            var question = NpcManager.GetQuestion(MessageId);

            if (question == null)
                return;

            var dialog = new NpcDialog(npc, executer, question);

            executer.Dialoger = dialog.Dialoger;

            ContextHandler.SendNpcDialogCreationMessage(executer.Client, npc);
            ContextHandler.SendNpcDialogQuestionMessage(executer.Client, question);
        }
    }
}