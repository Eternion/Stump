 


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
    [D2OClass("TitleCategories")]
    public class TitleCategoryRecord : ID2ORecord
    {
        private const String MODULE = "TitleCategories";
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

        public void AssignFields(object obj)
        {
            var castedObj = (TitleCategory)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
        }
        
        public object CreateObject()
        {
            var obj = new TitleCategory();
            
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}