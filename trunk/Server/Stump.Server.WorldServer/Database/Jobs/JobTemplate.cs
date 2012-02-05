using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.Jobs
{
    [ActiveRecord("jobs")]
    [D2OClass("Job", "com.ankamagames.dofus.datacenter.jobs")]
    public class JobTemplate : WorldBaseRecord<JobTemplate>
    {
        private string m_name;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
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
            get
            {
                return m_name ?? ( m_name = TextManager.Instance.GetText(NameId) );
            }
        }

        [D2OField("specializationOfId")]
        [Property("SpecializationOfId")]
        public int SpecializationOfId
        {
            get;
            set;
        }

        [D2OField("iconId")]
        [Property("IconId")]
        public int IconId
        {
            get;
            set;
        }

        [D2OField("toolIds")]
        [Property("ToolIds", ColumnType = "Serializable")]
        public List<int> ToolIds
        {
            get;
            set;
        }
    }
}