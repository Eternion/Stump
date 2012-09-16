using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class SpellTypeConfiguration : EntityTypeConfiguration<SpellType>
    {
        public SpellTypeConfiguration()
        {
            ToTable("spells_types");
        }
    }
    [D2OClass("SpellType", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellType : IAssignedByD2O
    {
        public int Id
        {
            get;
            set;
        }

        public uint LongNameId
        {
            get;
            set;
        }

        private string m_name;

        public string LongName
        {
            get
            {
                return m_name ?? ( m_name = TextManager.Instance.GetText(LongNameId) );
            }
        }

        public uint ShortNameId
        {
            get;
            set;
        }

        private string m_shortName;

        public string ShortName
        {
            get
            {
                return m_shortName ?? ( m_shortName = TextManager.Instance.GetText(ShortNameId) );
            }
        }

        public void AssignFields(object d2oObject)
        {
            var type = (DofusProtocol.D2oClasses.SpellType)d2oObject;
            Id = type.id;
            LongNameId = type.longNameId;
            ShortNameId = type.shortNameId;
        }
    }
}