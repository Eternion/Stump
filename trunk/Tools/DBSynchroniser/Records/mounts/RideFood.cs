 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("RideFood")]
    [D2OClass("RideFood")]
    public class RideFoodRecord : ID2ORecord
    {
        public uint gid;
        public uint typeId;
        public String MODULE = "RideFood";

        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        public uint Gid
        {
            get { return gid; }
            set { gid = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new RideFood();
            obj.gid = Gid;
            obj.typeId = TypeId;
            return obj;
        
        }
    }
}