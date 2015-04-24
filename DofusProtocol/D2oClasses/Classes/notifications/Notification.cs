

// Generated on 04/24/2015 03:38:28
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Notification", "com.ankamagames.dofus.datacenter.notifications")]
    [Serializable]
    public class Notification : IDataObject, IIndexedData
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
        public uint TitleId
        {
            get { return this.titleId; }
            set { this.titleId = value; }
        }
        [D2OIgnore]
        public uint MessageId
        {
            get { return this.messageId; }
            set { this.messageId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return this.iconId; }
            set { this.iconId = value; }
        }
        [D2OIgnore]
        public int TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }
        [D2OIgnore]
        public String Trigger
        {
            get { return this.trigger; }
            set { this.trigger = value; }
        }
    }
}