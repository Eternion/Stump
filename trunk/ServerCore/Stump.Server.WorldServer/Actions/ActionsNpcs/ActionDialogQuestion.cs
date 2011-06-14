
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