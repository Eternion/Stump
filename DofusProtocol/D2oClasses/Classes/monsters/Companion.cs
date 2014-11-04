

// Generated on 10/26/2014 23:27:52
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Companion", "com.ankamagames.dofus.datacenter.monsters")]
    [Serializable]
    public class Companion : IDataObject, IIndexedData
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public String Look
        {
            get { return this.look; }
            set { this.look = value; }
        }
        [D2OIgnore]
        public Boolean WebDisplay
        {
            get { return this.webDisplay; }
            set { this.webDisplay = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public uint StartingSpellLevelId
        {
            get { return this.startingSpellLevelId; }
            set { this.startingSpellLevelId = value; }
        }
        [D2OIgnore]
        public uint AssetId
        {
            get { return this.assetId; }
            set { this.assetId = value; }
        }
        [D2OIgnore]
        public List<uint> Characteristics
        {
            get { return this.characteristics; }
            set { this.characteristics = value; }
        }
        [D2OIgnore]
        public List<uint> Spells
        {
            get { return this.spells; }
            set { this.spells = value; }
        }
        [D2OIgnore]
        public int CreatureBoneId
        {
            get { return this.creatureBoneId; }
            set { this.creatureBoneId = value; }
        }
    }
}