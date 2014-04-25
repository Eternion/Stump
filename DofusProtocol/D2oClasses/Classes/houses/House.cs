

// Generated on 10/28/2013 14:03:17
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("House", "com.ankamagames.dofus.datacenter.houses")]
    [Serializable]
    public class House : IDataObject, IIndexedData
    {
        private const String MODULE = "Houses";
        public int typeId;
        public uint defaultPrice;
        [I18NField]
        public int nameId;
        [I18NField]
        public int descriptionId;
        public int gfxId;
        int IIndexedData.Id
        {
            get { return (int)nameId; }
        }
        [D2OIgnore]
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }
        [D2OIgnore]
        public uint DefaultPrice
        {
            get { return defaultPrice; }
            set { defaultPrice = value; }
        }
        [D2OIgnore]
        public int NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public int DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
        [D2OIgnore]
        public int GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }
    }
}