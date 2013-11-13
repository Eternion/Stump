

// Generated on 10/28/2013 14:03:17
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("ChatChannel", "com.ankamagames.dofus.datacenter.communication")]
    [Serializable]
    public class ChatChannel : IDataObject, IIndexedData
    {
        private const String MODULE = "ChatChannels";
        public uint id;
        [I18NField]
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
        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
        [D2OIgnore]
        public String Shortcut
        {
            get { return shortcut; }
            set { shortcut = value; }
        }
        [D2OIgnore]
        public String ShortcutKey
        {
            get { return shortcutKey; }
            set { shortcutKey = value; }
        }
        [D2OIgnore]
        public Boolean IsPrivate
        {
            get { return isPrivate; }
            set { isPrivate = value; }
        }
        [D2OIgnore]
        public Boolean AllowObjects
        {
            get { return allowObjects; }
            set { allowObjects = value; }
        }
    }
}