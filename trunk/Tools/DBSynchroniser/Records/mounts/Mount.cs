 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Mounts")]
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

        public String Look
        {
            get { return look; }
            set { look = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Mount)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Look = castedObj.look;
        }
        
        public object CreateObject()
        {
            var obj = new Mount();
            
            obj.id = Id;
            obj.nameId = NameId;
            obj.look = Look;
            return obj;
        
        }
    }
}