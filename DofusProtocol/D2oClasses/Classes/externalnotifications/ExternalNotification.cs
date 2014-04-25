

// Generated on 10/28/2013 14:03:17
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ExternalNotification", "com.ankamagames.dofus.datacenter.externalnotifications")]
    [Serializable]
    public class ExternalNotification : IDataObject, IIndexedData
    {
        private const String MODULE = "ExternalNotifications";
        public int id;
        public int categoryId;
        public int iconId;
        public int colorId;
        [I18NField]
        public uint descriptionId;
        public Boolean defaultEnable;
        public Boolean defaultSound;
        public Boolean defaultNotify;
        public Boolean defaultMultiAccount;
        public String name;
        [I18NField]
        public uint messageId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
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
    }
}