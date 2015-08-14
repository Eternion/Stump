 


// Generated on 08/13/2015 17:50:47
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
    [TableName("QuestCategory")]
    [D2OClass("QuestCategory", "com.ankamagames.dofus.datacenter.quest")]
    public class QuestCategoryRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "QuestCategory";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint order;
        public List<uint> questIds;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
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

        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> QuestIds
        {
            get { return questIds; }
            set
            {
                questIds = value;
                m_questIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_questIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] QuestIdsBin
        {
            get { return m_questIdsBin; }
            set
            {
                m_questIdsBin = value;
                questIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (QuestCategory)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Order = castedObj.order;
            QuestIds = castedObj.questIds;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (QuestCategory)parent : new QuestCategory();
            obj.id = Id;
            obj.nameId = NameId;
            obj.order = Order;
            obj.questIds = QuestIds;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_questIdsBin = questIds == null ? null : questIds.ToBinary();
        
        }
    }
}