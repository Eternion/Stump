

// Generated on 02/18/2015 11:05:56
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
        public const String MODULE = "Ornaments";
        public int id;
        [I18NField]
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public Boolean Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }
        [D2OIgnore]
        public int AssetId
        {
            get { return this.assetId; }
            set { this.assetId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return this.iconId; }
            set { this.iconId = value; }
        }
        [D2OIgnore]
        public int Rarity
        {
            get { return this.rarity; }
            set { this.rarity = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}