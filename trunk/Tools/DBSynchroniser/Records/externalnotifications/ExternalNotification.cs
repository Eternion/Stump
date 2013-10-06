 


// Generated on 10/06/2013 14:21:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("ExternalNotifications")]
    [D2OClass("ExternalNotification")]
    public class ExternalNotificationRecord : ID2ORecord
    {
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

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public int ColorId
        {
            get { return colorId; }
            set { colorId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public Boolean DefaultEnable
        {
            get { return defaultEnable; }
            set { defaultEnable = value; }
        }

        public Boolean DefaultSound
        {
            get { return defaultSound; }
            set { defaultSound = value; }
        }

        public Boolean DefaultNotify
        {
            get { return defaultNotify; }
            set { defaultNotify = value; }
        }

        public Boolean DefaultMultiAccount
        {
            get { return defaultMultiAccount; }
            set { defaultMultiAccount = value; }
        }

        [NullString]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new ExternalNotification();
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