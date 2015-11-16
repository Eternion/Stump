 


// Generated on 11/16/2015 14:26:39
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
    [TableName("SmileyPacks")]
    [D2OClass("SmileyPack", "com.ankamagames.dofus.datacenter.communication")]
    public class SmileyPackRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "SmileyPacks";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint order;
        public List<uint> smileys;

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
        public List<uint> Smileys
        {
            get { return smileys; }
            set
            {
                smileys = value;
                m_smileysBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_smileysBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SmileysBin
        {
            get { return m_smileysBin; }
            set
            {
                m_smileysBin = value;
                smileys = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SmileyPack)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Order = castedObj.order;
            Smileys = castedObj.smileys;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SmileyPack)parent : new SmileyPack();
            obj.id = Id;
            obj.nameId = NameId;
            obj.order = Order;
            obj.smileys = Smileys;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_smileysBin = smileys == null ? null : smileys.ToBinary();
        
        }
    }
}