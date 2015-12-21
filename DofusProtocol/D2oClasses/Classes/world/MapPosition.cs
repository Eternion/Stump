

// Generated on 12/20/2015 18:01:23
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
        public const String MODULE = "MapPositions";
        public int id;
        public int posX;
        public int posY;
        public Boolean outdoor;
        public int capabilities;
        [I18NField]
        public int nameId;
        public Boolean showNameOnFingerpost;
        public List<AmbientSound> sounds;
        public List<List<int>> playlists;
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public int PosX
        {
            get { return this.posX; }
            set { this.posX = value; }
        }
        [D2OIgnore]
        public int PosY
        {
            get { return this.posY; }
            set { this.posY = value; }
        }
        [D2OIgnore]
        public Boolean Outdoor
        {
            get { return this.outdoor; }
            set { this.outdoor = value; }
        }
        [D2OIgnore]
        public int Capabilities
        {
            get { return this.capabilities; }
            set { this.capabilities = value; }
        }
        [D2OIgnore]
        public int NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public Boolean ShowNameOnFingerpost
        {
            get { return this.showNameOnFingerpost; }
            set { this.showNameOnFingerpost = value; }
        }
        [D2OIgnore]
        public List<AmbientSound> Sounds
        {
            get { return this.sounds; }
            set { this.sounds = value; }
        }
        [D2OIgnore]
        public List<List<int>> Playlists
        {
            get { return this.playlists; }
            set { this.playlists = value; }
        }
        [D2OIgnore]
        public int SubAreaId
        {
            get { return this.subAreaId; }
            set { this.subAreaId = value; }
        }
        [D2OIgnore]
        public int WorldMap
        {
            get { return this.worldMap; }
            set { this.worldMap = value; }
        }
        [D2OIgnore]
        public Boolean HasPriorityOnWorldmap
        {
            get { return this.hasPriorityOnWorldmap; }
            set { this.hasPriorityOnWorldmap = value; }
        }
    }
}