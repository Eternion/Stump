
// Generated on 01/04/2013 14:36:08
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("RankNames")]
    [Serializable]
    public class RankName : IDataObject
    {
        private const String MODULE = "RankNames";
        public int id;
        public uint nameId;
        public int order;
    }
}