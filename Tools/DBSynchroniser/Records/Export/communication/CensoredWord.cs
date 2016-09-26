 


// Generated on 09/26/2016 01:50:40
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
    [TableName("CensoredWords")]
    [D2OClass("CensoredWord", "com.ankamagames.dofus.datacenter.communication")]
    public class CensoredWordRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "CensoredWords";
        public uint id;
        public uint listId;
        public String language;
        public String word;
        public Boolean deepLooking;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
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
        [NullString]
        public String Language
        {
            get { return language; }
            set { language = value; }
        }

        [D2OIgnore]
        [NullString]
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (CensoredWord)obj;
            
            Id = castedObj.id;
            ListId = castedObj.listId;
            Language = castedObj.language;
            Word = castedObj.word;
            DeepLooking = castedObj.deepLooking;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (CensoredWord)parent : new CensoredWord();
            obj.id = Id;
            obj.listId = ListId;
            obj.language = Language;
            obj.word = Word;
            obj.deepLooking = DeepLooking;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}