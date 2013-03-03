
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Houses")]
    [Serializable]
    public class House : IDataObject, IIndexedData
    {
        private const String MODULE = "Houses";
        public int typeId;
        public uint defaultPrice;
        public int nameId;
        public int descriptionId;
        public int gfxId;

        int IIndexedData.Id
        {
            get { return (int)typeId; }
        }

        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public uint DefaultPrice
        {
            get { return defaultPrice; }
            set { defaultPrice = value; }
        }

        public int NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public int DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public int GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

    }
}