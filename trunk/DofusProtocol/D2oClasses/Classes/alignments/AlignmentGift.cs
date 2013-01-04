
// Generated on 01/04/2013 14:36:07
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentGift")]
    [Serializable]
    public class AlignmentGift : IDataObject
    {
        private const String MODULE = "AlignmentGift";
        public int id;
        public uint nameId;
        public int effectId;
        public uint gfxId;
    }
}