 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("NpcMessages")]
    [D2OClass("NpcMessage")]
    public class NpcMessageRecord : ID2ORecord
    {
        private const String MODULE = "NpcMessages";
        public int id;
        public uint messageId;
        public String messageParams;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        [NullString]
        public String MessageParams
        {
            get { return messageParams; }
            set { messageParams = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (NpcMessage)obj;
            
            Id = castedObj.id;
            MessageId = castedObj.messageId;
            MessageParams = castedObj.messageParams;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new NpcMessage();
            obj.id = Id;
            obj.messageId = MessageId;
            obj.messageParams = MessageParams;
            return obj;
        
        }
    }
}