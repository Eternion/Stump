 


// Generated on 10/06/2013 14:21:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("AlignmentRank")]
    [D2OClass("AlignmentRank")]
    public class AlignmentRankRecord : ID2ORecord
    {
        private const String MODULE = "AlignmentRank";
        public int id;
        public uint orderId;
        public uint nameId;
        public uint descriptionId;
        public int minimumAlignment;
        public int objectsStolen;
        public List<int> gifts;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public int MinimumAlignment
        {
            get { return minimumAlignment; }
            set { minimumAlignment = value; }
        }

        public int ObjectsStolen
        {
            get { return objectsStolen; }
            set { objectsStolen = value; }
        }

        [Ignore]
        public List<int> Gifts
        {
            get { return gifts; }
            set
            {
                gifts = value;
                m_giftsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_giftsBin;
        public byte[] GiftsBin
        {
            get { return m_giftsBin; }
            set
            {
                m_giftsBin = value;
                gifts = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AlignmentRank)obj;
            
            Id = castedObj.id;
            OrderId = castedObj.orderId;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
            MinimumAlignment = castedObj.minimumAlignment;
            ObjectsStolen = castedObj.objectsStolen;
            Gifts = castedObj.gifts;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new AlignmentRank();
            obj.id = Id;
            obj.orderId = OrderId;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.minimumAlignment = MinimumAlignment;
            obj.objectsStolen = ObjectsStolen;
            obj.gifts = Gifts;
            return obj;
        
        }
    }
}