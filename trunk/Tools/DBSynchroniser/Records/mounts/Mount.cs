 


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
    [TableName("Mounts")]
    [D2OClass("Mount")]
    public class MountRecord : ID2ORecord
    {
        public uint id;
        public uint nameId;
        public String look;
        private String MODULE = "Mounts";

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
        
        public virtual object CreateObject()
        {
            
            var obj = new Mount();
            obj.id = Id;
            obj.nameId = NameId;
            obj.look = Look;
            return obj;
        
        }
    }
}