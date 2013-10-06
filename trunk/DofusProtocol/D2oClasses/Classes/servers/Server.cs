

// Generated on 10/06/2013 17:58:56
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
        public float OpeningDate
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