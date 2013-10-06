 


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
    [TableName("Titles")]
    [D2OClass("Title", "com.ankamagames.dofus.datacenter.appearance")]
    public class TitleRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "Titles";
        public int id;
        public uint nameId;
        public Boolean visible;
        public int categoryId;

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

        [D2OIgnore]
        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        [D2OIgnore]
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Title)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Visible = castedObj.visible;
            CategoryId = castedObj.categoryId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Title)parent : new Title();
            obj.id = Id;
            obj.nameId = NameId;
            obj.visible = Visible;
            obj.categoryId = CategoryId;
            return obj;
        
        }
    }
}