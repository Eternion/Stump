
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ChatChannels")]
    [Serializable]
    public class ChatChannel : IDataObject, IIndexedData
    {
        private const String MODULE = "ChatChannels";
        public uint id;
        public uint nameId;
        public uint descriptionId;
        public String shortcut;
        public String shortcutKey;
        public Boolean isPrivate;
        public Boolean allowObjects;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public String Shortcut
        {
            get { return shortcut; }
            set { shortcut = value; }
        }

        public String ShortcutKey
        {
            get { return shortcutKey; }
            set { shortcutKey = value; }
        }

        public Boolean IsPrivate
        {
            get { return isPrivate; }
            set { isPrivate = value; }
        }

        public Boolean AllowObjects
        {
            get { return allowObjects; }
            set { allowObjects = value; }
        }

    }
}