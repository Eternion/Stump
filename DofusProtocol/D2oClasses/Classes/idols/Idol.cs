

// Generated on 08/13/2015 17:13:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Idol", "com.ankamagames.dofus.datacenter.idols")]
    [Serializable]
    public class Idol : IDataObject, IIndexedData
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
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public String Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        [D2OIgnore]
        public int CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
        [D2OIgnore]
        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }
        [D2OIgnore]
        public Boolean GroupOnly
        {
            get { return this.groupOnly; }
            set { this.groupOnly = value; }
        }
        [D2OIgnore]
        public int SpellPairId
        {
            get { return this.spellPairId; }
            set { this.spellPairId = value; }
        }
        [D2OIgnore]
        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }
        [D2OIgnore]
        public int ExperienceBonus
        {
            get { return this.experienceBonus; }
            set { this.experienceBonus = value; }
        }
        [D2OIgnore]
        public int DropBonus
        {
            get { return this.dropBonus; }
            set { this.dropBonus = value; }
        }
        [D2OIgnore]
        public List<int> SynergyIdolsIds
        {
            get { return this.synergyIdolsIds; }
            set { this.synergyIdolsIds = value; }
        }
        [D2OIgnore]
        public List<double> SynergyIdolsCoeff
        {
            get { return this.synergyIdolsCoeff; }
            set { this.synergyIdolsCoeff = value; }
        }
    }
}