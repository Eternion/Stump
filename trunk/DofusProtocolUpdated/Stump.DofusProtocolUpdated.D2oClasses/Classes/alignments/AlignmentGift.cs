

// Generated on 12/12/2013 16:57:36
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
        public int EffectId
        {
            get { return effectId; }
            set { effectId = value; }
        }
        [D2OIgnore]
        public uint GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }
    }
}