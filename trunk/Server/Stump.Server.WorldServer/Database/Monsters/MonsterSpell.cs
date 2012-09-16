using System.ComponentModel;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Database
{
    public class MonsterSpellConfiguration : EntityTypeConfiguration<MonsterSpell>
    {
        public MonsterSpellConfiguration()
        {
            ToTable("monsters_spells");
        }
    }

    public class MonsterSpell : WorldBaseRecord<MonsterSpell>, ISpellRecord
    {
        public int Id
        {
            get;
            set;
        }

        public int MonsterGradeId
        {
            get;
            set;
        }

        private MonsterGrade m_monsterGrade;
        public MonsterGrade MonsterGrade
        {
            get
            {
                return m_monsterGrade ?? ( m_monsterGrade = MonsterManager.Instance.GetMonsterGrade(MonsterGradeId) );
            }
            set
            {
                m_monsterGrade = value;
                MonsterGradeId = value.Id;
            }
        }

        public int SpellId
        {
            get;
            set;
        }

        [DefaultValue(1)]
        public sbyte Level
        {
            get;
            set;
        }

        public override string ToString()
        {
            return (SpellIdEnum)SpellId + " (" + SpellId + ")";
        }
    }
}   