

// Generated on 02/02/2016 14:15:00
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
        public const String MODULE = "ChatChannels";
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public String Shortcut
        {
            get { return this.shortcut; }
            set { this.shortcut = value; }
        }
        [D2OIgnore]
        public String ShortcutKey
        {
            get { return this.shortcutKey; }
            set { this.shortcutKey = value; }
        }
        [D2OIgnore]
        public Boolean IsPrivate
        {
            get { return this.isPrivate; }
            set { this.isPrivate = value; }
        }
        [D2OIgnore]
        public Boolean AllowObjects
        {
            get { return this.allowObjects; }
            set { this.allowObjects = value; }
        }
    }
}