

// Generated on 12/29/2014 16:35:05
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentGift", "com.ankamagames.dofus.datacenter.alignments")]
    [Serializable]
    public class AlignmentGift : IDataObject, IIndexedData
    {
        public const String MODULE = "AlignmentGift";
        public int id;
        [I18NField]
        public uint nameId;
        public int effectId;
        public uint gfxId;
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
        public int EffectId
        {
            get { return this.effectId; }
            set { this.effectId = value; }
        }
        [D2OIgnore]
        public uint GfxId
        {
            get { return this.gfxId; }
            set { this.gfxId = value; }
        }
    }
}