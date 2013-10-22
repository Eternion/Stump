 


// Generated on 10/19/2013 17:17:45
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("ServerCommunities")]
    [D2OClass("ServerCommunity", "com.ankamagames.dofus.datacenter.servers")]
    public class ServerCommunityRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "ServerCommunities";
        public int id;
        public uint nameId;
        public String shortId;
        public List<String> defaultCountries;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [NullString]
        public String ShortId
        {
            get { return shortId; }
            set { shortId = value; }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
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
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (ServerCommunity)parent : new ServerCommunity();
            obj.id = Id;
            obj.nameId = NameId;
            obj.shortId = ShortId;
            obj.defaultCountries = DefaultCountries;
            return obj;
        
        }
    }
}