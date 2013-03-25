
// Generated on 03/25/2013 19:24:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("TaxCollectorFirstnames")]
    [Serializable]
    public class TaxCollectorFirstname : IDataObject, IIndexedData
    {
        private const String MODULE = "TaxCollectorFirstnames";
        public int id;
        public uint firstnameId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint FirstnameId
        {
            get { return firstnameId; }
            set { firstnameId = value; }
        }

    }
}