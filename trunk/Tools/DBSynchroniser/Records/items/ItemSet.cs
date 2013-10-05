 


// Generated on 10/06/2013 01:10:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("ItemSets")]
    public class ItemSetRecord : ID2ORecord
    {
        private const String MODULE = "ItemSets";
        public uint id;
        public List<uint> items;
        public uint nameId;
        public List<List<EffectInstance>> effects;
        public Boolean bonusIsSecret;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [Ignore]
        public List<uint> Items
        {
            get { return items; }
            set
            {
                items = value;
                m_itemsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_itemsBin;
        public byte[] ItemsBin
        {
            get { return m_itemsBin; }
            set
            {
                m_itemsBin = value;
                items = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [Ignore]
        public List<List<EffectInstance>> Effects
        {
            get { return effects; }
            set
            {
                effects = value;
                m_effectsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_effectsBin;
        public byte[] EffectsBin
        {
            get { return m_effectsBin; }
            set
            {
                m_effectsBin = value;
                effects = value == null ? null : value.ToObject<List<List<EffectInstance>>>();
            }
        }

        public Boolean BonusIsSecret
        {
            get { return bonusIsSecret; }
            set { bonusIsSecret = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (ItemSet)obj;
            
            Id = castedObj.id;
            Items = castedObj.items;
            NameId = castedObj.nameId;
            Effects = castedObj.effects;
            BonusIsSecret = castedObj.bonusIsSecret;
        }
        
        public object CreateObject()
        {
            var obj = new ItemSet();
            
            obj.id = Id;
            obj.items = Items;
            obj.nameId = NameId;
            obj.effects = Effects;
            obj.bonusIsSecret = BonusIsSecret;
            return obj;
        
        }
    }
}