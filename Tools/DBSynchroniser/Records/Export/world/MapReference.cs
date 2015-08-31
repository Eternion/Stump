 


// Generated on 08/13/2015 17:50:48
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("MapReferences")]
    [D2OClass("MapReference", "com.ankamagames.dofus.datacenter.world")]
    public class MapReferenceRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "MapReferences";
        public int id;
        public uint mapId;
        public int cellId;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public uint MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }

        [D2OIgnore]
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
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (MapReference)parent : new MapReference();
            obj.id = Id;
            obj.mapId = MapId;
            obj.cellId = CellId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}