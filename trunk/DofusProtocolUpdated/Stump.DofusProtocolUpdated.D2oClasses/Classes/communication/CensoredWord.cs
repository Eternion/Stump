

// Generated on 12/12/2013 16:57:37
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint ListId
        {
            get { return listId; }
            set { listId = value; }
        }
        [D2OIgnore]
        public String Language
        {
            get { return language; }
            set { language = value; }
        }
        [D2OIgnore]
        public String Word
        {
            get { return word; }
            set { word = value; }
        }
        [D2OIgnore]
        public Boolean DeepLooking
        {
            get { return deepLooking; }
            set { deepLooking = value; }
        }
    }
}