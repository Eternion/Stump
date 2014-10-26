

// Generated on 10/26/2014 23:27:54
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MapReference", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class MapReference : IDataObject, IIndexedData
    {
        public const String MODULE = "MapReferences";
        public int id;
        public uint mapId;
        public int cellId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
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
        public int CellId
        {
            get { return this.cellId; }
            set { this.cellId = value; }
        }
    }
}