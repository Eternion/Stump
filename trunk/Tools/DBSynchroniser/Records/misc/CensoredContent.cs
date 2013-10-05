 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("CensoredContents")]
    public class CensoredContentRecord : ID2ORecord
    {
        public const String MODULE = "CensoredContents";
        public int type;
        public int oldValue;
        public int newValue;
        public String lang;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public int OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }

        public int NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }

        public String Lang
        {
            get { return lang; }
            set { lang = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (CensoredContent)obj;
            
            Type = castedObj.type;
            OldValue = castedObj.oldValue;
            NewValue = castedObj.newValue;
            Lang = castedObj.lang;
        }
        
        public object CreateObject()
        {
            var obj = new CensoredContent();
            
            obj.type = Type;
            obj.oldValue = OldValue;
            obj.newValue = NewValue;
            obj.lang = Lang;
            return obj;
        
        }
    }
}