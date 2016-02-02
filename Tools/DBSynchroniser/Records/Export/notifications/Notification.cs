 


// Generated on 02/02/2016 14:15:17
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
    [TableName("Notifications")]
    [D2OClass("Notification", "com.ankamagames.dofus.datacenter.notifications")]
    public class NotificationRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Notifications";
        public int id;
        [I18NField]
        public uint titleId;
        [I18NField]
        public uint messageId;
        public int iconId;
        public int typeId;
        public String trigger;

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
        public uint TitleId
        {
            get { return titleId; }
            set { titleId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        [D2OIgnore]
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [D2OIgnore]
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
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Notification)parent : new Notification();
            obj.id = Id;
            obj.titleId = TitleId;
            obj.messageId = MessageId;
            obj.iconId = IconId;
            obj.typeId = TypeId;
            obj.trigger = Trigger;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}