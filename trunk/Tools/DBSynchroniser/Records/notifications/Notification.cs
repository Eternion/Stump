 


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
    [TableName("Notifications")]
    [D2OClass("Notification")]
    public class NotificationRecord : ID2ORecord
    {
        private const String MODULE = "Notifications";
        public int id;
        public uint titleId;
        public uint messageId;
        public int iconId;
        public int typeId;
        public String trigger;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint TitleId
        {
            get { return titleId; }
            set { titleId = value; }
        }

        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [NullString]
        public String Trigger
        {
            get { return trigger; }
            set { trigger = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Notification)obj;
            
            Id = castedObj.id;
            TitleId = castedObj.titleId;
            MessageId = castedObj.messageId;
            IconId = castedObj.iconId;
            TypeId = castedObj.typeId;
            Trigger = castedObj.trigger;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new Notification();
            obj.id = Id;
            obj.titleId = TitleId;
            obj.messageId = MessageId;
            obj.iconId = IconId;
            obj.typeId = TypeId;
            obj.trigger = Trigger;
            return obj;
        
        }
    }
}