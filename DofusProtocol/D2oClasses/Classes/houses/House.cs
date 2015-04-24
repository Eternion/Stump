

// Generated on 04/24/2015 03:38:25
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
        public const String MODULE = "Houses";
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
            get { return this.typeId; }
            set { this.typeId = value; }
        }
        [D2OIgnore]
        public uint DefaultPrice
        {
            get { return this.defaultPrice; }
            set { this.defaultPrice = value; }
        }
        [D2OIgnore]
        public int NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public int DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public int GfxId
        {
            get { return this.gfxId; }
            set { this.gfxId = value; }
        }
    }
}