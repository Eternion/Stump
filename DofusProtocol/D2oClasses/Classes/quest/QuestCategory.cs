

// Generated on 10/26/2014 23:27:53
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestCategory", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class QuestCategory : IDataObject, IIndexedData
    {
        public const String MODULE = "QuestCategory";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint order;
        public List<uint> questIds;
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public List<uint> QuestIds
        {
            get { return this.questIds; }
            set { this.questIds = value; }
        }
    }
}