

// Generated on 04/24/2015 03:38:29
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
        public const String MODULE = "ServerPopulations";
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
        public int Weight
        {
            get { return this.weight; }
            set { this.weight = value; }
        }
    }
}