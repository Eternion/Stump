 


// Generated on 10/06/2013 01:11:00
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("MapCoordinates")]
    public class MapCoordinatesRecord : ID2ORecord
    {
        private const String MODULE = "MapCoordinates";
        public uint compressedCoords;
        public List<int> mapIds;

        public uint CompressedCoords
        {
            get { return compressedCoords; }
            set { compressedCoords = value; }
        }

        [PrimaryKey("MapIds", false)]
        [Ignore]
        public List<int> MapIds
        {
            get { return mapIds; }
            set
            {
                mapIds = value;
                m_mapIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_mapIdsBin;
        public byte[] MapIdsBin
        {
            get { return m_mapIdsBin; }
            set
            {
                m_mapIdsBin = value;
                mapIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (MapCoordinates)obj;
            
            CompressedCoords = castedObj.compressedCoords;
            MapIds = castedObj.mapIds;
        }
        
        public object CreateObject()
        {
            var obj = new MapCoordinates();
            
            obj.compressedCoords = CompressedCoords;
            obj.mapIds = MapIds;
            return obj;
        
        }
    }
}