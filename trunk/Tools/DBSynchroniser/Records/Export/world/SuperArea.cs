 


// Generated on 10/19/2013 17:17:46
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
    [TableName("SuperAreas")]
    [D2OClass("SuperArea", "com.ankamagames.dofus.datacenter.world")]
    public class SuperAreaRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "SuperAreas";
        public int id;
        public uint nameId;
        public uint worldmapId;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
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
        public uint WorldmapId
        {
            get { return worldmapId; }
            set { worldmapId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SuperArea)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            WorldmapId = castedObj.worldmapId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (SuperArea)parent : new SuperArea();
            obj.id = Id;
            obj.nameId = NameId;
            obj.worldmapId = WorldmapId;
            return obj;
        
        }
    }
}