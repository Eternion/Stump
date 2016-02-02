 


// Generated on 02/02/2016 14:15:14
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
    [TableName("StealthBones")]
    [D2OClass("StealthBones", "com.ankamagames.dofus.datacenter.interactives")]
    public class StealthBonesRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "StealthBones";
        public uint id;

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
            var castedObj = (StealthBones)obj;
            
            Id = castedObj.id;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (StealthBones)parent : new StealthBones();
            obj.id = Id;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}