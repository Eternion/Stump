
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentGift")]
    [Serializable]
    public class AlignmentGift : IDataObject, IIndexedData
    {
        private const String MODULE = "AlignmentGift";
        public int id;
        public uint nameId;
        public int effectId;
        public uint gfxId;

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

        public int EffectId
        {
            get { return effectId; }
            set { effectId = value; }
        }

        public uint GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

    }
}