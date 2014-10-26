 


// Generated on 10/26/2014 23:31:16
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
    [TableName("HintCategory")]
    [D2OClass("HintCategory", "com.ankamagames.dofus.datacenter.world")]
    public class HintCategoryRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "HintCategory";
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
            var castedObj = (HintCategory)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (HintCategory)parent : new HintCategory();
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}