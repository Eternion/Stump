
// Generated on 03/25/2013 19:24:39
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MapPositions")]
    [Serializable]
    public class MapPosition : IDataObject, IIndexedData
    {
        private const String MODULE = "MapPositions";
        public int id;
        public int posX;
        public int posY;
        public Boolean outdoor;
        public int capabilities;
        public int nameId;
        public List<AmbientSound> sounds;
        public int subAreaId;
        public int worldMap;
        public Boolean hasPriorityOnWorldmap;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int PosX
        {
            get { return posX; }
            set { posX = value; }
        }

        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }

        public Boolean Outdoor
        {
            get { return outdoor; }
            set { outdoor = value; }
        }

        public int Capabilities
        {
            get { return capabilities; }
            set { capabilities = value; }
        }

        public int NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public List<AmbientSound> Sounds
        {
            get { return sounds; }
            set { sounds = value; }
        }

        public int SubAreaId
        {
            get { return subAreaId; }
            set { subAreaId = value; }
        }

        public int WorldMap
        {
            get { return worldMap; }
            set { worldMap = value; }
        }

        public Boolean HasPriorityOnWorldmap
        {
            get { return hasPriorityOnWorldmap; }
            set { hasPriorityOnWorldmap = value; }
        }

    }
}