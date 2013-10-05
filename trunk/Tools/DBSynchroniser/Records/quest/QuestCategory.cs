 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("QuestCategory")]
    public class QuestCategoryRecord : ID2ORecord
    {
        private const String MODULE = "QuestCategory";
        public uint id;
        public uint nameId;
        public uint order;
        public List<uint> questIds;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

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
        public byte[] QuestIdsBin
        {
            get { return m_questIdsBin; }
            set
            {
                m_questIdsBin = value;
                questIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (QuestCategory)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Order = castedObj.order;
            QuestIds = castedObj.questIds;
        }
        
        public object CreateObject()
        {
            var obj = new QuestCategory();
            
            obj.id = Id;
            obj.nameId = NameId;
            obj.order = Order;
            obj.questIds = QuestIds;
            return obj;
        
        }
    }
}