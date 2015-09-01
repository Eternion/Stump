 


// Generated on 09/01/2015 10:48:51
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
    [TableName("Waypoints")]
    [D2OClass("Waypoint", "com.ankamagames.dofus.datacenter.world")]
    public class WaypointRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Waypoints";
        public uint id;
        public uint mapId;
        public uint subAreaId;

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
        public uint MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }

        [D2OIgnore]
        public uint SubAreaId
        {
            get { return subAreaId; }
            set { subAreaId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Waypoint)obj;
            
            Id = castedObj.id;
            MapId = castedObj.mapId;
            SubAreaId = castedObj.subAreaId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Waypoint)parent : new Waypoint();
            obj.id = Id;
            obj.mapId = MapId;
            obj.subAreaId = SubAreaId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}