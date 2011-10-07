using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.Monsters
{
    [ActiveRecord("monsters_races")]
    [D2OClass("MonsterRace", "com.ankamagames.dofus.datacenter.monsters")]
    public sealed class MonsterRace : WorldBaseRecord<MonsterRace>
    {
        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OField("superRaceId")]
        [BelongsTo("SuperRaceId")]
        public MonsterSuperRace SuperRace
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
        private string m_name;

        public string Name
        {
            get
            {
                return m_name ?? ( m_name = TextManager.Instance.GetText(NameId) );
            }
        } 

    }
}