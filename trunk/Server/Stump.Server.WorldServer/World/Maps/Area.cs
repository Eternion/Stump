using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.World.Maps
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
            get { return m_subAreas; }
        }

        public SuperArea SuperArea
        {
            get;
            internal set;
        }

        internal void AddSubArea(SubArea subArea)
        {
            m_subAreas.Add(subArea);

            subArea.Area = this;
        }

        internal void RemoveSubArea(SubArea subArea)
        {
            m_subAreas.Remove(subArea);

            subArea.Area = null;
        }
    }
}