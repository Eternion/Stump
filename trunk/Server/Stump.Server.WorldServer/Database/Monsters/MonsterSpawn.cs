using System.ComponentModel;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Database
{
    public class MonsterSpawnConfiguration : EntityTypeConfiguration<MonsterSpawn>
    {
        public MonsterSpawnConfiguration()
        {
            ToTable("monsters_spawns");
        }
    }
    public class MonsterSpawn : WorldBaseRecord<MonsterSpawn>
    {
        public int Id
        {
            get;
            set;
        }

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

        public int MonsterId
        {
            get;
            set;
        }

        [DefaultValue(1.0)]
        public double Frequency
        {
            get;
            set;
        }

        [DefaultValue(1)]
        public int MinGrade
        {
            get;
            set;
        }

        [DefaultValue(5)]
        public int MaxGrade
        {
            get;
            set;
        }
    }
}