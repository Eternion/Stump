 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("RideFood")]
    public class RideFoodRecord : ID2ORecord
    {
        public uint gid;
        public uint typeId;
        public String MODULE = "RideFood";

        [PrimaryKey("Gid", false)]
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

        public void AssignFields(object obj)
        {
            var castedObj = (RideFood)obj;
            
            Gid = castedObj.gid;
            TypeId = castedObj.typeId;
        }
        
        public object CreateObject()
        {
            var obj = new RideFood();
            
            obj.gid = Gid;
            obj.typeId = TypeId;
            return obj;
        
        }
    }
}