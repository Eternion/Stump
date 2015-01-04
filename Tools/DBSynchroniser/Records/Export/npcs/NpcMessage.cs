 


// Generated on 01/04/2015 01:23:48
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
    [TableName("NpcMessages")]
    [D2OClass("NpcMessage", "com.ankamagames.dofus.datacenter.npcs")]
    public class NpcMessageRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "NpcMessages";
        public int id;
        [I18NField]
        public uint messageId;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (NpcMessage)obj;
            
            Id = castedObj.id;
            MessageId = castedObj.messageId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (NpcMessage)parent : new NpcMessage();
            obj.id = Id;
            obj.messageId = MessageId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}