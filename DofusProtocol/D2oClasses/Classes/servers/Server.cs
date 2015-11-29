

// Generated on 11/16/2015 14:26:35
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
        public uint CommentId
        {
            get { return this.commentId; }
            set { this.commentId = value; }
        }
        [D2OIgnore]
        public double OpeningDate
        {
            get { return this.openingDate; }
            set { this.openingDate = value; }
        }
        [D2OIgnore]
        public String Language
        {
            get { return this.language; }
            set { this.language = value; }
        }
        [D2OIgnore]
        public int PopulationId
        {
            get { return this.populationId; }
            set { this.populationId = value; }
        }
        [D2OIgnore]
        public uint GameTypeId
        {
            get { return this.gameTypeId; }
            set { this.gameTypeId = value; }
        }
        [D2OIgnore]
        public int CommunityId
        {
            get { return this.communityId; }
            set { this.communityId = value; }
        }
        [D2OIgnore]
        public List<String> RestrictedToLanguages
        {
            get { return this.restrictedToLanguages; }
            set { this.restrictedToLanguages = value; }
        }
    }
}