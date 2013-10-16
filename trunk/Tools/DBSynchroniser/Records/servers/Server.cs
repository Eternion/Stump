 


// Generated on 10/13/2013 12:21:17
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
    [TableName("Servers")]
    [D2OClass("Server", "com.ankamagames.dofus.datacenter.servers")]
    public class ServerRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "Servers";
        public int id;
        public uint nameId;
        public uint commentId;
        public float openingDate;
        public String language;
        public int populationId;
        public uint gameTypeId;
        public int communityId;
        public List<String> restrictedToLanguages;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
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
        public uint CommentId
        {
            get { return commentId; }
            set { commentId = value; }
        }

        [D2OIgnore]
        public float OpeningDate
        {
            get { return openingDate; }
            set { openingDate = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Language
        {
            get { return language; }
            set { language = value; }
        }

        [D2OIgnore]
        public int PopulationId
        {
            get { return populationId; }
            set { populationId = value; }
        }

        [D2OIgnore]
        public uint GameTypeId
        {
            get { return gameTypeId; }
            set { gameTypeId = value; }
        }

        [D2OIgnore]
        public int CommunityId
        {
            get { return communityId; }
            set { communityId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<String> RestrictedToLanguages
        {
            get { return restrictedToLanguages; }
            set
            {
                restrictedToLanguages = value;
                m_restrictedToLanguagesBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_restrictedToLanguagesBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] RestrictedToLanguagesBin
        {
            get { return m_restrictedToLanguagesBin; }
            set
            {
                m_restrictedToLanguagesBin = value;
                restrictedToLanguages = value == null ? null : value.ToObject<List<String>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Server)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            CommentId = castedObj.commentId;
            OpeningDate = castedObj.openingDate;
            Language = castedObj.language;
            PopulationId = castedObj.populationId;
            GameTypeId = castedObj.gameTypeId;
            CommunityId = castedObj.communityId;
            RestrictedToLanguages = castedObj.restrictedToLanguages;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Server)parent : new Server();
            obj.id = Id;
            obj.nameId = NameId;
            obj.commentId = CommentId;
            obj.openingDate = OpeningDate;
            obj.language = Language;
            obj.populationId = PopulationId;
            obj.gameTypeId = GameTypeId;
            obj.communityId = CommunityId;
            obj.restrictedToLanguages = RestrictedToLanguages;
            return obj;
        
        }
    }
}