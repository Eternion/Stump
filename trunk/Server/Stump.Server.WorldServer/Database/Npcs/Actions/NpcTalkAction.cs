using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;

namespace Stump.Server.WorldServer.Database
{
    public class NpcTalkActionConfiguration : EntityTypeConfiguration<NpcTalkAction>
    {
        public NpcTalkActionConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("Talk"));

            Property(x => x.MessageId).HasColumnName("Talk_MessageId");
        }
    }
    public class NpcTalkAction : Npcs.NpcAction
    {
        public override NpcActionTypeEnum ActionType
        {
            get
            {
                return NpcActionTypeEnum.ACTION_TALK;
            }
        }

        public int MessageId
        {
            get;
            set;
        }

        private Npcs.NpcMessage m_message;
        public Npcs.NpcMessage Message
        {
            get
            {
                return m_message ?? ( m_message = NpcManager.Instance.GetNpcMessage(MessageId) );
            }
            set
            {
                m_message = value;
                MessageId = value.Id;
            }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcDialog(character, npc);

            dialog.Open();
            dialog.ChangeMessage(Message);
        }
    }
}