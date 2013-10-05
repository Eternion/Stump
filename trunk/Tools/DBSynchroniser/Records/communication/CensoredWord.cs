 


// Generated on 10/06/2013 01:10:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("CensoredWords")]
    public class CensoredWordRecord : ID2ORecord
    {
        private const String MODULE = "CensoredWords";
        public uint id;
        public uint listId;
        public String language;
        public String word;
        public Boolean deepLooking;

        [PrimaryKey("Id", false)]
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

        public void AssignFields(object obj)
        {
            var castedObj = (CensoredWord)obj;
            
            Id = castedObj.id;
            ListId = castedObj.listId;
            Language = castedObj.language;
            Word = castedObj.word;
            DeepLooking = castedObj.deepLooking;
        }
        
        public object CreateObject()
        {
            var obj = new CensoredWord();
            
            obj.id = Id;
            obj.listId = ListId;
            obj.language = Language;
            obj.word = Word;
            obj.deepLooking = DeepLooking;
            return obj;
        
        }
    }
}