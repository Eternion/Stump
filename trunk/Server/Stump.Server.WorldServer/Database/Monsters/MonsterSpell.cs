using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Database.Monsters
{
    [ActiveRecord("monsters_spells")]
    public class MonsterSpell : WorldBaseRecord<MonsterSpell>, ISpellRecord
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property("MonsterGradeId")]
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

        [Property("SpellId", NotNull = true)]
        public int SpellId
        {
            get;
            set;
        }

        [Property("Level", NotNull = true, Default = "1")]
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