

// Generated on 09/01/2015 11:16:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundBones", "com.ankamagames.dofus.datacenter.sounds")]
    [Serializable]
    public class SoundBones : IDataObject, IIndexedData
    {
        public String MODULE = "SoundBones";
        public uint id;
        public List<String> keys;
        public List<List<SoundAnimation>> values;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public List<String> Keys
        {
            get { return this.keys; }
            set { this.keys = value; }
        }
        [D2OIgnore]
        public List<List<SoundAnimation>> Values
        {
            get { return this.values; }
            set { this.values = value; }
        }
    }
}