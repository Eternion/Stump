

// Generated on 02/19/2015 12:10:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("CensoredWord", "com.ankamagames.dofus.datacenter.communication")]
    [Serializable]
    public class CensoredWord : IDataObject, IIndexedData
    {
        public const String MODULE = "CensoredWords";
        public uint id;
        public uint listId;
        public String language;
        public String word;
        public Boolean deepLooking;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint ListId
        {
            get { return this.listId; }
            set { this.listId = value; }
        }
        [D2OIgnore]
        public String Language
        {
            get { return this.language; }
            set { this.language = value; }
        }
        [D2OIgnore]
        public String Word
        {
            get { return this.word; }
            set { this.word = value; }
        }
        [D2OIgnore]
        public Boolean DeepLooking
        {
            get { return this.deepLooking; }
            set { this.deepLooking = value; }
        }
    }
}