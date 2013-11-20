using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Replies;
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

        public DialogTypeEnum DialogType
        {
            get
            {
                return DialogTypeEnum.DIALOG_DIALOG;
            }
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
            protected set;
        }

        public virtual void Open()
        {
            Character.SetDialog(this);
            ContextRoleplayHandler.SendNpcDialogCreationMessage(Character.Client, Npc);
        }

        public virtual void Close()
        {
            DialogHandler.SendLeaveDialogMessage(Character.Client, DialogType);
            Character.CloseDialog(this);
        }

        public virtual void Reply(short replyId)
        {
            var lastMessage = CurrentMessage;
            var replies = CurrentMessage.Replies.Where(entry => entry.ReplyId == replyId).ToArray();

            if (replies.Any(x => !x.CanExecute(Npc, Character)))
            {
                Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 34);
                return;
            }

            foreach (var npcReply in replies)
            {
                Reply(npcReply);
            }

            // default action : close dialog
            if (replies.Length == 0 || lastMessage == CurrentMessage)
                Close();
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

        public virtual void ChangeMessage(NpcMessage message)
        {
            CurrentMessage = message;

            var replies = message.Replies.
                Where(entry => entry.CriteriaExpression == null || entry.CriteriaExpression.Eval(Character)).
                Select(entry => (short)entry.ReplyId).Distinct();

            ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, replies);
        }
    }
}