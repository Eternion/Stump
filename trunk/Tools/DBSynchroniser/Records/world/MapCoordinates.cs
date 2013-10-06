 


// Generated on 10/06/2013 18:02:19
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
    [D2OClass("MapCoordinates", "com.ankamagames.dofus.datacenter.world")]
    public class MapCoordinatesRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "MapCoordinates";
        public uint compressedCoords;
        public List<int> mapIds;

        [D2OIgnore]
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        [D2OIgnore]
        public uint CompressedCoords
        {
            get { return compressedCoords; }
            set { compressedCoords = value; }
        }

        [D2OIgnore]
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
        [D2OIgnore]
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
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (MapCoordinates)parent : new MapCoordinates();
            obj.compressedCoords = CompressedCoords;
            obj.mapIds = MapIds;
            return obj;
        
        }
    }
}