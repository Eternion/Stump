 


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
    [TableName("ChatChannels")]
    [D2OClass("ChatChannel")]
    public class ChatChannelRecord : ID2ORecord
    {
        private const String MODULE = "ChatChannels";
        public uint id;
        public uint nameId;
        public uint descriptionId;
        public String shortcut;
        public String shortcutKey;
        public Boolean isPrivate;
        public Boolean allowObjects;

        [PrimaryKey("Id", false)]
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

        [NullString]
        public String Shortcut
        {
            get { return shortcut; }
            set { shortcut = value; }
        }

        [NullString]
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (ChatChannel)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
            Shortcut = castedObj.shortcut;
            ShortcutKey = castedObj.shortcutKey;
            IsPrivate = castedObj.isPrivate;
            AllowObjects = castedObj.allowObjects;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new ChatChannel();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.shortcut = Shortcut;
            obj.shortcutKey = ShortcutKey;
            obj.isPrivate = IsPrivate;
            obj.allowObjects = AllowObjects;
            return obj;
        
        }
    }
}