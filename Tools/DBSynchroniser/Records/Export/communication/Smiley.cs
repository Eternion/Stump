 


// Generated on 12/20/2015 18:16:38
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
    [TableName("Smileys")]
    [D2OClass("Smiley", "com.ankamagames.dofus.datacenter.communication")]
    public class SmileyRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Smileys";
        public uint id;
        public uint order;
        public String gfxId;
        public Boolean forPlayers;
        public List<String> triggers;
        public uint referenceId;
        public uint categoryId;

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
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        [NullString]
        public String GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

        [D2OIgnore]
        public Boolean ForPlayers
        {
            get { return forPlayers; }
            set { forPlayers = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<String> Triggers
        {
            get { return triggers; }
            set
            {
                triggers = value;
                m_triggersBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_triggersBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] TriggersBin
        {
            get { return m_triggersBin; }
            set
            {
                m_triggersBin = value;
                triggers = value == null ? null : value.ToObject<List<String>>();
            }
        }

        [D2OIgnore]
        public uint ReferenceId
        {
            get { return referenceId; }
            set { referenceId = value; }
        }

        [D2OIgnore]
        public uint CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Smiley)obj;
            
            Id = castedObj.id;
            Order = castedObj.order;
            GfxId = castedObj.gfxId;
            ForPlayers = castedObj.forPlayers;
            Triggers = castedObj.triggers;
            ReferenceId = castedObj.referenceId;
            CategoryId = castedObj.categoryId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Smiley)parent : new Smiley();
            obj.id = Id;
            obj.order = Order;
            obj.gfxId = GfxId;
            obj.forPlayers = ForPlayers;
            obj.triggers = Triggers;
            obj.referenceId = ReferenceId;
            obj.categoryId = CategoryId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_triggersBin = triggers == null ? null : triggers.ToBinary();
        
        }
    }
}