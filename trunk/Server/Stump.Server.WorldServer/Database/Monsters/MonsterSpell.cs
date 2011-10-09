using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Spells;

namespace Stump.Server.WorldServer.Database.Monsters
{
    [ActiveRecord("monsters_spells")]
    public class MonsterSpell : WorldBaseRecord<MonsterGrade>, ISpellRecord
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [BelongsTo("MonsterGradeId")]
        public MonsterGrade MonsterGrade
        {
            get;
            set;
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