 


// Generated on 09/01/2015 10:48:46
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
    [TableName("CharacteristicCategories")]
    [D2OClass("CharacteristicCategory", "com.ankamagames.dofus.datacenter.characteristics")]
    public class CharacteristicCategoryRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "CharacteristicCategories";
        public int id;
        [I18NField]
        public uint nameId;
        public int order;
        public List<uint> characteristicIds;

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
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> CharacteristicIds
        {
            get { return characteristicIds; }
            set
            {
                characteristicIds = value;
                m_characteristicIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_characteristicIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] CharacteristicIdsBin
        {
            get { return m_characteristicIdsBin; }
            set
            {
                m_characteristicIdsBin = value;
                characteristicIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (CharacteristicCategory)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Order = castedObj.order;
            CharacteristicIds = castedObj.characteristicIds;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (CharacteristicCategory)parent : new CharacteristicCategory();
            obj.id = Id;
            obj.nameId = NameId;
            obj.order = Order;
            obj.characteristicIds = CharacteristicIds;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_characteristicIdsBin = characteristicIds == null ? null : characteristicIds.ToBinary();
        
        }
    }
}