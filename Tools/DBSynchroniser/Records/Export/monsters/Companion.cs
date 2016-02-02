 


// Generated on 02/02/2016 14:15:17
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
    [TableName("Companions")]
    [D2OClass("Companion", "com.ankamagames.dofus.datacenter.monsters")]
    public class CompanionRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Companions";
        public int id;
        [I18NField]
        public uint nameId;
        public String look;
        public Boolean webDisplay;
        [I18NField]
        public uint descriptionId;
        public uint startingSpellLevelId;
        public uint assetId;
        public List<uint> characteristics;
        public List<uint> spells;
        public int creatureBoneId;

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
        [NullString]
        public String Look
        {
            get { return look; }
            set { look = value; }
        }

        [D2OIgnore]
        public Boolean WebDisplay
        {
            get { return webDisplay; }
            set { webDisplay = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public uint StartingSpellLevelId
        {
            get { return startingSpellLevelId; }
            set { startingSpellLevelId = value; }
        }

        [D2OIgnore]
        public uint AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> Characteristics
        {
            get { return characteristics; }
            set
            {
                characteristics = value;
                m_characteristicsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_characteristicsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] CharacteristicsBin
        {
            get { return m_characteristicsBin; }
            set
            {
                m_characteristicsBin = value;
                characteristics = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> Spells
        {
            get { return spells; }
            set
            {
                spells = value;
                m_spellsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_spellsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SpellsBin
        {
            get { return m_spellsBin; }
            set
            {
                m_spellsBin = value;
                spells = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        public int CreatureBoneId
        {
            get { return creatureBoneId; }
            set { creatureBoneId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Companion)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Look = castedObj.look;
            WebDisplay = castedObj.webDisplay;
            DescriptionId = castedObj.descriptionId;
            StartingSpellLevelId = castedObj.startingSpellLevelId;
            AssetId = castedObj.assetId;
            Characteristics = castedObj.characteristics;
            Spells = castedObj.spells;
            CreatureBoneId = castedObj.creatureBoneId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Companion)parent : new Companion();
            obj.id = Id;
            obj.nameId = NameId;
            obj.look = Look;
            obj.webDisplay = WebDisplay;
            obj.descriptionId = DescriptionId;
            obj.startingSpellLevelId = StartingSpellLevelId;
            obj.assetId = AssetId;
            obj.characteristics = Characteristics;
            obj.spells = Spells;
            obj.creatureBoneId = CreatureBoneId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_characteristicsBin = characteristics == null ? null : characteristics.ToBinary();
            m_spellsBin = spells == null ? null : spells.ToBinary();
        
        }
    }
}