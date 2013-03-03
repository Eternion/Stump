
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ExternalNotifications")]
    [Serializable]
    public class ExternalNotification : IDataObject, IIndexedData
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

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

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

    }
}