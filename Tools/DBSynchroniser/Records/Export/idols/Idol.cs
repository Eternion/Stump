 


// Generated on 08/13/2015 17:50:44
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
    [TableName("Idols")]
    [D2OClass("Idol", "com.ankamagames.dofus.datacenter.idols")]
    public class IdolRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Idols";
        public int id;
        public String description;
        public int categoryId;
        public int itemId;
        public Boolean groupOnly;
        public int spellPairId;
        public int score;
        public int experienceBonus;
        public int dropBonus;
        public List<int> synergyIdolsIds;
        public List<double> synergyIdolsCoeff;

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
        [NullString]
        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        [D2OIgnore]
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        [D2OIgnore]
        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        [D2OIgnore]
        public Boolean GroupOnly
        {
            get { return groupOnly; }
            set { groupOnly = value; }
        }

        [D2OIgnore]
        public int SpellPairId
        {
            get { return spellPairId; }
            set { spellPairId = value; }
        }

        [D2OIgnore]
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        [D2OIgnore]
        public int ExperienceBonus
        {
            get { return experienceBonus; }
            set { experienceBonus = value; }
        }

        [D2OIgnore]
        public int DropBonus
        {
            get { return dropBonus; }
            set { dropBonus = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> SynergyIdolsIds
        {
            get { return synergyIdolsIds; }
            set
            {
                synergyIdolsIds = value;
                m_synergyIdolsIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_synergyIdolsIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SynergyIdolsIdsBin
        {
            get { return m_synergyIdolsIdsBin; }
            set
            {
                m_synergyIdolsIdsBin = value;
                synergyIdolsIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<double> SynergyIdolsCoeff
        {
            get { return synergyIdolsCoeff; }
            set
            {
                synergyIdolsCoeff = value;
                m_synergyIdolsCoeffBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_synergyIdolsCoeffBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SynergyIdolsCoeffBin
        {
            get { return m_synergyIdolsCoeffBin; }
            set
            {
                m_synergyIdolsCoeffBin = value;
                synergyIdolsCoeff = value == null ? null : value.ToObject<List<double>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Idol)obj;
            
            Id = castedObj.id;
            Description = castedObj.description;
            CategoryId = castedObj.categoryId;
            ItemId = castedObj.itemId;
            GroupOnly = castedObj.groupOnly;
            SpellPairId = castedObj.spellPairId;
            Score = castedObj.score;
            ExperienceBonus = castedObj.experienceBonus;
            DropBonus = castedObj.dropBonus;
            SynergyIdolsIds = castedObj.synergyIdolsIds;
            SynergyIdolsCoeff = castedObj.synergyIdolsCoeff;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Idol)parent : new Idol();
            obj.id = Id;
            obj.description = Description;
            obj.categoryId = CategoryId;
            obj.itemId = ItemId;
            obj.groupOnly = GroupOnly;
            obj.spellPairId = SpellPairId;
            obj.score = Score;
            obj.experienceBonus = ExperienceBonus;
            obj.dropBonus = DropBonus;
            obj.synergyIdolsIds = SynergyIdolsIds;
            obj.synergyIdolsCoeff = SynergyIdolsCoeff;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_synergyIdolsIdsBin = synergyIdolsIds == null ? null : synergyIdolsIds.ToBinary();
            m_synergyIdolsCoeffBin = synergyIdolsCoeff == null ? null : synergyIdolsCoeff.ToBinary();
        
        }
    }
}