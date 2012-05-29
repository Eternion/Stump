using System;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [ActiveRecord(DiscriminatorValue = "Dialog")]
    public class ContinueDialogReply : NpcReply
    {
        [Property("Dialog_NextMessageId", NotNull = false)]
        public int NextMessageId
        {
            get;
            set;
        }

        private NpcMessage m_message;
        public NpcMessage NextMessage
        {
            get
            {
                return m_message ?? ( m_message = NpcManager.Instance.GetNpcMessage(NextMessageId) );
            }
            set
            {
                m_message = value;
                NextMessageId = value.Id;
            }
        }
        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            if (!character.IsTalkingWithNpc())
                return false;

            ( (NpcDialog)character.Dialog ).ChangeMessage(NextMessage);

            return true;
        }
    }
}