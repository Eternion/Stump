using Castle.ActiveRecord;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Database.World.Maps;
using Stump.Server.WorldServer.Game.Maps;

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

        [Property(NotNull = false)]
        public int? MapId
        {
            get;
            set;
        }
        
        private Map m_map;
        public Map Map
        {
            get
            {
                if (!MapId.HasValue)
                    return null;

                return m_map ?? ( m_map = Game.World.Instance.GetMap(MapId.Value) );
            }
            set
            {
                m_map = value;

                if (value == null)
                    MapId = null;
                else
                    MapId = value.Id;
            }
        }

        [Property(NotNull = false)]
        public int? SubAreaId
        {
            get;
            set;
        }

        private SubArea m_subArea;
        public SubArea SubArea
        {
            get
            {
                if (!SubAreaId.HasValue)
                    return null;

                return m_subArea ?? ( m_subArea = Game.World.Instance.GetSubArea(SubAreaId.Value) );
            }
            set
            {
                m_subArea = value;

                if (value == null)
                    SubAreaId = null;
                else
                    SubAreaId = value.Id;
            }
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