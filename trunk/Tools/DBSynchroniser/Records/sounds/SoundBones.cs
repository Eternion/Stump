 


// Generated on 10/06/2013 01:11:00
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("SoundBones")]
    public class SoundBonesRecord : ID2ORecord
    {
        public uint id;
        public List<String> keys;
        public List<List<SoundAnimation>> values;
        public String MODULE = "SoundBones";

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

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
        public byte[] KeysBin
        {
            get { return m_keysBin; }
            set
            {
                m_keysBin = value;
                keys = value == null ? null : value.ToObject<List<String>>();
            }
        }

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
        public byte[] ValuesBin
        {
            get { return m_valuesBin; }
            set
            {
                m_valuesBin = value;
                values = value == null ? null : value.ToObject<List<List<SoundAnimation>>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (SoundBones)obj;
            
            Id = castedObj.id;
            Keys = castedObj.keys;
            Values = castedObj.values;
        }
        
        public object CreateObject()
        {
            var obj = new SoundBones();
            
            obj.id = Id;
            obj.keys = Keys;
            obj.values = Values;
            return obj;
        
        }
    }
}