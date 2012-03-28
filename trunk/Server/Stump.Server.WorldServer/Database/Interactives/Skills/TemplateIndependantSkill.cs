using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Interactives;

namespace Stump.Server.WorldServer.Database.Interactives.Skills
{
    //[ActiveRecord("interactives_skills", DiscriminatorValue = "Template_Independant")]
    public abstract class TemplateIndependantSkill : SkillRecord
    {
        [Property]
        public int TemplateId
        {
            get;
            set;
        }

        private SkillTemplate m_template;
        public override SkillTemplate Template
        {
            get
            {
                return m_template ?? ( m_template = InteractiveManager.Instance.GetSkillTemplate(TemplateId) );
            }
        }
    }
}