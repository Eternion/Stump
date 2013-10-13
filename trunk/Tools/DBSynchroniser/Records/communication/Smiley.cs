 


// Generated on 10/13/2013 12:21:14
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
    public class SmileyRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "Smileys";
        public uint id;
        public uint order;
        public String gfxId;
        public Boolean forPlayers;
        public List<String> triggers;

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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Smiley)obj;
            
            Id = castedObj.id;
            Order = castedObj.order;
            GfxId = castedObj.gfxId;
            ForPlayers = castedObj.forPlayers;
            Triggers = castedObj.triggers;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Smiley)parent : new Smiley();
            obj.id = Id;
            obj.order = Order;
            obj.gfxId = GfxId;
            obj.forPlayers = ForPlayers;
            obj.triggers = Triggers;
            return obj;
        
        }
    }
}