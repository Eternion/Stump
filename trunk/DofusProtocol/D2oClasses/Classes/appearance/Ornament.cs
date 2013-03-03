
// Generated on 03/02/2013 21:17:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Ornaments")]
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public int AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public int Rarity
        {
            get { return rarity; }
            set { rarity = value; }
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

    }
}