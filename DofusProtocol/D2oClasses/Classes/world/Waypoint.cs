

// Generated on 09/01/2015 11:16:35
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Waypoint", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class Waypoint : IDataObject, IIndexedData
    {
        public const String MODULE = "Waypoints";
        public uint id;
        public uint mapId;
        public uint subAreaId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint MapId
        {
            get { return this.mapId; }
            set { this.mapId = value; }
        }
        [D2OIgnore]
        public uint SubAreaId
        {
            get { return this.subAreaId; }
            set { this.subAreaId = value; }
        }
    }
}