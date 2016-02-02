 


// Generated on 02/02/2016 14:15:17
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
        public int caracId;
        public int companionId;
        public int order;
        public List<List<double>> statPerLevelRange;

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
        public int CaracId
        {
            get { return caracId; }
            set { caracId = value; }
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
        [Ignore]
        public List<List<double>> StatPerLevelRange
        {
            get { return statPerLevelRange; }
            set
            {
                statPerLevelRange = value;
                m_statPerLevelRangeBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statPerLevelRangeBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] StatPerLevelRangeBin
        {
            get { return m_statPerLevelRangeBin; }
            set
            {
                m_statPerLevelRangeBin = value;
                statPerLevelRange = value == null ? null : value.ToObject<List<List<double>>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (CompanionCharacteristic)obj;
            
            Id = castedObj.id;
            CaracId = castedObj.caracId;
            CompanionId = castedObj.companionId;
            Order = castedObj.order;
            StatPerLevelRange = castedObj.statPerLevelRange;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (CompanionCharacteristic)parent : new CompanionCharacteristic();
            obj.id = Id;
            obj.caracId = CaracId;
            obj.companionId = CompanionId;
            obj.order = Order;
            obj.statPerLevelRange = StatPerLevelRange;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_statPerLevelRangeBin = statPerLevelRange == null ? null : statPerLevelRange.ToBinary();
        
        }
    }
}