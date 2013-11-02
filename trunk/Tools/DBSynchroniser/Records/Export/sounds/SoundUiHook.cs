 


// Generated on 11/02/2013 14:55:50
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
    [TableName("SoundUiHook")]
    [D2OClass("SoundUiHook", "com.ankamagames.dofus.datacenter.sounds")]
    public class SoundUiHookRecord : ID2ORecord, ISaveIntercepter
    {
        public uint id;
        public String name;
        public String MODULE = "SoundUiHook";

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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SoundUiHook)obj;
            
            Id = castedObj.id;
            Name = castedObj.name;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SoundUiHook)parent : new SoundUiHook();
            obj.id = Id;
            obj.name = Name;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}