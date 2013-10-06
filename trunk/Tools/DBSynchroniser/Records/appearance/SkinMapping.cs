 


// Generated on 10/06/2013 18:02:16
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SkinMappings")]
    [D2OClass("SkinMapping", "com.ankamagames.dofus.datacenter.appearance")]
    public class SkinMappingRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "SkinMappings";
        public int id;
        public int lowDefId;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int LowDefId
        {
            get { return lowDefId; }
            set { lowDefId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SkinMapping)obj;
            
            Id = castedObj.id;
            LowDefId = castedObj.lowDefId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (SkinMapping)parent : new SkinMapping();
            obj.id = Id;
            obj.lowDefId = LowDefId;
            return obj;
        
        }
    }
}