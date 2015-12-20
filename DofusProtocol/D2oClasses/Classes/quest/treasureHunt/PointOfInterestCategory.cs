

// Generated on 12/20/2015 18:01:22
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("PointOfInterestCategory", "com.ankamagames.dofus.datacenter.quest.treasureHunt")]
    [Serializable]
    public class PointOfInterestCategory : IDataObject, IIndexedData
    {
        public const String MODULE = "PointOfInterestCategory";
        public uint id;
        [I18NField]
        public uint actionLabelId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint ActionLabelId
        {
            get { return this.actionLabelId; }
            set { this.actionLabelId = value; }
        }
    }
}