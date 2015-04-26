using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Actions
{
    [Discriminator("Talk", typeof(NpcActionDatabase), typeof(NpcActionRecord))]
    public class NpcTalkAction : NpcActionDatabase
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get
            {
                return new [] { NpcActionTypeEnum.ACTION_TALK };
            }
        }

        private NpcMessage m_message;

        public NpcTalkAction(NpcActionRecord record)
            : base(record)
        {
        }

        /// <summary>
        /// Parameter 0
        /// </summary>
        public int MessageId
        {
            get { return Record.GetParameter<int>(0); }
            set { Record.SetParameter(0, value); }
        }

        public NpcMessage Message
        {
            get { return m_message ?? (m_message = NpcManager.Instance.GetNpcMessage(MessageId)); }
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