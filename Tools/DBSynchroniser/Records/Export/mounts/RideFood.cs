 


// Generated on 10/26/2014 23:31:15
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
    [TableName("RideFood")]
    [D2OClass("RideFood", "com.ankamagames.dofus.datacenter.mounts")]
    public class RideFoodRecord : ID2ORecord, ISaveIntercepter
    {
        public String MODULE = "RideFood";
        public uint gid;
        public uint typeId;

        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }

        [D2OIgnore]
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OIgnore]
        public uint Gid
        {
            get { return gid; }
            set { gid = value; }
        }

        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (RideFood)obj;
            
            Gid = castedObj.gid;
            TypeId = castedObj.typeId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (RideFood)parent : new RideFood();
            obj.gid = Gid;
            obj.typeId = TypeId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}