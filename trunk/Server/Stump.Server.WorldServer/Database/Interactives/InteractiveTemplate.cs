using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Interactives.Skills;

namespace Stump.Server.WorldServer.Database.Interactives
{
    [ActiveRecord("interactives")]
    [D2OClass("Interactive", "com.ankamagames.dofus.datacenter.interactives")]
    public class InteractiveTemplate : WorldBaseRecord<InteractiveTemplate>
    {
        private string m_name;
        private IList<SkillRecord> m_skills;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        public InteractiveTypeEnum Type
        {
            get { return (InteractiveTypeEnum) Id; }
        }

        [D2OField("nameId")]
        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }


        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        [D2OField("actionId")]
        [Property("ActionId")]
        public int ActionId
        {
            get;
            set;
        }

        [HasAndBelongsToMany(Table = "interactives_templates_skills", ColumnKey = "InteractiveId", ColumnRef = "SkillId")]
        public IList<SkillRecord> Skills
        {
            get { return m_skills ?? (m_skills = new List<SkillRecord>()); }
            set { m_skills = value; }
        }

        [D2OField("displayTooltip")]
        [Property]
        public bool DisplayTooltip
        {
            get;
            set;
        }
    }
}