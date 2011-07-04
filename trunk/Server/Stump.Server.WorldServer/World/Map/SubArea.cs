using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.World.Map
{
    public class SubArea
    {
        private readonly List<Map> m_maps = new List<Map>();
        protected internal SubAreaRecord m_record;

        public SubArea(SubAreaRecord record)
        {
            m_record = record;
        }

        public int Id
        {
            get { return m_record.Id; }
        }

        public IEnumerable<Map> Maps
        {
            get { return m_maps; }
        }
        
        internal void AddMap(Map map)
        {
            m_maps.Add(map);
        }

        internal void RemoveMap(Map map)
        {
            m_maps.Remove(map);
        }
    }
}