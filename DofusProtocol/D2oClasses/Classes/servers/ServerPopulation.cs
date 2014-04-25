

// Generated on 10/28/2013 14:03:21
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ServerPopulation", "com.ankamagames.dofus.datacenter.servers")]
    [Serializable]
    public class ServerPopulation : IDataObject, IIndexedData
    {
        private const String MODULE = "ServerPopulations";
        public int id;
        [I18NField]
        public uint nameId;
        public int weight;
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
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }
    }
}