 


// Generated on 10/06/2013 14:21:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Smileys")]
    [D2OClass("Smiley")]
    public class SmileyRecord : ID2ORecord
    {
        private const String MODULE = "Smileys";
        public uint id;
        public uint order;
        public String gfxId;
        public Boolean forPlayers;
        public List<String> triggers;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        [NullString]
        public String GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

        public Boolean ForPlayers
        {
            get { return forPlayers; }
            set { forPlayers = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new Smiley();
            obj.id = Id;
            obj.order = Order;
            obj.gfxId = GfxId;
            obj.forPlayers = ForPlayers;
            obj.triggers = Triggers;
            return obj;
        
        }
    }
}