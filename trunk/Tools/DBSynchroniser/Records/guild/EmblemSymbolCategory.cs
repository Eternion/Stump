 


// Generated on 10/06/2013 14:21:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("EmblemSymbolCategories")]
    [D2OClass("EmblemSymbolCategory")]
    public class EmblemSymbolCategoryRecord : ID2ORecord
    {
        private const String MODULE = "EmblemSymbolCategories";
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
            var castedObj = (EmblemSymbolCategory)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new EmblemSymbolCategory();
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}