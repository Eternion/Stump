 


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
    [TableName("AlignmentRank")]
    [D2OClass("AlignmentRank", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentRankRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "AlignmentRank";
        public int id;
        public uint orderId;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descriptionId;
        public int minimumAlignment;
        public int objectsStolen;
        public List<int> gifts;

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
        public uint OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public int MinimumAlignment
        {
            get { return minimumAlignment; }
            set { minimumAlignment = value; }
        }

        [D2OIgnore]
        public int ObjectsStolen
        {
            get { return objectsStolen; }
            set { objectsStolen = value; }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
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
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (AlignmentRank)parent : new AlignmentRank();
            obj.id = Id;
            obj.orderId = OrderId;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.minimumAlignment = MinimumAlignment;
            obj.objectsStolen = ObjectsStolen;
            obj.gifts = Gifts;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_giftsBin = gifts == null ? null : gifts.ToBinary();
        
        }
    }
}