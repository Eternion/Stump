 


// Generated on 09/26/2016 01:50:40
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("ChatChannels")]
    [D2OClass("ChatChannel", "com.ankamagames.dofus.datacenter.communication")]
    public class ChatChannelRecord : ID2ORecord, ISaveIntercepter
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

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
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
        [NullString]
        public String Shortcut
        {
            get { return shortcut; }
            set { shortcut = value; }
        }

        [D2OIgnore]
        [NullString]
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
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (ChatChannel)parent : new ChatChannel();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.shortcut = Shortcut;
            obj.shortcutKey = ShortcutKey;
            obj.isPrivate = IsPrivate;
            obj.allowObjects = AllowObjects;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}