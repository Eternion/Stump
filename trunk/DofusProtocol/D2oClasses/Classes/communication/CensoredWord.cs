
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("CensoredWords")]
    [Serializable]
    public class CensoredWord : IDataObject, IIndexedData
    {
        private const String MODULE = "CensoredWords";
        public uint id;
        public uint listId;
        public String language;
        public String word;
        public Boolean deepLooking;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint ListId
        {
            get { return listId; }
            set { listId = value; }
        }

        public String Language
        {
            get { return language; }
            set { language = value; }
        }

        public String Word
        {
            get { return word; }
            set { word = value; }
        }

        public Boolean DeepLooking
        {
            get { return deepLooking; }
            set { deepLooking = value; }
        }

    }
}