 


// Generated on 08/13/2015 17:50:43
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
    [TableName("Appearances")]
    [D2OClass("Appearance", "com.ankamagames.dofus.datacenter.appearance")]
    public class AppearanceRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Appearances";
        public uint id;
        public uint type;
        public String data;

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
        public uint Type
        {
            get { return type; }
            set { type = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Data
        {
            get { return data; }
            set { data = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Appearance)obj;
            
            Id = castedObj.id;
            Type = castedObj.type;
            Data = castedObj.data;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Appearance)parent : new Appearance();
            obj.id = Id;
            obj.type = Type;
            obj.data = Data;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}