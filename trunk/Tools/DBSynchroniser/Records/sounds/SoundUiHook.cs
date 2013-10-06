 


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
    [TableName("SoundUiHook")]
    [D2OClass("SoundUiHook")]
    public class SoundUiHookRecord : ID2ORecord
    {
        public uint id;
        public String name;
        public String MODULE = "SoundUiHook";

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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SoundUiHook)obj;
            
            Id = castedObj.id;
            Name = castedObj.name;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new SoundUiHook();
            obj.id = Id;
            obj.name = Name;
            return obj;
        
        }
    }
}