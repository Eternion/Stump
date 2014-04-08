

// Generated on 12/12/2013 16:57:42
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Server", "com.ankamagames.dofus.datacenter.servers")]
    [Serializable]
    public class Server : IDataObject, IIndexedData
    {
        public const String MODULE = "Servers";
        public int id;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint commentId;
        public double openingDate;
        public String language;
        public int populationId;
        public uint gameTypeId;
        public int communityId;
        public List<String> restrictedToLanguages;
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
        public uint CommentId
        {
            get { return commentId; }
            set { commentId = value; }
        }
        [D2OIgnore]
        public double OpeningDate
        {
            get { return openingDate; }
            set { openingDate = value; }
        }
        [D2OIgnore]
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
        public List<String> RestrictedToLanguages
        {
            get { return restrictedToLanguages; }
            set { restrictedToLanguages = value; }
        }
    }
}