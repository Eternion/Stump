

// Generated on 10/06/2013 17:58:56
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ServerCommunity", "com.ankamagames.dofus.datacenter.servers")]
    [Serializable]
    public class ServerCommunity : IDataObject, IIndexedData
    {
        private const String MODULE = "ServerCommunities";
        public int id;
        public uint nameId;
        public String shortId;
        public List<String> defaultCountries;
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
        public String ShortId
        {
            get { return shortId; }
            set { shortId = value; }
        }
        [D2OIgnore]
        public List<String> DefaultCountries
        {
            get { return defaultCountries; }
            set { defaultCountries = value; }
        }
    }
}