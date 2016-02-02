

// Generated on 02/02/2016 14:15:04
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
        public int caracId;
        public int companionId;
        public int order;
        public List<List<double>> statPerLevelRange;
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
        public int CaracId
        {
            get { return this.caracId; }
            set { this.caracId = value; }
        }
        [D2OIgnore]
        public int CompanionId
        {
            get { return this.companionId; }
            set { this.companionId = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public List<List<double>> StatPerLevelRange
        {
            get { return this.statPerLevelRange; }
            set { this.statPerLevelRange = value; }
        }
    }
}