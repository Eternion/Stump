
// Generated on 03/25/2013 19:24:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ServerPopulations")]
    [Serializable]
    public class ServerPopulation : IDataObject, IIndexedData
    {
        private const String MODULE = "ServerPopulations";
        public int id;
        public uint nameId;
        public int weight;

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

        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

    }
}