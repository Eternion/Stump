 


// Generated on 10/06/2013 01:11:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("SuperAreas")]
    public class SuperAreaRecord : ID2ORecord
    {
        private const String MODULE = "SuperAreas";
        public int id;
        public uint nameId;
        public uint worldmapId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint WorldmapId
        {
            get { return worldmapId; }
            set { worldmapId = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (SuperArea)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            WorldmapId = castedObj.worldmapId;
        }
        
        public object CreateObject()
        {
            var obj = new SuperArea();
            
            obj.id = Id;
            obj.nameId = NameId;
            obj.worldmapId = WorldmapId;
            return obj;
        
        }
    }
}