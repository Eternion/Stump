using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Worlds.Dialogs.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Actions
{
    [ActiveRecord(DiscriminatorValue = "Talk")]
    public class NpcTalkAction : NpcAction
    {
        public override NpcActionTypeEnum ActionType
        {
            get
            {
                return NpcActionTypeEnum.ACTION_TALK;
            }
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

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcDialog(character, npc);

            dialog.Open();
            dialog.ChangeMessage(Message);
        }
    }
}