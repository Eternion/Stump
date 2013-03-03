
// Generated on 03/02/2013 21:17:47
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SubAreas")]
    [Serializable]
    public class SubArea : IDataObject, IIndexedData
    {
        private const String MODULE = "SubAreas";
        public int id;
        public uint nameId;
        public int areaId;
        public List<AmbientSound> ambientSounds;
        public List<uint> mapIds;
        public Rectangle bounds;
        public List<int> shape;
        public List<uint> customWorldMap;
        public int packId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

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

        public int AreaId
        {
            get { return areaId; }
            set { areaId = value; }
        }

        public List<AmbientSound> AmbientSounds
        {
            get { return ambientSounds; }
            set { ambientSounds = value; }
        }

        public List<uint> MapIds
        {
            get { return mapIds; }
            set { mapIds = value; }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public List<int> Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        public List<uint> CustomWorldMap
        {
            get { return customWorldMap; }
            set { customWorldMap = value; }
        }

        public int PackId
        {
            get { return packId; }
            set { packId = value; }
        }

    }
}