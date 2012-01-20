using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs
{
    [ActiveRecord("npcs_replies", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Base")]
    public abstract class NpcReply : WorldBaseRecord<NpcReply>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property]
        public int ReplyId
        {
            get;
            set;
        }

        [Property("MessageId")]
        public int MessageId
        {
            get;
            set;
        }

        private NpcMessage m_message;
        public NpcMessage Message
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

        public abstract void Execute(Npc npc, Character character);
    }
}