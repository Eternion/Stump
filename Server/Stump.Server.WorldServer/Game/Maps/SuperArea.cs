using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly Dictionary<Point, List<Map>> m_mapsByPoint = new Dictionary<Point, List<Map>>();
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

        public Dictionary<Point, List<Map>> MapsByPosition
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
                if (!m_mapsByPoint.ContainsKey(map.Position))
                    m_mapsByPoint.Add(map.Position, new List<Map>());

                m_mapsByPoint[map.Position].Add(map);
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
                                         if (m_mapsByPoint.ContainsKey(entry.Position))
                                         {
                                             var list = m_mapsByPoint[entry.Position];
                                             list.Remove(entry);

                                             if (list.Count <= 0)
                                                 m_mapsByPoint.Remove(entry.Position);
                                         }

                                         return true;
                                     }

                                     return false;
                                 });

            area.SuperArea = null;
        }

        public Map[] GetMaps(int x, int y)
        {
            return GetMaps(new Point(x, y));
        }

        public Map[] GetMaps(int x, int y, bool outdoor)
        {
            return GetMaps(new Point(x, y), outdoor);
        }

        public Map[] GetMaps(Point position)
        {
            if (!m_mapsByPoint.ContainsKey(position))
                return new Map[0];

            return m_mapsByPoint[position].ToArray();
        }

        public Map[] GetMaps(Point position, bool outdoor)
        {
            if (!m_mapsByPoint.ContainsKey(position))
                return new Map[0];

            return m_mapsByPoint[position].Where(entry => entry.Outdoor == outdoor).ToArray();
        }

        public void AddMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Add(spawn);

            foreach (var area in Areas)
            {
                area.AddMonsterSpawn(spawn);
            }
        }

        public void RemoveMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Remove(spawn);
            
            foreach (var area in Areas)
            {
                area.RemoveMonsterSpawn(spawn);
            }
        }

        public ReadOnlyCollection<MonsterSpawn> MonsterSpawns
        {
            get
            {
                return m_monsterSpawns.AsReadOnly();
            }
        }

    }
}