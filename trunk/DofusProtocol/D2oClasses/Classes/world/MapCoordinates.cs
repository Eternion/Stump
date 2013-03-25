
// Generated on 03/25/2013 19:24:39
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MapCoordinates")]
    [Serializable]
    public class MapCoordinates : IDataObject, IIndexedData
    {
        private const String MODULE = "MapCoordinates";
        public uint compressedCoords;
        public List<int> mapIds;

        int IIndexedData.Id
        {
            get { return (int)compressedCoords; }
        }

        public uint CompressedCoords
        {
            get { return compressedCoords; }
            set { compressedCoords = value; }
        }

        public List<int> MapIds
        {
            get { return mapIds; }
            set { mapIds = value; }
        }

    }
}