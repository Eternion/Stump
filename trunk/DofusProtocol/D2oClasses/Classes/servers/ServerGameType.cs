
// Generated on 01/04/2013 14:36:10
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ServerGameTypes")]
    [Serializable]
    public class ServerGameType : IDataObject
    {
        private const String MODULE = "ServerGameTypes";
        public int id;
        public uint nameId;
    }
}