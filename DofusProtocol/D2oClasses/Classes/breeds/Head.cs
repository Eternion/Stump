

// Generated on 08/13/2015 17:13:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Head", "com.ankamagames.dofus.datacenter.breeds")]
    [Serializable]
    public class Head : IDataObject, IIndexedData
    {
        public const String MODULE = "Heads";
        public int id;
        public String skins;
        public String assetId;
        public uint breed;
        public uint gender;
        public String label;
        public uint order;
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
        public String Skins
        {
            get { return this.skins; }
            set { this.skins = value; }
        }
        [D2OIgnore]
        public String AssetId
        {
            get { return this.assetId; }
            set { this.assetId = value; }
        }
        [D2OIgnore]
        public uint Breed
        {
            get { return this.breed; }
            set { this.breed = value; }
        }
        [D2OIgnore]
        public uint Gender
        {
            get { return this.gender; }
            set { this.gender = value; }
        }
        [D2OIgnore]
        public String Label
        {
            get { return this.label; }
            set { this.label = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}