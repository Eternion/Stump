 


// Generated on 10/06/2013 01:10:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("SkinMappings")]
    public class SkinMappingRecord : ID2ORecord
    {
        private const String MODULE = "SkinMappings";
        public int id;
        public int lowDefId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int LowDefId
        {
            get { return lowDefId; }
            set { lowDefId = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (SkinMapping)obj;
            
            Id = castedObj.id;
            LowDefId = castedObj.lowDefId;
        }
        
        public object CreateObject()
        {
            var obj = new SkinMapping();
            
            obj.id = Id;
            obj.lowDefId = LowDefId;
            return obj;
        
        }
    }
}