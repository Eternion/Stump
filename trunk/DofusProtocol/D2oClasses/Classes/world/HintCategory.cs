
// Generated on 03/25/2013 19:24:39
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("HintCategory")]
    [Serializable]
    public class HintCategory : IDataObject, IIndexedData
    {
        private const String MODULE = "HintCategory";
        public int id;
        public uint nameId;

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

    }
}