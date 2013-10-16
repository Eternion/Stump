 


// Generated on 10/13/2013 12:21:15
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
    public class StealthBonesRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "StealthBones";
        public uint id;

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
    }
}