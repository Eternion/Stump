

// Generated on 12/20/2015 18:01:10
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SmileyCategory", "com.ankamagames.dofus.datacenter.communication")]
    [Serializable]
    public class SmileyCategory : IDataObject, IIndexedData
    {
        public const String MODULE = "SmileyCategories";
        public int id;
        public uint order;
        public String gfxId;
        public Boolean isFake;
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
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public String GfxId
        {
            get { return this.gfxId; }
            set { this.gfxId = value; }
        }
        [D2OIgnore]
        public Boolean IsFake
        {
            get { return this.isFake; }
            set { this.isFake = value; }
        }
    }
}