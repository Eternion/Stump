
// Generated on 03/02/2013 21:17:47
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ServerCommunities")]
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

        public String ShortId
        {
            get { return shortId; }
            set { shortId = value; }
        }

        public List<String> DefaultCountries
        {
            get { return defaultCountries; }
            set { defaultCountries = value; }
        }

    }
}