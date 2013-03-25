
// Generated on 03/25/2013 19:24:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("RankNames")]
    [Serializable]
    public class RankName : IDataObject, IIndexedData
    {
        private const String MODULE = "RankNames";
        public int id;
        public uint nameId;
        public int order;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

    }
}