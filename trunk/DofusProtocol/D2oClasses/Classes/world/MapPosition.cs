

// Generated on 10/06/2013 17:58:57
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MapPosition", "com.ankamagames.dofus.datacenter.world")]
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
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public int PosX
        {
            get { return posX; }
            set { posX = value; }
        }
        [D2OIgnore]
        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }
        [D2OIgnore]
        public Boolean Outdoor
        {
            get { return outdoor; }
            set { outdoor = value; }
        }
        [D2OIgnore]
        public int Capabilities
        {
            get { return capabilities; }
            set { capabilities = value; }
        }
        [D2OIgnore]
        public int NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public List<AmbientSound> Sounds
        {
            get { return sounds; }
            set { sounds = value; }
        }
        [D2OIgnore]
        public int SubAreaId
        {
            get { return subAreaId; }
            set { subAreaId = value; }
        }
        [D2OIgnore]
        public int WorldMap
        {
            get { return worldMap; }
            set { worldMap = value; }
        }
        [D2OIgnore]
        public Boolean HasPriorityOnWorldmap
        {
            get { return hasPriorityOnWorldmap; }
            set { hasPriorityOnWorldmap = value; }
        }
    }
}