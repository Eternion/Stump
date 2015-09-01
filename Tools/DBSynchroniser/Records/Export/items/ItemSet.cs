 


// Generated on 09/01/2015 10:48:48
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
    [TableName("ItemSets")]
    [D2OClass("ItemSet", "com.ankamagames.dofus.datacenter.items")]
    public class ItemSetRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "ItemSets";
        public uint id;
        public List<uint> items;
        [I18NField]
        public uint nameId;
        public List<List<EffectInstance>> effects;
        public Boolean bonusIsSecret;

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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ItemsBin
        {
            get { return m_itemsBin; }
            set
            {
                m_itemsBin = value;
                items = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] EffectsBin
        {
            get { return m_effectsBin; }
            set
            {
                m_effectsBin = value;
                effects = value == null ? null : value.ToObject<List<List<EffectInstance>>>();
            }
        }

        [D2OIgnore]
        public Boolean BonusIsSecret
        {
            get { return bonusIsSecret; }
            set { bonusIsSecret = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (ItemSet)obj;
            
            Id = castedObj.id;
            Items = castedObj.items;
            NameId = castedObj.nameId;
            Effects = castedObj.effects;
            BonusIsSecret = castedObj.bonusIsSecret;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (ItemSet)parent : new ItemSet();
            obj.id = Id;
            obj.items = Items;
            obj.nameId = NameId;
            obj.effects = Effects;
            obj.bonusIsSecret = BonusIsSecret;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_itemsBin = items == null ? null : items.ToBinary();
            m_effectsBin = effects == null ? null : effects.ToBinary();
        
        }
    }
}