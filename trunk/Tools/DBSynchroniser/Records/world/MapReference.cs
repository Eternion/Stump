 


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
    [TableName("MapReferences")]
    [D2OClass("MapReference")]
    public class MapReferenceRecord : ID2ORecord
    {
        private const String MODULE = "MapReferences";
        public int id;
        public uint mapId;
        public int cellId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }

        public int CellId
        {
            get { return cellId; }
            set { cellId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MapReference)obj;
            
            Id = castedObj.id;
            MapId = castedObj.mapId;
            CellId = castedObj.cellId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new MapReference();
            obj.id = Id;
            obj.mapId = MapId;
            obj.cellId = CellId;
            return obj;
        
        }
    }
}