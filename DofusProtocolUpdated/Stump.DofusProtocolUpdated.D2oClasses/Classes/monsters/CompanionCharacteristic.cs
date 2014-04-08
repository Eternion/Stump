

// Generated on 12/12/2013 16:57:41
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("CompanionCharacteristic", "com.ankamagames.dofus.datacenter.monsters")]
    [Serializable]
    public class CompanionCharacteristic : IDataObject, IIndexedData
    {
        public const String MODULE = "CompanionCharacteristics";
        public int id;
        public String labelId;
        public int companionId;
        public int order;
        public int initialValue;
        public int levelPerValue;
        public int valuePerLevel;
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
        public String LabelId
        {
            get { return labelId; }
            set { labelId = value; }
        }
        [D2OIgnore]
        public int CompanionId
        {
            get { return companionId; }
            set { companionId = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }
        [D2OIgnore]
        public int InitialValue
        {
            get { return initialValue; }
            set { initialValue = value; }
        }
        [D2OIgnore]
        public int LevelPerValue
        {
            get { return levelPerValue; }
            set { levelPerValue = value; }
        }
        [D2OIgnore]
        public int ValuePerLevel
        {
            get { return valuePerLevel; }
            set { valuePerLevel = value; }
        }
    }
}