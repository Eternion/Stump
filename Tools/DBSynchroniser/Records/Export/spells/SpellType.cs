 


// Generated on 09/01/2015 10:48:50
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
    [TableName("SpellTypes")]
    [D2OClass("SpellType", "com.ankamagames.dofus.datacenter.spells")]
    public class SpellTypeRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "SpellTypes";
        public int id;
        [I18NField]
        public uint longNameId;
        [I18NField]
        public uint shortNameId;

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
        public uint LongNameId
        {
            get { return longNameId; }
            set { longNameId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint ShortNameId
        {
            get { return shortNameId; }
            set { shortNameId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SpellType)obj;
            
            Id = castedObj.id;
            LongNameId = castedObj.longNameId;
            ShortNameId = castedObj.shortNameId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SpellType)parent : new SpellType();
            obj.id = Id;
            obj.longNameId = LongNameId;
            obj.shortNameId = ShortNameId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}