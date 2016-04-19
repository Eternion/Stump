 


// Generated on 04/19/2016 10:18:06
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
    [TableName("BonusesCriterions")]
    [D2OClass("BonusCriterion", "com.ankamagames.dofus.datacenter.bonus.criterion")]
    public class BonusCriterionRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "BonusesCriterions";
        public int id;
        public uint type;
        public int value;

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
        public uint Type
        {
            get { return type; }
            set { type = value; }
        }

        [D2OIgnore]
        public int Value
        {
            get { return value; }
            set { value = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (BonusCriterion)obj;
            
            Id = castedObj.id;
            Type = castedObj.type;
            Value = castedObj.value;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (BonusCriterion)parent : new BonusCriterion();
            obj.id = Id;
            obj.type = Type;
            obj.value = Value;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}