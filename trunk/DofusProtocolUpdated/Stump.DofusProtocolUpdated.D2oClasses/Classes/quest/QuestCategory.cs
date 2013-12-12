

// Generated on 12/12/2013 16:57:42
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }
        [D2OIgnore]
        public List<uint> QuestIds
        {
            get { return questIds; }
            set { questIds = value; }
        }
    }
}