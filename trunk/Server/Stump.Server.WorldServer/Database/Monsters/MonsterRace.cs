using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Database
{
    public class MonsterRaceConfiguration : EntityTypeConfiguration<MonsterRace>
    {
        public MonsterRaceConfiguration()
        {
            ToTable("monsters_races");
        }
    }

    [D2OClass("MonsterRace", "com.ankamagames.dofus.datacenter.monsters")]
    public class MonsterRace : IAssignedByD2O
    {
        public int Id
        {
            get;
            set;
        }

        public int SuperRaceId
        {
            get;
            set;
        }

        private Monsters.MonsterSuperRace m_superRace;
        public Monsters.MonsterSuperRace SuperRace
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
            var race = (DofusProtocol.D2oClasses.MonsterRace)d2oObject;
            Id = race.id;
            NameId = race.nameId;
            SuperRaceId = race.superRaceId;
        }
    }
}