
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestCategory")]
    [Serializable]
    public class QuestCategory : IDataObject, IIndexedData
    {
        private const String MODULE = "QuestCategory";
        public uint id;
        public uint nameId;
        public uint order;
        public List<uint> questIds;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        public List<uint> QuestIds
        {
            get { return questIds; }
            set { questIds = value; }
        }

    }
}