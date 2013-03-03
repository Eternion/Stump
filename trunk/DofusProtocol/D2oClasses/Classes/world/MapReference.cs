
// Generated on 03/02/2013 21:17:47
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MapReferences")]
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }

        public int CellId
        {
            get { return cellId; }
            set { cellId = value; }
        }

    }
}