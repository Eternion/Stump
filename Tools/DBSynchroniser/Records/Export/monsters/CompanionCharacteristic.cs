 


// Generated on 10/26/2014 23:31:15
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
    [TableName("CompanionCharacteristics")]
    [D2OClass("CompanionCharacteristic", "com.ankamagames.dofus.datacenter.monsters")]
    public class CompanionCharacteristicRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "CompanionCharacteristics";
        public int id;
        public String labelId;
        public int companionId;
        public int order;
        public int initialValue;
        public int levelPerValue;
        public int valuePerLevel;

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
        [NullString]
        public String LabelId
        {
            get { return labelId; }
            set { labelId = value; }
        }

        [D2OIgnore]
        public int CompanionId
        {
            get { return companionId; }
            set { companionId = value; }
        }

        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        public int InitialValue
        {
            get { return initialValue; }
            set { initialValue = value; }
        }

        [D2OIgnore]
        public int LevelPerValue
        {
            get { return levelPerValue; }
            set { levelPerValue = value; }
        }

        [D2OIgnore]
        public int ValuePerLevel
        {
            get { return valuePerLevel; }
            set { valuePerLevel = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (CompanionCharacteristic)obj;
            
            Id = castedObj.id;
            LabelId = castedObj.labelId;
            CompanionId = castedObj.companionId;
            Order = castedObj.order;
            InitialValue = castedObj.initialValue;
            LevelPerValue = castedObj.levelPerValue;
            ValuePerLevel = castedObj.valuePerLevel;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (CompanionCharacteristic)parent : new CompanionCharacteristic();
            obj.id = Id;
            obj.labelId = LabelId;
            obj.companionId = CompanionId;
            obj.order = Order;
            obj.initialValue = InitialValue;
            obj.levelPerValue = LevelPerValue;
            obj.valuePerLevel = ValuePerLevel;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}