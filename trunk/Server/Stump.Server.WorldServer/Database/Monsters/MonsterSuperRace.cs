using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class MonsterSuperRaceConfiguration : EntityTypeConfiguration<MonsterSuperRace>
    {
        public MonsterSuperRaceConfiguration()
        {
            ToTable("monsters_superraces");
        }
    }
    [D2OClass("MonsterSuperRace", "com.ankamagames.dofus.datacenter.monsters")]
    public sealed class MonsterSuperRace : IAssignedByD2O
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

        public void AssignFields(object d2oObject)
        {
            var race = (DofusProtocol.D2oClasses.MonsterSuperRace)d2oObject;
            Id = race.id;
            NameId = race.nameId;
        }
    }
}