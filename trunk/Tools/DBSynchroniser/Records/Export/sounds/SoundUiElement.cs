 


// Generated on 10/28/2013 14:03:26
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
    [TableName("SoundUiElement")]
    [D2OClass("SoundUiElement", "com.ankamagames.dofus.datacenter.sounds")]
    public class SoundUiElementRecord : ID2ORecord
    {
        public uint id;
        public String name;
        public uint hookId;
        public String file;
        public uint volume;
        public String MODULE = "SoundUiElement";

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
        [NullString]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        [D2OIgnore]
        public uint HookId
        {
            get { return hookId; }
            set { hookId = value; }
        }

        [D2OIgnore]
        [NullString]
        public String File
        {
            get { return file; }
            set { file = value; }
        }

        [D2OIgnore]
        public uint Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SoundUiElement)obj;
            
            Id = castedObj.id;
            Name = castedObj.name;
            HookId = castedObj.hookId;
            File = castedObj.file;
            Volume = castedObj.volume;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (SoundUiElement)parent : new SoundUiElement();
            obj.id = Id;
            obj.name = Name;
            obj.hookId = HookId;
            obj.file = File;
            obj.volume = Volume;
            return obj;
        
        }
    }
}