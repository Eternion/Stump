

// Generated on 04/19/2016 10:17:57
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
        public const String MODULE = "ExternalNotifications";
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public int CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return this.iconId; }
            set { this.iconId = value; }
        }
        [D2OIgnore]
        public int ColorId
        {
            get { return this.colorId; }
            set { this.colorId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public Boolean DefaultEnable
        {
            get { return this.defaultEnable; }
            set { this.defaultEnable = value; }
        }
        [D2OIgnore]
        public Boolean DefaultSound
        {
            get { return this.defaultSound; }
            set { this.defaultSound = value; }
        }
        [D2OIgnore]
        public Boolean DefaultNotify
        {
            get { return this.defaultNotify; }
            set { this.defaultNotify = value; }
        }
        [D2OIgnore]
        public Boolean DefaultMultiAccount
        {
            get { return this.defaultMultiAccount; }
            set { this.defaultMultiAccount = value; }
        }
        [D2OIgnore]
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        [D2OIgnore]
        public uint MessageId
        {
            get { return this.messageId; }
            set { this.messageId = value; }
        }
    }
}