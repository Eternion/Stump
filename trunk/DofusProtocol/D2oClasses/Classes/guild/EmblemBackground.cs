

// Generated on 10/06/2013 17:58:53
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EmblemBackground", "com.ankamagames.dofus.datacenter.guild")]
    [Serializable]
    public class EmblemBackground : IDataObject, IIndexedData
    {
        private const String MODULE = "EmblemBackgrounds";
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