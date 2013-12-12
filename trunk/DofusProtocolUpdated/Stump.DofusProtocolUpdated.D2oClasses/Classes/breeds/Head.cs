

// Generated on 12/12/2013 16:57:37
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public String Skins
        {
            get { return skins; }
            set { skins = value; }
        }
        [D2OIgnore]
        public String AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }
        [D2OIgnore]
        public uint Breed
        {
            get { return breed; }
            set { breed = value; }
        }
        [D2OIgnore]
        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        [D2OIgnore]
        public String Label
        {
            get { return label; }
            set { label = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }
    }
}