using System;
using System.Linq;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Dialogs;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Worlds.Dialogs.Npcs
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
            var reply = CurrentMessage.Replies.Where(entry => entry.Id == replyId).FirstOrDefault();

            if (reply != null)
                Reply(reply);
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

            ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage);
        }
    }
}