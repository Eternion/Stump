

// Generated on 09/26/2016 01:50:26
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("BonusCriterion", "com.ankamagames.dofus.datacenter.bonus.criterion")]
    [Serializable]
    public class BonusCriterion : IDataObject, IIndexedData
    {
        public const String MODULE = "BonusesCriterions";
        public int id;
        public uint type;
        public int value;
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
        public uint Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        [D2OIgnore]
        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}