 


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
    [D2OClass("Titles")]
    public class TitleRecord : ID2ORecord
    {
        private const String MODULE = "Titles";
        public int id;
        public uint nameId;
        public Boolean visible;
        public int categoryId;

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

        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Title)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Visible = castedObj.visible;
            CategoryId = castedObj.categoryId;
        }
        
        public object CreateObject()
        {
            var obj = new Title();
            
            obj.id = Id;
            obj.nameId = NameId;
            obj.visible = Visible;
            obj.categoryId = CategoryId;
            return obj;
        
        }
    }
}