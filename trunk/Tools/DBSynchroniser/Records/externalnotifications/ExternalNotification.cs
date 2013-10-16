 


// Generated on 10/13/2013 12:21:15
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
    [TableName("ExternalNotifications")]
    [D2OClass("ExternalNotification", "com.ankamagames.dofus.datacenter.externalnotifications")]
    public class ExternalNotificationRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "ExternalNotifications";
        public int id;
        public int categoryId;
        public int iconId;
        public int colorId;
        public uint descriptionId;
        public Boolean defaultEnable;
        public Boolean defaultSound;
        public Boolean defaultNotify;
        public Boolean defaultMultiAccount;
        public String name;
        public uint messageId;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        [D2OIgnore]
        public int ColorId
        {
            get { return colorId; }
            set { colorId = value; }
        }

        [D2OIgnore]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public Boolean DefaultEnable
        {
            get { return defaultEnable; }
            set { defaultEnable = value; }
        }

        [D2OIgnore]
        public Boolean DefaultSound
        {
            get { return defaultSound; }
            set { defaultSound = value; }
        }

        [D2OIgnore]
        public Boolean DefaultNotify
        {
            get { return defaultNotify; }
            set { defaultNotify = value; }
        }

        [D2OIgnore]
        public Boolean DefaultMultiAccount
        {
            get { return defaultMultiAccount; }
            set { defaultMultiAccount = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        [D2OIgnore]
        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (ExternalNotification)obj;
            
            Id = castedObj.id;
            CategoryId = castedObj.categoryId;
            IconId = castedObj.iconId;
            ColorId = castedObj.colorId;
            DescriptionId = castedObj.descriptionId;
            DefaultEnable = castedObj.defaultEnable;
            DefaultSound = castedObj.defaultSound;
            DefaultNotify = castedObj.defaultNotify;
            DefaultMultiAccount = castedObj.defaultMultiAccount;
            Name = castedObj.name;
            MessageId = castedObj.messageId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (ExternalNotification)parent : new ExternalNotification();
            obj.id = Id;
            obj.categoryId = CategoryId;
            obj.iconId = IconId;
            obj.colorId = ColorId;
            obj.descriptionId = DescriptionId;
            obj.defaultEnable = DefaultEnable;
            obj.defaultSound = DefaultSound;
            obj.defaultNotify = DefaultNotify;
            obj.defaultMultiAccount = DefaultMultiAccount;
            obj.name = Name;
            obj.messageId = MessageId;
            return obj;
        
        }
    }
}