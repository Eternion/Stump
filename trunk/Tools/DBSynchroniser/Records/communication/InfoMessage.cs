 


// Generated on 10/13/2013 12:21:14
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
    [TableName("InfoMessages")]
    [D2OClass("InfoMessage", "com.ankamagames.dofus.datacenter.communication")]
    public class InfoMessageRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "InfoMessages";
        public uint typeId;
        public uint messageId;
        public uint textId;

        [D2OIgnore]
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [D2OIgnore]
        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        [D2OIgnore]
        public uint TextId
        {
            get { return textId; }
            set { textId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (InfoMessage)obj;
            
            TypeId = castedObj.typeId;
            MessageId = castedObj.messageId;
            TextId = castedObj.textId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (InfoMessage)parent : new InfoMessage();
            obj.typeId = TypeId;
            obj.messageId = MessageId;
            obj.textId = TextId;
            return obj;
        
        }
    }
}