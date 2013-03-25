
// Generated on 03/25/2013 19:24:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Incarnation")]
    [Serializable]
    public class Incarnation : IDataObject, IIndexedData
    {
        private const String MODULE = "Incarnation";
        public uint id;
        public String lookMale;
        public String lookFemale;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public String LookMale
        {
            get { return lookMale; }
            set { lookMale = value; }
        }

        public String LookFemale
        {
            get { return lookFemale; }
            set { lookFemale = value; }
        }

    }
}