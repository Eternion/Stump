 


// Generated on 11/16/2015 14:26:41
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
    public class CensoredContentRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "CensoredContents";
        public String lang;
        public int type;
        public int oldValue;
        public int newValue;

        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }

        [D2OIgnore]
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OIgnore]
        [NullString]
        public String Lang
        {
            get { return lang; }
            set { lang = value; }
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (CensoredContent)obj;
            
            Lang = castedObj.lang;
            Type = castedObj.type;
            OldValue = castedObj.oldValue;
            NewValue = castedObj.newValue;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (CensoredContent)parent : new CensoredContent();
            obj.lang = Lang;
            obj.type = Type;
            obj.oldValue = OldValue;
            obj.newValue = NewValue;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}