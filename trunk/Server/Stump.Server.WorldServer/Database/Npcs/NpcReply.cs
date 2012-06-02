using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Conditions;
using Stump.Server.WorldServer.Handlers.Basic;

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

        [Property("Reply")]
        public int ReplyId
        {
            get;
            set;
        }

        [Property("Message")]
        public int MessageId
        {
            get;
            set;
        }

        [Property("Criteria")]
        public string Criteria
        {
            get;
            set;
        }

        private ConditionExpression m_criteriaExpression;

        public ConditionExpression CriteriaExpression
        {
            get
            {
                if (string.IsNullOrEmpty(Criteria) || Criteria == "null")
                    return null;

                return m_criteriaExpression ?? ( m_criteriaExpression = ConditionExpression.Parse(Criteria) );
            }
            set
            {
                m_criteriaExpression = value;
                Criteria = value.ToString();
            }
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

        public virtual bool Execute(Npc npc, Character character)
        {
            if (CriteriaExpression != null && !CriteriaExpression.Eval(character))
            {
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 34);
                return false;
            }

            return true;
        }
    }
}