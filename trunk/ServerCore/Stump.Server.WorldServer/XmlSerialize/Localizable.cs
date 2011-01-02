using System.Xml.Serialization;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.XmlSerialize
{
    public abstract class Localizable
    {
        private Map m_map;

        protected Localizable()
        {
            
        }

        protected Localizable(ushort cellId, uint mapId)
        {
            CellId = cellId;
            MapId = mapId;
        }

        public ushort CellId
        {
            get;
            set;
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
            private set
            {
                m_map = value;
            }
        }

        [XmlIgnore]
        public CellData Cell
        {
            get { return Map.CellsData[CellId]; }
        }

        public void LoadMapFromWorld()
        {
            Map = World.Instance.GetMap(MapId);
        }
    }
}