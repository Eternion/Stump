 


// Generated on 09/26/2016 01:50:39
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
    [TableName("Bonuses")]
    [D2OClass("Bonus", "com.ankamagames.dofus.datacenter.bonus")]
    public class BonusRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Bonuses";
        public int id;
        public uint type;
        public int amount;
        public List<int> criterionsIds;

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
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> CriterionsIds
        {
            get { return criterionsIds; }
            set
            {
                criterionsIds = value;
                m_criterionsIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_criterionsIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] CriterionsIdsBin
        {
            get { return m_criterionsIdsBin; }
            set
            {
                m_criterionsIdsBin = value;
                criterionsIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Bonus)obj;
            
            Id = castedObj.id;
            Type = castedObj.type;
            Amount = castedObj.amount;
            CriterionsIds = castedObj.criterionsIds;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Bonus)parent : new Bonus();
            obj.id = Id;
            obj.type = Type;
            obj.amount = Amount;
            obj.criterionsIds = CriterionsIds;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_criterionsIdsBin = criterionsIds == null ? null : criterionsIds.ToBinary();
        
        }
    }
}