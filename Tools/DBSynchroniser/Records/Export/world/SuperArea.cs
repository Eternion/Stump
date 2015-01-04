 


// Generated on 01/04/2015 01:23:49
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
    public class SuperAreaRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "SuperAreas";
        public int id;
        [I18NField]
        public uint nameId;
        public uint worldmapId;
        public Boolean hasWorldMap;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


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

        [D2OIgnore]
        public Boolean HasWorldMap
        {
            get { return hasWorldMap; }
            set { hasWorldMap = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SuperArea)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            WorldmapId = castedObj.worldmapId;
            HasWorldMap = castedObj.hasWorldMap;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SuperArea)parent : new SuperArea();
            obj.id = Id;
            obj.nameId = NameId;
            obj.worldmapId = WorldmapId;
            obj.hasWorldMap = HasWorldMap;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}