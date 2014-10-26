 


// Generated on 10/26/2014 23:31:12
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
    [TableName("TitleCategories")]
    [D2OClass("TitleCategory", "com.ankamagames.dofus.datacenter.appearance")]
    public class TitleCategoryRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "TitleCategories";
        public int id;
        [I18NField]
        public uint nameId;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (TitleCategory)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (TitleCategory)parent : new TitleCategory();
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}