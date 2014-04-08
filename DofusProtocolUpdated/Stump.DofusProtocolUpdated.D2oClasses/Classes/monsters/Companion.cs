

// Generated on 12/12/2013 16:57:41
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
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
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
        public List<uint> Characteristics
        {
            get { return characteristics; }
            set { characteristics = value; }
        }
        [D2OIgnore]
        public List<uint> Spells
        {
            get { return spells; }
            set { spells = value; }
        }
    }
}