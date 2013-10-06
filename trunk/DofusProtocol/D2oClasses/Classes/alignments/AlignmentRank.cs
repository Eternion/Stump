

// Generated on 10/06/2013 17:58:52
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlignmentRank", "com.ankamagames.dofus.datacenter.alignments")]
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
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
        [D2OIgnore]
        public int MinimumAlignment
        {
            get { return minimumAlignment; }
            set { minimumAlignment = value; }
        }
        [D2OIgnore]
        public int ObjectsStolen
        {
            get { return objectsStolen; }
            set { objectsStolen = value; }
        }
        [D2OIgnore]
        public List<int> Gifts
        {
            get { return gifts; }
            set { gifts = value; }
        }
    }
}