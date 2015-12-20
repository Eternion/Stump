

// Generated on 12/20/2015 18:01:22
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
        public const String MODULE = "ServerCommunities";
        public int id;
        [I18NField]
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
        public String ShortId
        {
            get { return this.shortId; }
            set { this.shortId = value; }
        }
        [D2OIgnore]
        public List<String> DefaultCountries
        {
            get { return this.defaultCountries; }
            set { this.defaultCountries = value; }
        }
    }
}