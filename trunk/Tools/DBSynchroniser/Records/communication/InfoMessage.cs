 


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
    [D2OClass("InfoMessages")]
    public class InfoMessageRecord : ID2ORecord
    {
        private const String MODULE = "InfoMessages";
        public uint typeId;
        public uint messageId;
        public uint textId;

        [PrimaryKey("TypeId", false)]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        public uint TextId
        {
            get { return textId; }
            set { textId = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (InfoMessage)obj;
            
            TypeId = castedObj.typeId;
            MessageId = castedObj.messageId;
            TextId = castedObj.textId;
        }
        
        public object CreateObject()
        {
            var obj = new InfoMessage();
            
            obj.typeId = TypeId;
            obj.messageId = MessageId;
            obj.textId = TextId;
            return obj;
        
        }
    }
}