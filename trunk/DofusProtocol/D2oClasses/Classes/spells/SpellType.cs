
// Generated on 03/25/2013 19:24:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpellTypes")]
    [Serializable]
    public class SpellType : IDataObject, IIndexedData
    {
        private const String MODULE = "SpellTypes";
        public int id;
        public uint longNameId;
        public uint shortNameId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint LongNameId
        {
            get { return longNameId; }
            set { longNameId = value; }
        }

        public uint ShortNameId
        {
            get { return shortNameId; }
            set { shortNameId = value; }
        }

    }
}