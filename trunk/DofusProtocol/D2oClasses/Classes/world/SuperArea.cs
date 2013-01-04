
// Generated on 01/04/2013 14:36:11
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SuperAreas")]
    [Serializable]
    public class SuperArea : IDataObject
    {
        private const String MODULE = "SuperAreas";
        public int id;
        public uint nameId;
        public uint worldmapId;
    }
}