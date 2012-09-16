using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Interactives;

namespace Stump.Server.WorldServer.Database
{
    public abstract partial class InteractiveSkillTemplateDependantRecord
    {
        public abstract int TemplateId
        {
            get;
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