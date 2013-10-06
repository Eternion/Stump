 


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
    [TableName("MountBones")]
    [D2OClass("MountBone")]
    public class MountBoneRecord : ID2ORecord
    {
        public uint id;
        private String MODULE = "MountBones";

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MountBone)obj;
            
            Id = castedObj.id;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new MountBone();
            obj.id = Id;
            return obj;
        
        }
    }
}