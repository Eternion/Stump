
// Generated on 03/25/2013 19:24:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Notifications")]
    [Serializable]
    public class Notification : IDataObject, IIndexedData
    {
        private const String MODULE = "Notifications";
        public int id;
        public uint titleId;
        public uint messageId;
        public int iconId;
        public int typeId;
        public String trigger;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

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

        public String Trigger
        {
            get { return trigger; }
            set { trigger = value; }
        }

    }
}