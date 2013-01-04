
// Generated on 01/04/2013 14:36:11
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MapReferences")]
    [Serializable]
    public class MapReference : IDataObject
    {
        private const String MODULE = "MapReferences";
        public int id;
        public uint mapId;
        public int cellId;
    }
}