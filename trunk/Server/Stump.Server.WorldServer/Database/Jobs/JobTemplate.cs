using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Castle.ActiveRecord;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class JobTemplateConfiguration : EntityTypeConfiguration<JobTemplate>
    {
        public JobTemplateConfiguration()
        {
            ToTable("jobs_templates");
        }
    }

    [D2OClass("Job", "com.ankamagames.dofus.datacenter.jobs")]
    public class JobTemplate : ISaveIntercepter, IAssignedByD2O
    {
        private string m_name;

        public int Id
        {
            get;
            set;
        }

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

        public int SpecializationOfId
        {
            get;
            set;
        }

        public int IconId
        {
            get;
            set;
        }

        public byte[] ToolIdsBin
        {
            get
            {
                return m_toolIdsBin;
            }
            set
            {
                m_toolIdsBin = value;
                m_toolIds = value.ToObject<List<int>>();
            }
        }

        private List<int> m_toolIds;
        private byte[] m_toolIdsBin;

        public List<int> ToolIds
        {
            get { return m_toolIds; }
            set
            {
                m_toolIds = value; 
                m_toolIdsBin = value.ToBinary();
            }
        }

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_toolIdsBin = m_toolIds.ToBinary();
        }

        public void AssignFields(object d2oObject)
        {
            var job = (DofusProtocol.D2oClasses.Job)d2oObject;
            Id = job.id;
            NameId = job.nameId;
            SpecializationOfId = job.specializationOfId;
            IconId = job.iconId;
            ToolIds = job.toolIds;
        }
    }
}