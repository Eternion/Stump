

// Generated on 11/16/2015 14:26:31
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("VeteranReward", "com.ankamagames.dofus.datacenter.items")]
    [Serializable]
    public class VeteranReward : IDataObject, IIndexedData
    {
        public const String MODULE = "VeteranRewards";
        public int id;
        public uint requiredSubDays;
        public uint itemGID;
        public uint itemQuantity;
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
        public uint RequiredSubDays
        {
            get { return this.requiredSubDays; }
            set { this.requiredSubDays = value; }
        }
        [D2OIgnore]
        public uint ItemGID
        {
            get { return this.itemGID; }
            set { this.itemGID = value; }
        }
        [D2OIgnore]
        public uint ItemQuantity
        {
            get { return this.itemQuantity; }
            set { this.itemQuantity = value; }
        }
    }
}