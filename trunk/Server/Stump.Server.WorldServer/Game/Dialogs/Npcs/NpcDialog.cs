using System.Linq;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Dialogs;

namespace Stump.Server.WorldServer.Game.Dialogs.Npcs
{
    public class NpcDialog : IDialog
    {
        public NpcDialog(Character character, Npc npc)
        {
            Character = character;
            Npc = npc;
        }

        public Character Character
        {
            get;
            private set;
        }

        public Npc Npc
        {
            get;
            private set;
        }

        public NpcMessage CurrentMessage
        {
            get;
            private set;
        }

        public void Open()
        {
            Character.SetDialog(this);
            ContextRoleplayHandler.SendNpcDialogCreationMessage(Character.Client, Npc);
        }

        public void Close()
        {
            DialogHandler.SendLeaveDialogMessage(Character.Client);
            Character.ResetDialog();
        }

        public void Reply(short replyId)
        {
            var replies = CurrentMessage.Replies.Where(entry => entry.ReplyId == replyId);

            foreach (var npcReply in replies)
            {
                Reply(npcReply);
            }
        }

        public void Reply(NpcReply reply)
        {
            reply.Execute(Npc, Character);
        }

        public void ChangeMessage(short id)
        {
            var message = NpcManager.Instance.GetNpcMessage(id);

            if (message != null)
                ChangeMessage(message);
        }

        public void ChangeMessage(NpcMessage message)
        {
            CurrentMessage = message;

            var replies = message.Replies.
                Where(entry => entry.CriteriaExpression == null || entry.CriteriaExpression.Eval(Character)).
                Select(entry => (short)entry.ReplyId).Distinct();

            ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, replies);
        }
    }
}