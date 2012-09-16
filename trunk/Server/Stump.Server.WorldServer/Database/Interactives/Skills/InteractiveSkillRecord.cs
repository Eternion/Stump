using System.Data.Entity.ModelConfiguration;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Database
{
    public class InteractiveSkillRecordConfiguration : EntityTypeConfiguration<InteractiveSkillRecord>
    {
        public InteractiveSkillRecordConfiguration()
        {
            ToTable("interactives_skills");
        }
    }

    public abstract class InteractiveSkillRecord
    {
        public const int DEFAULT_TEMPLATE = 184;

        private int? m_customTemplateId;
        private InteractiveSkillTemplate m_template;

        public int Id
        {
            get;
            set;
        }

        public int? CustomTemplateId
        {
            get { return m_customTemplateId; }
            set
            {
                m_customTemplateId = value;
                m_template = null;
            }
        }

        protected virtual int GenericTemplateId // determin the name
        {
            get { return DEFAULT_TEMPLATE; }
        }

        public virtual InteractiveSkillTemplate Template
        {
            get
            {
                return m_template ??
                       (m_template =
                        InteractiveManager.Instance.GetSkillTemplate(CustomTemplateId.HasValue
                                                                         ? CustomTemplateId.Value
                                                                         : GenericTemplateId));
            }
        }

        public abstract Skill GenerateSkill(int id, InteractiveObject interactiveObject);
    }
}