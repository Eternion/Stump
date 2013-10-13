 


// Generated on 10/13/2013 12:21:17
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
    public class SubAreaRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
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

        [D2OIgnore]
        [PrimaryKey("Id", false)]
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SubArea)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            AreaId = castedObj.areaId;
            AmbientSounds = castedObj.ambientSounds;
            MapIds = castedObj.mapIds;
            Bounds = castedObj.bounds;
            Shape = castedObj.shape;
            CustomWorldMap = castedObj.customWorldMap;
            PackId = castedObj.packId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (SubArea)parent : new SubArea();
            obj.id = Id;
            obj.nameId = NameId;
            obj.areaId = AreaId;
            obj.ambientSounds = AmbientSounds;
            obj.mapIds = MapIds;
            obj.bounds = Bounds;
            obj.shape = Shape;
            obj.customWorldMap = CustomWorldMap;
            obj.packId = PackId;
            return obj;
        
        }
    }
}