 


// Generated on 11/02/2013 14:55:50
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
    [TableName("MountBones")]
    [D2OClass("MountBone", "com.ankamagames.dofus.datacenter.mounts")]
    public class MountBoneRecord : ID2ORecord, ISaveIntercepter
    {
        public uint id;
        private String MODULE = "MountBones";

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
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
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (MountBone)parent : new MountBone();
            obj.id = Id;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}