using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Worlds.Maps
{
    public class Area
    {
        private readonly List<Map> m_maps = new List<Map>();
        private readonly List<SubArea> m_subAreas = new List<SubArea>();

        protected internal AreaRecord Record;

        public Area(AreaRecord record)
        {
            Record = record;
        }

        public IEnumerable<SubArea> SubAreas
        {
            get { return m_subAreas; }
        }

        public IEnumerable<Map> Maps
        {
            get { return m_maps; }
        }

        public SuperArea SuperArea
        {
            get;
            internal set;
        }

        internal void AddSubArea(SubArea subArea)
        {
            m_subAreas.Add(subArea);
            m_maps.AddRange(subArea.Maps);

            subArea.Area = this;
        }

        internal void RemoveSubArea(SubArea subArea)
        {
            m_subAreas.Remove(subArea);
            m_maps.RemoveAll(entry => subArea.Maps.Contains(entry));

            subArea.Area = null;
        }
    }
}