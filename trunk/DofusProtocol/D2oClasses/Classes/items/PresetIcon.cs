

// Generated on 10/28/2013 14:03:18
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("PresetIcon", "com.ankamagames.dofus.datacenter.items")]
    [Serializable]
    public class PresetIcon : IDataObject, IIndexedData
    {
        private const String MODULE = "PresetIcons";
        public int id;
        public int order;
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
        public int Order
        {
            get { return order; }
            set { order = value; }
        }
    }
}