using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Maps
{
    public class SuperArea
    {
        private readonly List<Area> m_areas = new List<Area>();
        private readonly List<Map> m_maps = new List<Map>();
        private readonly Dictionary<Point, Map> m_mapsByPoint = new Dictionary<Point, Map>();
        private readonly List<SubArea> m_subAreas = new List<SubArea>();
        private readonly List<MonsterSpawn> m_monsterSpawns = new List<MonsterSpawn>();


        protected internal SuperAreaRecord m_record;

        public SuperArea(SuperAreaRecord record)
        {
            m_record = record;
        }

        public int Id
        {
            get { return m_record.Id; }
        }

        public string Name
        {
            get { return m_record.Name; }
        }

        public IEnumerable<Area> Areas
        {
            get { return m_areas; }
        }

        public IEnumerable<SubArea> SubAreas
        {
            get { return m_subAreas; }
        }

        public IEnumerable<Map> Maps
        {
            get { return m_maps; }
        }

        public Dictionary<Point, Map> MapsByPosition
        {
            get { return m_mapsByPoint; }
        }

        internal void AddArea(Area area)
        {
            m_areas.Add(area);
            m_subAreas.AddRange(area.SubAreas);
            m_maps.AddRange(area.Maps);

            foreach (Map map in area.Maps)
            {
                if (map.Outdoor && !m_mapsByPoint.ContainsKey(map.Position))
                    m_mapsByPoint.Add(map.Position, map);
            }

            area.SuperArea = this;
        }

        internal void RemoveArea(Area area)
        {
            m_areas.Remove(area);
            m_subAreas.RemoveAll(entry => area.SubAreas.Contains(entry));
            m_maps.RemoveAll(delegate(Map entry)
                                 {
                                     if (area.Maps.Contains(entry))
                                     {
                                         if (entry.Outdoor)
                                             m_mapsByPoint.Remove(entry.Position);

                                         return true;
                                     }

                                     return false;
                                 });

            area.SuperArea = null;
        }

        public void AddMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Add(spawn);
        }

        public void RemoveMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Remove(spawn);
        }

        public IEnumerable<MonsterSpawn> GetMonsterSpawns()
        {
            return m_monsterSpawns;
        }

    }
}