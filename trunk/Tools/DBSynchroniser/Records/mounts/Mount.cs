 


// Generated on 10/19/2013 17:17:45
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
    [TableName("Mounts")]
    [D2OClass("Mount", "com.ankamagames.dofus.datacenter.mounts")]
    public class MountRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        public uint id;
        public uint nameId;
        public String look;
        private String MODULE = "Mounts";

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
        [NullString]
        public String Look
        {
            get { return look; }
            set { look = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Mount)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Look = castedObj.look;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Mount)parent : new Mount();
            obj.id = Id;
            obj.nameId = NameId;
            obj.look = Look;
            return obj;
        
        }
    }
}