using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.World.Map
{
    public class Area
    {
        private readonly List<SubArea> m_subAreas = new List<SubArea>();
        protected internal AreaRecord m_record;

        public Area(AreaRecord record)
        {
            m_record = record;
        }

        public IEnumerable<SubArea> SubAreas
        {
            get
            {
                return m_subAreas;
            }
        }

        internal void AddSubArea(SubArea subArea)
        {
            m_subAreas.Add(subArea);
        }

        internal void RemoveSubArea(SubArea subArea)
        {
            m_subAreas.Remove(subArea);
        }
    }
}