 


// Generated on 09/26/2016 01:50:39
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
    [TableName("SkinPositions")]
    [D2OClass("SkinPosition", "com.ankamagames.dofus.datacenter.appearance")]
    public class SkinPositionRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "SkinPositions";
        public uint id;
        public List<TransformData> transformation;
        public List<String> clip;
        public List<uint> skin;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<TransformData> Transformation
        {
            get { return transformation; }
            set
            {
                transformation = value;
                m_transformationBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_transformationBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] TransformationBin
        {
            get { return m_transformationBin; }
            set
            {
                m_transformationBin = value;
                transformation = value == null ? null : value.ToObject<List<TransformData>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<String> Clip
        {
            get { return clip; }
            set
            {
                clip = value;
                m_clipBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_clipBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ClipBin
        {
            get { return m_clipBin; }
            set
            {
                m_clipBin = value;
                clip = value == null ? null : value.ToObject<List<String>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> Skin
        {
            get { return skin; }
            set
            {
                skin = value;
                m_skinBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_skinBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SkinBin
        {
            get { return m_skinBin; }
            set
            {
                m_skinBin = value;
                skin = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SkinPosition)obj;
            
            Id = castedObj.id;
            Transformation = castedObj.transformation;
            Clip = castedObj.clip;
            Skin = castedObj.skin;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SkinPosition)parent : new SkinPosition();
            obj.id = Id;
            obj.transformation = Transformation;
            obj.clip = Clip;
            obj.skin = Skin;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_transformationBin = transformation == null ? null : transformation.ToBinary();
            m_clipBin = clip == null ? null : clip.ToBinary();
            m_skinBin = skin == null ? null : skin.ToBinary();
        
        }
    }
}