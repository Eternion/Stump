 


// Generated on 09/26/2016 01:50:45
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
    [TableName("SpeakingItemsText")]
    [D2OClass("SpeakingItemText", "com.ankamagames.dofus.datacenter.livingObjects")]
    public class SpeakingItemTextRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "SpeakingItemsText";
        public int textId;
        public double textProba;
        [I18NField]
        public uint textStringId;
        public int textLevel;
        public int textSound;
        public String textRestriction;

        int ID2ORecord.Id
        {
            get { return (int)textId; }
        }


        [D2OIgnore]
        [PrimaryKey("TextId", false)]
        public int TextId
        {
            get { return textId; }
            set { textId = value; }
        }

        [D2OIgnore]
        public double TextProba
        {
            get { return textProba; }
            set { textProba = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint TextStringId
        {
            get { return textStringId; }
            set { textStringId = value; }
        }

        [D2OIgnore]
        public int TextLevel
        {
            get { return textLevel; }
            set { textLevel = value; }
        }

        [D2OIgnore]
        public int TextSound
        {
            get { return textSound; }
            set { textSound = value; }
        }

        [D2OIgnore]
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
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SpeakingItemText)parent : new SpeakingItemText();
            obj.textId = TextId;
            obj.textProba = TextProba;
            obj.textStringId = TextStringId;
            obj.textLevel = TextLevel;
            obj.textSound = TextSound;
            obj.textRestriction = TextRestriction;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}