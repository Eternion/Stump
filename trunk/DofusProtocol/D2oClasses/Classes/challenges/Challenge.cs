
// Generated on 01/04/2013 14:36:07
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Challenge")]
    [Serializable]
    public class Challenge : IDataObject
    {
        private const String MODULE = "Challenge";
        public int id;
        public uint nameId;
        public uint descriptionId;
    }
}