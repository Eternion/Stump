using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

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
        [Property("SuperRaceId")]
        public int SuperRaceId
        {
            get;
            set;
        }

        private MonsterSuperRace m_superRace;
        public MonsterSuperRace SuperRace
        {
            get
            {
                return m_superRace ?? ( m_superRace = MonsterManager.Instance.GetSuperRace(SuperRaceId) );
            }
            set
            {
                m_superRace = value;
                SuperRaceId = value.Id;
            }
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