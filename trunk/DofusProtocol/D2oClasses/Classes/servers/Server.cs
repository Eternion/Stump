
// Generated on 03/02/2013 21:17:47
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Servers")]
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

        public uint CommentId
        {
            get { return commentId; }
            set { commentId = value; }
        }

        public float OpeningDate
        {
            get { return openingDate; }
            set { openingDate = value; }
        }

        public String Language
        {
            get { return language; }
            set { language = value; }
        }

        public int PopulationId
        {
            get { return populationId; }
            set { populationId = value; }
        }

        public uint GameTypeId
        {
            get { return gameTypeId; }
            set { gameTypeId = value; }
        }

        public int CommunityId
        {
            get { return communityId; }
            set { communityId = value; }
        }

        public List<String> RestrictedToLanguages
        {
            get { return restrictedToLanguages; }
            set { restrictedToLanguages = value; }
        }

    }
}