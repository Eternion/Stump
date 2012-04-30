using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Conditions;

namespace Stump.Server.WorldServer.Database.Npcs
{
    [ActiveRecord("npcs_actions", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Base")]
    public abstract class NpcAction : WorldBaseRecord<NpcAction>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public uint Id
        {
            get;
            set;
        }

        [Property("Npc")]
        public int NpcId
        {
            get;
            set;
        }

        private NpcTemplate m_template;
        public NpcTemplate Template
        {
            get
            {
                return m_template ?? ( m_template = NpcManager.Instance.GetNpcTemplate(NpcId) );
            }
            set
            {
                m_template = value;
                NpcId = value.Id;
            }
        }


        [Property("`Condition`")]
        public String Condition
        {
            get;
            set;
        }

        private ConditionExpression m_conditionExpression;

        public ConditionExpression ConditionaExpression
        {
            get
            {
                if (string.IsNullOrEmpty(Condition) || Condition == "null")
                    return null;

                return m_conditionExpression ?? ( m_conditionExpression = ConditionExpression.Parse(Condition) );
            }
            set
            {
                m_conditionExpression = value;
                Condition = value.ToString();
            }
        }

        public abstract NpcActionTypeEnum ActionType
        {
            get;
        }

        public abstract void Execute(Npc npc, Character character);
    }
}