
// Generated on 03/02/2013 21:17:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Appearances")]
    [Serializable]
    public class Appearance : IDataObject, IIndexedData
    {
        public const String MODULE = "Appearances";
        public uint id;
        public uint type;
        public String data;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint Type
        {
            get { return type; }
            set { type = value; }
        }

        public String Data
        {
            get { return data; }
            set { data = value; }
        }

    }
}