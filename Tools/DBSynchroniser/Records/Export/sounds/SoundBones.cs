 


// Generated on 09/26/2016 01:50:48
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
    [TableName("SoundBones")]
    [D2OClass("SoundBones", "com.ankamagames.dofus.datacenter.sounds")]
    public class SoundBonesRecord : ID2ORecord, ISaveIntercepter
    {
        public String MODULE = "SoundBones";
        public uint id;
        public List<String> keys;
        public List<List<SoundAnimation>> values;

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
        public List<String> Keys
        {
            get { return keys; }
            set
            {
                keys = value;
                m_keysBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_keysBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] KeysBin
        {
            get { return m_keysBin; }
            set
            {
                m_keysBin = value;
                keys = value == null ? null : value.ToObject<List<String>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<List<SoundAnimation>> Values
        {
            get { return values; }
            set
            {
                values = value;
                m_valuesBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_valuesBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ValuesBin
        {
            get { return m_valuesBin; }
            set
            {
                m_valuesBin = value;
                values = value == null ? null : value.ToObject<List<List<SoundAnimation>>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SoundBones)obj;
            
            Id = castedObj.id;
            Keys = castedObj.keys;
            Values = castedObj.values;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SoundBones)parent : new SoundBones();
            obj.id = Id;
            obj.keys = Keys;
            obj.values = Values;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_keysBin = keys == null ? null : keys.ToBinary();
            m_valuesBin = values == null ? null : values.ToBinary();
        
        }
    }
}