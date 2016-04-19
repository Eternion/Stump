

// Generated on 04/19/2016 10:17:56
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Bonus", "com.ankamagames.dofus.datacenter.bonus")]
    [Serializable]
    public class Bonus : IDataObject, IIndexedData
    {
        public const String MODULE = "Bonuses";
        public int id;
        public uint type;
        public int amount;
        public List<int> criterionsIds;
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
        public int Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }
        [D2OIgnore]
        public List<int> CriterionsIds
        {
            get { return this.criterionsIds; }
            set { this.criterionsIds = value; }
        }
    }
}