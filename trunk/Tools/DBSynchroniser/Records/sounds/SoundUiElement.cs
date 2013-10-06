 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SoundUiElement")]
    [D2OClass("SoundUiElement")]
    public class SoundUiElementRecord : ID2ORecord
    {
        public uint id;
        public String name;
        public uint hookId;
        public String file;
        public uint volume;
        public String MODULE = "SoundUiElement";

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [NullString]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public uint HookId
        {
            get { return hookId; }
            set { hookId = value; }
        }

        [NullString]
        public String File
        {
            get { return file; }
            set { file = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new SoundUiElement();
            obj.id = Id;
            obj.name = Name;
            obj.hookId = HookId;
            obj.file = File;
            obj.volume = Volume;
            return obj;
        
        }
    }
}