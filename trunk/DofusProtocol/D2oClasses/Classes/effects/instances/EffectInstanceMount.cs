

// Generated on 10/06/2013 17:58:53
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceMount", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceMount : EffectInstance
    {
        public float date;
        public uint modelId;
        public uint mountId;
        [D2OIgnore]
        public float Date
        {
            get { return date; }
            set { date = value; }
        }
        [D2OIgnore]
        public uint ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }
        [D2OIgnore]
        public uint MountId
        {
            get { return mountId; }
            set { mountId = value; }
        }
    }
}