 


// Generated on 10/06/2013 14:22:02
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("MapCoordinates")]
    [D2OClass("MapCoordinates")]
    public class MapCoordinatesRecord : ID2ORecord
    {
        private const String MODULE = "MapCoordinates";
        public uint compressedCoords;
        public List<int> mapIds;

        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        public uint CompressedCoords
        {
            get { return compressedCoords; }
            set { compressedCoords = value; }
        }

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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MapCoordinates)obj;
            
            CompressedCoords = castedObj.compressedCoords;
            MapIds = castedObj.mapIds;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new MapCoordinates();
            obj.compressedCoords = CompressedCoords;
            obj.mapIds = MapIds;
            return obj;
        
        }
    }
}