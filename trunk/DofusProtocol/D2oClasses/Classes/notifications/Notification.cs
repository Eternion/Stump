

// Generated on 10/28/2013 14:03:20
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
        private const String MODULE = "Notifications";
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint TitleId
        {
            get { return titleId; }
            set { titleId = value; }
        }
        [D2OIgnore]
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
        public String Trigger
        {
            get { return trigger; }
            set { trigger = value; }
        }
    }
}