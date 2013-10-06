 


// Generated on 10/06/2013 14:21:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("StealthBones")]
    [D2OClass("StealthBones")]
    public class StealthBonesRecord : ID2ORecord
    {
        private const String MODULE = "StealthBones";
        public uint id;

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
        
        public virtual object CreateObject()
        {
            
            var obj = new StealthBones();
            obj.id = Id;
            return obj;
        
        }
    }
}