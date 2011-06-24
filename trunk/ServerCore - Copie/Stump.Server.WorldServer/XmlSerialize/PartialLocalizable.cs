
using System;
using System.Xml.Serialization;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.XmlSerialize
{
    public abstract class PartialLocalizable
    {
        private Map m_map;

        protected PartialLocalizable()
        {
        }

        protected PartialLocalizable(uint mapId)
        {
            MapId = mapId;
        }

        public uint MapId
        {
            get;
            set;
        }

        [XmlIgnore]
        public Map Map
        {
            get
            {
                if (m_map == null)
                    LoadMapFromWorld();

                return m_map;
            }
            private set { m_map = value; }
        }

        public void LoadMapFromWorld()
        {
            Map = World.Instance.GetMap(MapId);

            if (Map == null)
                throw new NullReferenceException(string.Format("Map {0} doesn't exist", MapId));
        }
    }
}