using System;
using System.Collections;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Monsters
{
    [ActiveRecord("monsters_spawns_dungeons")]
    public class MonsterDungeonSpawn : WorldBaseRecord<MonsterDungeonSpawn>
    {
        private List<MonsterGrade> m_groupMonsters = new List<MonsterGrade>();
        private Map m_map; 
        private Map m_teleportMap;
        private byte[] m_serializedMonsterGroup;

        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property(NotNull = true)]
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


        [Property(NotNull = true)]
        private byte[] SerializedMonsterGroup
        {
            get { return m_serializedMonsterGroup; }
            set
            {
                m_serializedMonsterGroup = value;
                GroupMonsters = UnSerializeGroup(value);
            }
        }

        public List<MonsterGrade> GroupMonsters
        {
            get { return m_groupMonsters; }
            set { m_groupMonsters = value; }
        }

        private byte[] SerializeGroup(List<MonsterGrade> group)
        {
            var result = new byte[group.Count*4];

            for (int i = 0; i < group.Count; i++)
            {
                result[i*4] = (byte) (group[i].Id >> 24);
                result[i*4 + 1] = (byte) ((group[i].Id >> 16) & 0xFF);
                result[i*4 + 2] = (byte) ((group[i].Id >> 8) & 0xFF);
                result[i*4 + 3] = (byte) (group[i].Id & 0xFF);
            }

            return result;
        }

        [Property]
        public bool TeleportEvent
        {
            get;
            set;
        }

        [Property]
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

        [Property]
        public short TeleportCell
        {
            get;
            set;
        }

        [Property]
        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public ObjectPosition GetTeleportPosition()
        {
            if (!TeleportEvent)
                return null;

            return new ObjectPosition(TeleportMap, TeleportCell, Direction);
        }

        private List<MonsterGrade> UnSerializeGroup(byte[] serialized)
        {
            var result = new List<MonsterGrade>();

            for (int i = 0; i < serialized.Length; i += 4)
            {
                int id = serialized[i] << 24 | serialized[i + 1] << 16 | serialized[i + 2] << 8 | serialized[i + 3];

                MonsterGrade grade = MonsterManager.Instance.GetMonsterGrade(id);

                if (grade == null)
                    throw new Exception("Grade " + id + " not found");

                result.Add(grade);
            }

            return result;
        }

        protected override bool OnFlushDirty(object id, IDictionary previousState, IDictionary currentState, NHibernate.Type.IType[] types)
        {
            SerializedMonsterGroup = (byte[])(currentState["SerializedMonsterGroup"] = SerializeGroup(GroupMonsters));

            return base.OnFlushDirty(id, previousState, currentState, types);
        }

        protected override bool BeforeSave(IDictionary state)
        {
            SerializedMonsterGroup = (byte[])( state["SerializedMonsterGroup"] = SerializeGroup(GroupMonsters) );

            return base.BeforeSave(state);
        }
    }
}