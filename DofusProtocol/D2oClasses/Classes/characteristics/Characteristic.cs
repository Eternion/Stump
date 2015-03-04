

// Generated on 02/19/2015 12:10:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Characteristic", "com.ankamagames.dofus.datacenter.characteristics")]
    [Serializable]
    public class Characteristic : IDataObject, IIndexedData
    {
        public const String MODULE = "Characteristics";
        public int id;
        public String keyword;
        [I18NField]
        public uint nameId;
        public String asset;
        public int categoryId;
        public Boolean visible;
        public int order;
        public Boolean upgradable;
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
        public String Keyword
        {
            get { return this.keyword; }
            set { this.keyword = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public String Asset
        {
            get { return this.asset; }
            set { this.asset = value; }
        }
        [D2OIgnore]
        public int CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
        [D2OIgnore]
        public Boolean Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public Boolean Upgradable
        {
            get { return this.upgradable; }
            set { this.upgradable = value; }
        }
    }
}