

// Generated on 12/12/2013 16:57:43
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
        public List<uint> mapIds;
        public Rectangle bounds;
        public List<int> shape;
        public List<uint> customWorldMap;
        public int packId;
        public uint level;
        public Boolean isConquestVillage;
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public int AreaId
        {
            get { return areaId; }
            set { areaId = value; }
        }
        [D2OIgnore]
        public List<AmbientSound> AmbientSounds
        {
            get { return ambientSounds; }
            set { ambientSounds = value; }
        }
        [D2OIgnore]
        public List<uint> MapIds
        {
            get { return mapIds; }
            set { mapIds = value; }
        }
        [D2OIgnore]
        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }
        [D2OIgnore]
        public List<int> Shape
        {
            get { return shape; }
            set { shape = value; }
        }
        [D2OIgnore]
        public List<uint> CustomWorldMap
        {
            get { return customWorldMap; }
            set { customWorldMap = value; }
        }
        [D2OIgnore]
        public int PackId
        {
            get { return packId; }
            set { packId = value; }
        }
        [D2OIgnore]
        public uint Level
        {
            get { return level; }
            set { level = value; }
        }
        [D2OIgnore]
        public Boolean IsConquestVillage
        {
            get { return isConquestVillage; }
            set { isConquestVillage = value; }
        }
        [D2OIgnore]
        public Boolean DisplayOnWorldMap
        {
            get { return displayOnWorldMap; }
            set { displayOnWorldMap = value; }
        }
        [D2OIgnore]
        public List<uint> Monsters
        {
            get { return monsters; }
            set { monsters = value; }
        }
        [D2OIgnore]
        public List<uint> EntranceMapIds
        {
            get { return entranceMapIds; }
            set { entranceMapIds = value; }
        }
        [D2OIgnore]
        public List<uint> ExitMapIds
        {
            get { return exitMapIds; }
            set { exitMapIds = value; }
        }
        [D2OIgnore]
        public Boolean Capturable
        {
            get { return capturable; }
            set { capturable = value; }
        }
    }
}