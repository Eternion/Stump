

// Generated on 12/12/2013 16:57:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SkinPosition", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class SkinPosition : IDataObject, IIndexedData
    {
        private const String MODULE = "SkinPositions";
        public uint id;
        public List<TransformData> transformation;
        public List<String> clip;
        public List<uint> skin;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public List<TransformData> Transformation
        {
            get { return transformation; }
            set { transformation = value; }
        }
        [D2OIgnore]
        public List<String> Clip
        {
            get { return clip; }
            set { clip = value; }
        }
        [D2OIgnore]
        public List<uint> Skin
        {
            get { return skin; }
            set { skin = value; }
        }
    }
}