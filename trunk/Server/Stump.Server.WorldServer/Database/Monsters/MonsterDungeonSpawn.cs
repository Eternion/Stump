using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database
{
    public class MonsterDungeonSpawnConfiguration : EntityTypeConfiguration<MonsterDungeonSpawn>
    {
        public MonsterDungeonSpawnConfiguration()
        {
            ToTable("monsters_spawns_dungeons");
            HasMany(x => x.GroupMonsters).WithMany().Map(x => x.MapLeftKey("DungeonSpawnId").MapRightKey("MonsterGradeId").ToTable("monsters_spawns_dungeons_groups"));
        }
    }
    public class MonsterDungeonSpawn
    {
        private List<Monsters.MonsterGrade> m_groupMonsters = new List<Monsters.MonsterGrade>();
        private Map m_map; 
        private Map m_teleportMap;
        private byte[] m_serializedMonsterGroup;

        public int Id
        {
            get;
            set;
        }

        public int MapId
        {
            get;
            set;
        }

        public Map Map
        {
            get
            {
                return m_map ?? (m_map = Game.World.Instance.GetMap(MapId));
            }
            set
            {
                m_map = value;
                MapId = value.Id;
            }
        }

        public HashSet<Monsters.MonsterGrade> GroupMonsters
        {
            get;
            set;
        }

        public bool TeleportEvent
        {
            get;
            set;
        }

        public int TeleportMapId
        {
            get;
            set;
        }

        public Map TeleportMap
        {
            get
            {
                return m_teleportMap ?? ( m_teleportMap = Game.World.Instance.GetMap(TeleportMapId) );
            }
            set
            {
                m_teleportMap = value;
                TeleportMapId = value.Id;
            }
        }

        public short TeleportCell
        {
            get;
            set;
        }

        public int DirectionInt
        {
            get;
            set;
        }

        public DirectionsEnum Direction
        {
            get { return (DirectionsEnum) DirectionInt; }
            set { DirectionInt = (int) value; }
        }

        public ObjectPosition GetTeleportPosition()
        {
            if (!TeleportEvent)
                return null;

            return new ObjectPosition(TeleportMap, TeleportCell, Direction);
        }
    }
}