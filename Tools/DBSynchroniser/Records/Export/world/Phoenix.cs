 


// Generated on 02/02/2016 14:15:20
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
    [TableName("Phoenixes")]
    [D2OClass("Phoenix", "com.ankamagames.dofus.datacenter.world")]
    public class PhoenixRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Phoenixes";
        public uint mapId;

        int ID2ORecord.Id
        {
            get { return (int)mapId; }
        }


        [D2OIgnore]
        [PrimaryKey("MapId", false)]
        public uint MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Phoenix)obj;
            
            MapId = castedObj.mapId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Phoenix)parent : new Phoenix();
            obj.mapId = MapId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}