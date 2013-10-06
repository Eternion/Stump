

// Generated on 10/06/2013 17:58:52
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Ornament", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class Ornament : IDataObject, IIndexedData
    {
        private const String MODULE = "Ornaments";
        public int id;
        public uint nameId;
        public Boolean visible;
        public int assetId;
        public int iconId;
        public int rarity;
        public int order;
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
        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        [D2OIgnore]
        public int AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }
        [D2OIgnore]
        public int Rarity
        {
            get { return rarity; }
            set { rarity = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }
    }
}