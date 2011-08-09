using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Worlds.Maps
{
    public class SubArea
    {
        private readonly List<Map> m_maps = new List<Map>();
        protected internal SubAreaRecord Record;

        public SubArea(SubAreaRecord record)
        {
            Record = record;
        }

        public int Id
        {
            get { return Record.Id; }
        }

        public IEnumerable<Map> Maps
        {
            get { return m_maps; }
        }

        public Area Area
        {
            get;
            internal set;
        }
        
        internal void AddMap(Map map)
        {
            m_maps.Add(map);

            map.SubArea = this;
        }

        internal void RemoveMap(Map map)
        {
            m_maps.Remove(map);

            map.SubArea = null;
        }
    }
}