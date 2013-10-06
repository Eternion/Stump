 


// Generated on 10/06/2013 14:22:02
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("HintCategory")]
    [D2OClass("HintCategory")]
    public class HintCategoryRecord : ID2ORecord
    {
        private const String MODULE = "HintCategory";
        public int id;
        public uint nameId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (HintCategory)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new HintCategory();
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}