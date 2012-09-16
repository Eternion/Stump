using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class SuperAreaRecordConfiguration : EntityTypeConfiguration<SuperAreaRecord>
    {
        public SuperAreaRecordConfiguration()
        {
            ToTable("superareas");
        }
    }

    [D2OClass("SuperArea", "com.ankamagames.dofus.datacenter.world")]
    public sealed class SuperAreaRecord : IAssignedByD2O
    {
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

        private string m_name;

        public string Name
        {
            get
            {
                return m_name ?? ( m_name = TextManager.Instance.GetText(NameId) );
            }
        }

        public uint WorldmapId
        {
            get;
            set;
        }

        public void AssignFields(object d2oObject)
        {
            var area = (SuperArea)d2oObject;
            Id = area.id;
            NameId = area.nameId;
            WorldmapId = area.worldmapId;
        }
    }
}