using Castle.ActiveRecord;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Database.Monsters
{
    [ActiveRecord("monsters_spawns")]
    public class MonsterSpawn : WorldBaseRecord<MonsterSpawn>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [BelongsTo(NotNull = false)]
        public MapRecord Map
        {
            get;
            set;
        }

        [BelongsTo(NotNull = false)]
        public SubAreaRecord SubArea
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public int MonsterId
        {
            get;
            set;
        }

        [Property(Column = "`Frenquency`", Default = "1.0", NotNull = true)]
        public double Frequency
        {
            get;
            set;
        }

        [Property(Default = "1", NotNull = true)]
        public int MinGrade
        {
            get;
            set;
        }

        [Property(Default = "5", NotNull = true)]
        public int MaxGrade
        {
            get;
            set;
        }
    }
}