 


// Generated on 10/06/2013 14:22:00
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SpeakingItemsText")]
    [D2OClass("SpeakingItemText")]
    public class SpeakingItemTextRecord : ID2ORecord
    {
        private const String MODULE = "SpeakingItemsText";
        public int textId;
        public float textProba;
        public uint textStringId;
        public int textLevel;
        public int textSound;
        public String textRestriction;

        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        public int TextId
        {
            get { return textId; }
            set { textId = value; }
        }

        public float TextProba
        {
            get { return textProba; }
            set { textProba = value; }
        }

        public uint TextStringId
        {
            get { return textStringId; }
            set { textStringId = value; }
        }

        public int TextLevel
        {
            get { return textLevel; }
            set { textLevel = value; }
        }

        public int TextSound
        {
            get { return textSound; }
            set { textSound = value; }
        }

        [NullString]
        public String TextRestriction
        {
            get { return textRestriction; }
            set { textRestriction = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SpeakingItemText)obj;
            
            TextId = castedObj.textId;
            TextProba = castedObj.textProba;
            TextStringId = castedObj.textStringId;
            TextLevel = castedObj.textLevel;
            TextSound = castedObj.textSound;
            TextRestriction = castedObj.textRestriction;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new SpeakingItemText();
            obj.textId = TextId;
            obj.textProba = TextProba;
            obj.textStringId = TextStringId;
            obj.textLevel = TextLevel;
            obj.textSound = TextSound;
            obj.textRestriction = TextRestriction;
            return obj;
        
        }
    }
}