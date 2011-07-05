using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.World.Maps
{
    public class SuperArea
    {
        private readonly List<Area> m_areas = new List<Area>();
        protected internal SuperAreaRecord m_record;

        public SuperArea(SuperAreaRecord record)
        {
            m_record = record;
        }

        public IEnumerable<Area> Areas
        {
            get { return m_areas; }
        }

        internal void AddArea(Area area)
        {
            m_areas.Add(area);

            area.SuperArea = this;
        }

        internal void RemoveArea(Area area)
        {
            m_areas.Remove(area);

            area.SuperArea = null;
        }
    }
}