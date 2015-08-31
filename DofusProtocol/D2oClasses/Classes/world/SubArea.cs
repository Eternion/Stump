

// Generated on 08/13/2015 17:13:56
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SubArea", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class SubArea : IDataObject, IIndexedData
    {
        public const String MODULE = "SubAreas";
        public int id;
        [I18NField]
        public uint nameId;
        public int areaId;
        public List<AmbientSound> ambientSounds;
        public List<List<int>> playlists;
        public List<uint> mapIds;
        public Rectangle bounds;
        public List<int> shape;
        public List<uint> customWorldMap;
        public int packId;
        public uint level;
        public Boolean isConquestVillage;
        public Boolean basicAccountAllowed;
        public Boolean displayOnWorldMap;
        public List<uint> monsters;
        public List<uint> entranceMapIds;
        public List<uint> exitMapIds;
        public Boolean capturable;
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public int AreaId
        {
            get { return this.areaId; }
            set { this.areaId = value; }
        }
        [D2OIgnore]
        public List<AmbientSound> AmbientSounds
        {
            get { return this.ambientSounds; }
            set { this.ambientSounds = value; }
        }
        [D2OIgnore]
        public List<List<int>> Playlists
        {
            get { return this.playlists; }
            set { this.playlists = value; }
        }
        [D2OIgnore]
        public List<uint> MapIds
        {
            get { return this.mapIds; }
            set { this.mapIds = value; }
        }
        [D2OIgnore]
        public Rectangle Bounds
        {
            get { return this.bounds; }
            set { this.bounds = value; }
        }
        [D2OIgnore]
        public List<int> Shape
        {
            get { return this.shape; }
            set { this.shape = value; }
        }
        [D2OIgnore]
        public List<uint> CustomWorldMap
        {
            get { return this.customWorldMap; }
            set { this.customWorldMap = value; }
        }
        [D2OIgnore]
        public int PackId
        {
            get { return this.packId; }
            set { this.packId = value; }
        }
        [D2OIgnore]
        public uint Level
        {
            get { return this.level; }
            set { this.level = value; }
        }
        [D2OIgnore]
        public Boolean IsConquestVillage
        {
            get { return this.isConquestVillage; }
            set { this.isConquestVillage = value; }
        }
        [D2OIgnore]
        public Boolean BasicAccountAllowed
        {
            get { return this.basicAccountAllowed; }
            set { this.basicAccountAllowed = value; }
        }
        [D2OIgnore]
        public Boolean DisplayOnWorldMap
        {
            get { return this.displayOnWorldMap; }
            set { this.displayOnWorldMap = value; }
        }
        [D2OIgnore]
        public List<uint> Monsters
        {
            get { return this.monsters; }
            set { this.monsters = value; }
        }
        [D2OIgnore]
        public List<uint> EntranceMapIds
        {
            get { return this.entranceMapIds; }
            set { this.entranceMapIds = value; }
        }
        [D2OIgnore]
        public List<uint> ExitMapIds
        {
            get { return this.exitMapIds; }
            set { this.exitMapIds = value; }
        }
        [D2OIgnore]
        public Boolean Capturable
        {
            get { return this.capturable; }
            set { this.capturable = value; }
        }
    }
}