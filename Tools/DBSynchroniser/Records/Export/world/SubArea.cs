 


// Generated on 08/13/2015 17:50:48
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SubAreas")]
    [D2OClass("SubArea", "com.ankamagames.dofus.datacenter.world")]
    public class SubAreaRecord : ID2ORecord, ISaveIntercepter
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

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
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
        [Ignore]
        public List<AmbientSound> AmbientSounds
        {
            get { return ambientSounds; }
            set
            {
                ambientSounds = value;
                m_ambientSoundsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_ambientSoundsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] AmbientSoundsBin
        {
            get { return m_ambientSoundsBin; }
            set
            {
                m_ambientSoundsBin = value;
                ambientSounds = value == null ? null : value.ToObject<List<AmbientSound>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<List<int>> Playlists
        {
            get { return playlists; }
            set
            {
                playlists = value;
                m_playlistsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_playlistsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] PlaylistsBin
        {
            get { return m_playlistsBin; }
            set
            {
                m_playlistsBin = value;
                playlists = value == null ? null : value.ToObject<List<List<int>>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> MapIds
        {
            get { return mapIds; }
            set
            {
                mapIds = value;
                m_mapIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_mapIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] MapIdsBin
        {
            get { return m_mapIdsBin; }
            set
            {
                m_mapIdsBin = value;
                mapIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public Rectangle Bounds
        {
            get { return bounds; }
            set
            {
                bounds = value;
                m_boundsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_boundsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] BoundsBin
        {
            get { return m_boundsBin; }
            set
            {
                m_boundsBin = value;
                bounds = value == null ? null : value.ToObject<Rectangle>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> Shape
        {
            get { return shape; }
            set
            {
                shape = value;
                m_shapeBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_shapeBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ShapeBin
        {
            get { return m_shapeBin; }
            set
            {
                m_shapeBin = value;
                shape = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> CustomWorldMap
        {
            get { return customWorldMap; }
            set
            {
                customWorldMap = value;
                m_customWorldMapBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_customWorldMapBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] CustomWorldMapBin
        {
            get { return m_customWorldMapBin; }
            set
            {
                m_customWorldMapBin = value;
                customWorldMap = value == null ? null : value.ToObject<List<uint>>();
            }
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
        public Boolean BasicAccountAllowed
        {
            get { return basicAccountAllowed; }
            set { basicAccountAllowed = value; }
        }

        [D2OIgnore]
        public Boolean DisplayOnWorldMap
        {
            get { return displayOnWorldMap; }
            set { displayOnWorldMap = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> Monsters
        {
            get { return monsters; }
            set
            {
                monsters = value;
                m_monstersBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_monstersBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] MonstersBin
        {
            get { return m_monstersBin; }
            set
            {
                m_monstersBin = value;
                monsters = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> EntranceMapIds
        {
            get { return entranceMapIds; }
            set
            {
                entranceMapIds = value;
                m_entranceMapIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_entranceMapIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] EntranceMapIdsBin
        {
            get { return m_entranceMapIdsBin; }
            set
            {
                m_entranceMapIdsBin = value;
                entranceMapIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> ExitMapIds
        {
            get { return exitMapIds; }
            set
            {
                exitMapIds = value;
                m_exitMapIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_exitMapIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ExitMapIdsBin
        {
            get { return m_exitMapIdsBin; }
            set
            {
                m_exitMapIdsBin = value;
                exitMapIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        public Boolean Capturable
        {
            get { return capturable; }
            set { capturable = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SubArea)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            AreaId = castedObj.areaId;
            AmbientSounds = castedObj.ambientSounds;
            Playlists = castedObj.playlists;
            MapIds = castedObj.mapIds;
            Bounds = castedObj.bounds;
            Shape = castedObj.shape;
            CustomWorldMap = castedObj.customWorldMap;
            PackId = castedObj.packId;
            Level = castedObj.level;
            IsConquestVillage = castedObj.isConquestVillage;
            BasicAccountAllowed = castedObj.basicAccountAllowed;
            DisplayOnWorldMap = castedObj.displayOnWorldMap;
            Monsters = castedObj.monsters;
            EntranceMapIds = castedObj.entranceMapIds;
            ExitMapIds = castedObj.exitMapIds;
            Capturable = castedObj.capturable;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SubArea)parent : new SubArea();
            obj.id = Id;
            obj.nameId = NameId;
            obj.areaId = AreaId;
            obj.ambientSounds = AmbientSounds;
            obj.playlists = Playlists;
            obj.mapIds = MapIds;
            obj.bounds = Bounds;
            obj.shape = Shape;
            obj.customWorldMap = CustomWorldMap;
            obj.packId = PackId;
            obj.level = Level;
            obj.isConquestVillage = IsConquestVillage;
            obj.basicAccountAllowed = BasicAccountAllowed;
            obj.displayOnWorldMap = DisplayOnWorldMap;
            obj.monsters = Monsters;
            obj.entranceMapIds = EntranceMapIds;
            obj.exitMapIds = ExitMapIds;
            obj.capturable = Capturable;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_ambientSoundsBin = ambientSounds == null ? null : ambientSounds.ToBinary();
            m_playlistsBin = playlists == null ? null : playlists.ToBinary();
            m_mapIdsBin = mapIds == null ? null : mapIds.ToBinary();
            m_boundsBin = bounds == null ? null : bounds.ToBinary();
            m_shapeBin = shape == null ? null : shape.ToBinary();
            m_customWorldMapBin = customWorldMap == null ? null : customWorldMap.ToBinary();
            m_monstersBin = monsters == null ? null : monsters.ToBinary();
            m_entranceMapIdsBin = entranceMapIds == null ? null : entranceMapIds.ToBinary();
            m_exitMapIdsBin = exitMapIds == null ? null : exitMapIds.ToBinary();
        
        }
    }
}