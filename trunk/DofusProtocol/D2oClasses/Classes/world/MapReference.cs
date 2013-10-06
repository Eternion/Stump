

// Generated on 10/06/2013 17:58:57
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
        private const String MODULE = "MapReferences";
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
        public int CellId
        {
            get { return cellId; }
            set { cellId = value; }
        }
    }
}