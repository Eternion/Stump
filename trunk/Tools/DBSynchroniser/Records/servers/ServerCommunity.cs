 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("ServerCommunities")]
    [D2OClass("ServerCommunity")]
    public class ServerCommunityRecord : ID2ORecord
    {
        private const String MODULE = "ServerCommunities";
        public int id;
        public uint nameId;
        public String shortId;
        public List<String> defaultCountries;

        [PrimaryKey("Id", false)]
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

        [NullString]
        public String ShortId
        {
            get { return shortId; }
            set { shortId = value; }
        }

        [Ignore]
        public List<String> DefaultCountries
        {
            get { return defaultCountries; }
            set
            {
                defaultCountries = value;
                m_defaultCountriesBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_defaultCountriesBin;
        public byte[] DefaultCountriesBin
        {
            get { return m_defaultCountriesBin; }
            set
            {
                m_defaultCountriesBin = value;
                defaultCountries = value == null ? null : value.ToObject<List<String>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (ServerCommunity)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            ShortId = castedObj.shortId;
            DefaultCountries = castedObj.defaultCountries;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new ServerCommunity();
            obj.id = Id;
            obj.nameId = NameId;
            obj.shortId = ShortId;
            obj.defaultCountries = DefaultCountries;
            return obj;
        
        }
    }
}