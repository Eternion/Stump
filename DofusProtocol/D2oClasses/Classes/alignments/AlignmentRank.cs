

// Generated on 09/26/2016 01:50:25
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
        public const String MODULE = "AlignmentRank";
        public int id;
        public uint orderId;
        [I18NField]
        public uint nameId;
        [I18NField]
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint OrderId
        {
            get { return this.orderId; }
            set { this.orderId = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public int MinimumAlignment
        {
            get { return this.minimumAlignment; }
            set { this.minimumAlignment = value; }
        }
        [D2OIgnore]
        public int ObjectsStolen
        {
            get { return this.objectsStolen; }
            set { this.objectsStolen = value; }
        }
        [D2OIgnore]
        public List<int> Gifts
        {
            get { return this.gifts; }
            set { this.gifts = value; }
        }
    }
}