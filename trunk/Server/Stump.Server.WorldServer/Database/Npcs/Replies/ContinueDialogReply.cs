using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;

namespace Stump.Server.WorldServer.Database
{
    public class ContinueDialogReplyConfiguration : EntityTypeConfiguration<ContinueDialogReply>
    {
        public ContinueDialogReplyConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("Dialog"));

            Property(x => x.NextMessageId).HasColumnName("Dialog_NextMessageId");
        }
    }
    public class ContinueDialogReply : Npcs.NpcReply
    {
        public int NextMessageId
        {
            get;
            set;
        }

        private Npcs.NpcMessage m_message;
        public Npcs.NpcMessage NextMessage
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