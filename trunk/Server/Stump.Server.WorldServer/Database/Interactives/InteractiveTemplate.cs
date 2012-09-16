using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class InteractiveTemplateConfiguration : EntityTypeConfiguration<InteractiveTemplate>
    {
        public InteractiveTemplateConfiguration()
        {
            ToTable("interactives_templates");
            HasMany(x => x.Skills).WithMany().
                Map(x => x.
                             MapLeftKey("InteractiveTemplateId").
                             MapRightKey("SkillId").
                             ToTable("interactives_templates_skills"));
        }
    }

    [D2OClass("Interactive", "com.ankamagames.dofus.datacenter.interactives")]
    public class InteractiveTemplate : IAssignedByD2O
    {
        private string m_name;
        private IList<SkillRecord> m_skills;

        public int Id
        {
            get;
            set;
        }

        public InteractiveTypeEnum Type
        {
            get { return (InteractiveTypeEnum) Id; }
        }

        public uint NameId
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        public int ActionId
        {
            get;
            set;
        }

        public bool DisplayTooltip
        {
            get;
            set;
        }

        public virtual HashSet<SkillRecord> Skills
        {
            get;
            set;
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var template = (DofusProtocol.D2oClasses.Interactive) d2oObject;
            Id = template.id;
            NameId = template.nameId;
            ActionId = template.actionId;
            DisplayTooltip = template.displayTooltip;
        }

        #endregion
    }
}