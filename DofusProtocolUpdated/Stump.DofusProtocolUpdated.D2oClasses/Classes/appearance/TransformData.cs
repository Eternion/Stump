

// Generated on 12/12/2013 16:57:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("TransformData", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class TransformData
    {
        public const String MODULE = "TransformData";
        [D2OIgnore]
        public string overrideClip
        {
            get { return overrideClip; }
            set { overrideClip = value; }
        }
        [D2OIgnore]
        public string originalClip
        {
            get { return originalClip; }
            set { originalClip = value; }
        }
        [D2OIgnore]
        public int x
        {
            get { return x; }
            set { x = value; }
        }
        [D2OIgnore]
        public int y
        {
            get { return y; }
            set { y = value; }
        }
        [D2OIgnore]
        public int scaleX
        {
            get { return scaleX; }
            set { scaleX = value; }
        }
        [D2OIgnore]
        public int scaleY
        {
            get { return scaleY; }
            set { scaleY = value; }
        }
        [D2OIgnore]
        public int rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
    }
}