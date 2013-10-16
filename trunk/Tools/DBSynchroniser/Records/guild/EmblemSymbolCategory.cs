 


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
    [TableName("EmblemSymbolCategories")]
    [D2OClass("EmblemSymbolCategory", "com.ankamagames.dofus.datacenter.guild")]
    public class EmblemSymbolCategoryRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "EmblemSymbolCategories";
        public int id;
        public uint nameId;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
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
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (EmblemSymbolCategory)parent : new EmblemSymbolCategory();
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}