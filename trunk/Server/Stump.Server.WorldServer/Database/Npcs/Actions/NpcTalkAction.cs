using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;

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

        [Property("Talk_MessageId")]
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