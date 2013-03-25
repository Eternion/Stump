
// Generated on 03/25/2013 19:24:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundBones")]
    [Serializable]
    public class SoundBones : IDataObject, IIndexedData
    {
        public uint id;
        public List<String> keys;
        public List<List<SoundAnimation>> values;
        public String MODULE = "SoundBones";

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public List<String> Keys
        {
            get { return keys; }
            set { keys = value; }
        }

        public List<List<SoundAnimation>> Values
        {
            get { return values; }
            set { values = value; }
        }

    }
}