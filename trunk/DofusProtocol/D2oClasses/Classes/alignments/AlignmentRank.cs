
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentRank")]
    [Serializable]
    public class AlignmentRank : IDataObject, IIndexedData
    {
        private const String MODULE = "AlignmentRank";
        public int id;
        public uint orderId;
        public uint nameId;
        public uint descriptionId;
        public int minimumAlignment;
        public int objectsStolen;
        public List<int> gifts;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public int MinimumAlignment
        {
            get { return minimumAlignment; }
            set { minimumAlignment = value; }
        }

        public int ObjectsStolen
        {
            get { return objectsStolen; }
            set { objectsStolen = value; }
        }

        public List<int> Gifts
        {
            get { return gifts; }
            set { gifts = value; }
        }

    }
}