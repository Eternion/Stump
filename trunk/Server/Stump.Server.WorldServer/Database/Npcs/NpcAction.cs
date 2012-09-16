using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Conditions;

namespace Stump.Server.WorldServer.Database
{
    public class NpcActionConfiguration : EntityTypeConfiguration<NpcAction>
    {
        public NpcActionConfiguration()
        {
            ToTable("npcs_actions");
            Map(x => x.Requires("Discriminator").HasValue("Base"));
        }
    }

    public abstract class NpcAction
    {
        public uint Id
        {
            get;
            set;
        }

        public int NpcId
        {
            get;
            set;
        }

        private Npcs.NpcTemplate m_template;
        public Npcs.NpcTemplate Template
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