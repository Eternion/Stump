 


// Generated on 10/19/2013 17:17:44
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
    [TableName("CensoredContents")]
    [D2OClass("CensoredContent", "com.ankamagames.dofus.datacenter.misc")]
    public class CensoredContentRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        public const String MODULE = "CensoredContents";
        public int type;
        public int oldValue;
        public int newValue;
        public String lang;

        [D2OIgnore]
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        [D2OIgnore]
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        [D2OIgnore]
        public int OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }

        [D2OIgnore]
        public int NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Lang
        {
            get { return lang; }
            set { lang = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (CensoredContent)obj;
            
            Type = castedObj.type;
            OldValue = castedObj.oldValue;
            NewValue = castedObj.newValue;
            Lang = castedObj.lang;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (CensoredContent)parent : new CensoredContent();
            obj.type = Type;
            obj.oldValue = OldValue;
            obj.newValue = NewValue;
            obj.lang = Lang;
            return obj;
        
        }
    }
}